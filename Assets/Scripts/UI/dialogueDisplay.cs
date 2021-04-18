using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dialogueDisplay : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private RectTransform _bubbleTail;
    private int _dialogID;
    private int _dialogIndex;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// 다음 텍스트가 있으면 true, 마지막 텍스트였을 경우 false 리턴
    /// </summary>
    /// <returns></returns>
    public bool LoadNextText()
    {
        
        if(_dialogIndex == Dialogues.DB[_dialogID].Count)
        {
            return false;
        }
        else
        {
            _text.text = Dialogues.DB[_dialogID][_dialogIndex].TextEntry;
            _bubbleTail.position = new Vector2(Dialogues.DB[_dialogID][_dialogIndex].TailLocation, -100);
            _dialogIndex++;
        }
        return true;

    }
    public void SetNextDialogue(int dialogueID)
    {
        _dialogIndex = 0;
        _dialogID = dialogueID;
    }

}
