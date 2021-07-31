using System.Collections;
using System.Collections.Generic;
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
        if(_dialogIndex == Dialogues.DB[_dialogID].Count)// 백신때문에 몸 상태가 안좋아서 이게 맞는 조건문인지 햇갈립니당..
        {
            _animator.SetBool("Open", false);
            _dialogueActive = false;
            return false;
        }
        else
        {
            _dialogText.text = Dialogues.DB[_dialogID][_dialogIndex].TextEntry;
            _dialogName.text = Dialogues.DB[_dialogID][_dialogIndex].NameEntry;
            _portrait.sprite = Global.ResourceManager.LoadTexture(Dialogues.DB[_dialogID][_dialogIndex].PortraitImage);
            _dialogIndex++;
            _moreDialog = true;
            _dialogueActive = true;
        }
        return true;

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
            IngameManager.ProgressManager.NextSequence();
        }
    }

    public void SetDialogue(int dialogueID)
    {
        _dialogIndex = 0;
        _dialogID = dialogueID;
        _dialogName.text = string.Empty;
        _dialogText.text = string.Empty;
        _animator.SetBool("Open", true);
        LoadNextText();
    }

}
