using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CurrentEvent { Dialog, StartCombat, EndCombat, LoadScene, LoadStage, RoomReward, BossReward, DisplayReady, FadeOut, FadeIn }
public class ProgressManager : MonoBehaviour
{
    private static bool _dialogTurn;//트루면 현재 진행상황이 대사를 출력중
    private static int _currentProgressIndex = -1;
    public bool IsFirstScene = false;
    // Start is called before the first frame update
    void Start()
    {
            IngameManager.instance.SetProgressManager(this);
        
        NextSequence();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            NextSequence();
        }

    }


    //현재 시퀀스  = 불러옴
    //현재 시퀀스가 컴뱃일 경우,
    //그에 맞는 컴뱃을 셋업
    //현재 시퀀스가 대사일경우,
    //IngameMnaager.DialogueDisplay 에 값을 지정하고 불러옴

    public void NextSequence()
    {
        _currentProgressIndex++;

        Debug.Log($"Current progress is: {ProgressSequences.DB[_currentProgressIndex].CurrentProgressEvent}");
        switch (ProgressSequences.DB[_currentProgressIndex].CurrentProgressEvent)   
        {
            case CurrentEvent.Dialog:
                _dialogTurn = true;
                Global.UIManager.DialogueDisplay.SetupDialogue(int.Parse(ProgressSequences.DB[_currentProgressIndex].value));
                break;
            case CurrentEvent.LoadScene:
                Global.UIPopupManager.PopAll();
                SceneManager.sceneLoaded += IngameManager.UnitManager.OnSceneLoaded;
                //IngameManager.UnitManager.OnSceneChange();
                SceneManager.LoadScene(ProgressSequences.DB[_currentProgressIndex].value);
                break;
            case CurrentEvent.LoadStage:
                Global.UIPopupManager.PopAll();
                IngameManager.StageManager.SetStage(int.Parse(ProgressSequences.DB[_currentProgressIndex].value));
                break;
            case CurrentEvent.StartCombat:
                StartCombat();
                break;
            case CurrentEvent.EndCombat:
                StartCoroutine(EndCombat());
                break;
            case CurrentEvent.RoomReward:
                Global.UIPopupManager.Push(PopupID.UIReward, int.Parse(ProgressSequences.DB[_currentProgressIndex].value));
                break;
            case CurrentEvent.BossReward:
                break;
            case CurrentEvent.DisplayReady:
                Global.UIManager.ShowReadyButton();
                break;
            case CurrentEvent.FadeIn:
                Global.UIManager.SceneTransition.PlayAnimation("open", true);
                break;
            case CurrentEvent.FadeOut:
                Global.UIManager.SceneTransition.PlayAnimation("close", true);
                break;
        }
    }

    private void StartCombat()
    {//AI 및 조작 작동
        IngameManager.UnitManager.ControlOn = true;
        UnitCombat.AIenabled = true;
        Global.UIManager.ToggleMenu(false);
    }

    /// <summary>
    /// 지금 NextSequence가 Recursive 로 돌아가서, EndCombat일떄 코루틴으로 넣어서 끊기도록 했습니다.
    /// </summary>
    /// <returns></returns>
    public IEnumerator  EndCombat()
    {//AI 및 조작 종료
        Debug.Log("End of Combat sequence called");
        IngameManager.UnitManager.ControlOn = false;
        UnitCombat.AIenabled = false;
        yield return new WaitForSeconds(1);
        Global.UIManager.ToggleMenu(true);
        Debug.Log("Next sequence is being called from the EndCombat");
        NextSequence();

    }
    public void SetProgressIndex(int index)
    {
        _currentProgressIndex = index - 1;
    }
    public void Restart()
    {
        _currentProgressIndex = -1;
    }
}
