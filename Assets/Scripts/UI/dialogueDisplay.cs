using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private RectTransform _bubbleTail;
    [SerializeField] private Animator _animator;
    private int _dialogID;
    private int _dialogIndex;

    // Start is called before the first frame update
    void Start()
    {
        if (IngameManager.DialogueDisplay == null)
        {
            IngameManager.instance.SetDialogue(this);
            DontDestroyOnLoad(gameObject);
        }
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
            return false;
        }
        else
        {
            _text.text = Dialogues.DB[_dialogID][_dialogIndex].TextEntry;
            _bubbleTail.localPosition = new Vector2(Dialogues.DB[_dialogID][_dialogIndex].TailLocation, -100);
            _dialogIndex++;
        }
        return true;

    }
    public void SetDialogue(int dialogueID)
    {
        _dialogIndex = 0;
        _dialogID = dialogueID;
        _text.text = "";
        _animator.SetBool("Open", true);
        LoadNextText();
    }

}
