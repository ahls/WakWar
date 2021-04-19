using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CurrentEvent { Dialog, StartCombat, EndCombat, LoadStage }
public class ProgressManager : MonoBehaviour
{
    private bool _dialogTurn;//트루면 현재 진행상황이 대사를 출력중
    private int _currentProgressIndex = -1;
    // Start is called before the first frame update
    void Start()
    {
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
        switch (ProgressSequences.DB[_currentProgressIndex].CurrentProgressEvent)   
        {
            case CurrentEvent.Dialog:
                _dialogTurn = true;
                IngameManager.DialogueDisplay.SetDialogue(ProgressSequences.DB[_currentProgressIndex].value);
                break;
            case CurrentEvent.LoadStage:
                break;
            case CurrentEvent.StartCombat:
                StartCombat();
                break;
            case CurrentEvent.EndCombat:
                EndCombat();
                break;
        }
    }
    public void DialogOnClick()
    {
        if (!_dialogTurn)
        {
            return;
        }
        _dialogTurn = IngameManager.DialogueDisplay.LoadNextText();
        if (!_dialogTurn)
        {//마지막 대사 후, 다음으로 진행
            NextSequence();
        }
    }
    private void StartCombat()
    {//AI 및 조작 작동
        IngameManager.UnitManager.ControlOn = true;
    }

    private void EndCombat()
    {//AI 및 조작 종료
        IngameManager.UnitManager.ControlOn = false;

    }

    public void Restart()
    {
        _currentProgressIndex = 0;
    }
}
