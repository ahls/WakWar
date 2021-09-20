using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private Text _dialogName, _dialogText;
    [SerializeField] private Image _portrait;

    [SerializeField] private Animator _animator;
    private int _dialogID;
    private int _dialogIndex;
    private bool _moreDialog;
    private bool _dialogueActive;
    private readonly HashSet<string> commands = new HashSet<string>() { "Sound", "BlackScreen","ClearScreen", "FadeOut", "FadeIn","Wait" ,"UnitMove","UnitPlay","CameraSet","Open","Close"};

    // Start is called before the first frame update
    void Start()
    {
        Global.UIManager.DialogueDisplay = this;
    }
    /// <summary>
    /// 다음 텍스트가 있으면 true, 마지막 텍스트였을 경우 false 리턴
    /// </summary>
    /// <returns></returns>
    public bool LoadNextText()
    {
        Debug.Log($"current dialogIndex = {_dialogIndex}");
        if(_dialogIndex == Dialogues.DB[_dialogID].Count)// 백신때문에 몸 상태가 안좋아서 이게 맞는 조건문인지 햇갈립니당..
        {
            _dialogueActive = false;
            IngameManager.ProgressManager.NextSequence();
        }
        else
        {
            string namePart = Dialogues.DB[_dialogID][_dialogIndex].NameEntry;
            if (commands.Contains(namePart))
            {
                switch (namePart)
                {
                    case "Sound":
                        Global.AudioManager.PlayOnce(Dialogues.DB[_dialogID][_dialogIndex].TextEntry);
                        break;
                    case "FadeIn":
                    case "FadeOut":
                        Global.UIManager.SceneTransition.Fade(namePart == "FadeIn");
                        break;
                    case "Wait":
                        _dialogueActive = false;
                        StartCoroutine(WaitCommand(float.Parse(Dialogues.DB[_dialogID][_dialogIndex].TextEntry)));
                        return true;
                    case "UnitMove":
                        UnitMoveTo();

                        break;
                    case "UnitPlay":
                        UnitPlayAnim();
                        break;
                    case "CameraSet":
                        CameraSet();
                        break;
                    case "Open":
                    case "Close":
                        ToggleDialogWindow(namePart == "Open");
                        return true;
                    case "BlackScreen":
                    case "ClearScreen":
                        Global.UIManager.SceneTransition.BlackScreen(namePart == "BlackScreen");
                        break;
                }
                _dialogIndex++;
                return LoadNextText();

            }
            _dialogText.text = Dialogues.DB[_dialogID][_dialogIndex].TextEntry;
            _dialogName.text = Dialogues.DB[_dialogID][_dialogIndex].NameEntry;

            _portrait.sprite = Global.ResourceManager.LoadTexture(Dialogues.DB[_dialogID][_dialogIndex].PortraitImage);
            _dialogIndex++;
            _moreDialog = true;
            _dialogueActive = true;
        }
        return true;

    }
    public IEnumerator WaitCommand(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _dialogueActive = true;
        _dialogIndex++;
        LoadNextText();
    }
    private void ToggleDialogWindow(bool open)
    {
        _animator.SetBool("Open",open);
        _dialogIndex++;
        LoadNextText();
    }

    private void UnitMoveTo()
    {
        List<string> inputs = Dialogues.DB[_dialogID][_dialogIndex].TextEntry.Split(',').ToList();

        Vector2 targetLoc = new Vector2(float.Parse(inputs[1]),float.Parse(inputs[2]));
        GameObject go = GameObject.Find(inputs[0]);
        UnitStats unitStats = go.GetComponent<UnitStats>();
        unitStats.MoveToTarget(targetLoc);
    }
    private void UnitPlayAnim()
    {
        List<string> inputs = Dialogues.DB[_dialogID][_dialogIndex].TextEntry.Split(',').ToList();

        GameObject go = GameObject.Find(inputs[0]);
        go.GetComponent<Animator>().SetTrigger(inputs[1]);
    }
    private void CameraSet()
    {
        List<string> inputs = Dialogues.DB[_dialogID][_dialogIndex].TextEntry.Split(',').ToList();
        if(inputs[0] == "Lock")
        {
            IngameManager.instance.GetComponent<CameraControl>().CameraLocked = bool.Parse(inputs[1]);
            return;
        }
        Camera.main.transform.position = new Vector3(float.Parse(inputs[0]), float.Parse(inputs[1]),-10);
        Camera.main.orthographicSize = float.Parse(inputs[2]);
    }
    public void DialogOnClick()
    {
        if (!_dialogueActive || !_moreDialog)
        {
            return;
        }
        _moreDialog = Global.UIManager.DialogueDisplay.LoadNextText();
        if (!_moreDialog)
        {//마지막 대사 후, 다음으로 진행
            _moreDialog = true;
        }
    }

    public void SetupDialogue(int dialogueID)
    {
        _dialogIndex = -1;
        _dialogID = dialogueID;
        _dialogName.text = string.Empty;
        _dialogText.text = string.Empty; 
        ToggleDialogWindow(true);
    }

}
