using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private bool _dialogTurn;//트루면 현재 진행상황이 대사를 출력중
    private int _currentProgressIndex;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {

            IngameManager.DialogueDisplay.SetDialogue(1001);
            IngameManager.DialogueDisplay.LoadNextText();
            _dialogTurn = true;
        }
        
    }
    public void DialogOnClick()
    {
        if(!_dialogTurn)
        {
            return;
        }
        _dialogTurn = IngameManager.DialogueDisplay.LoadNextText();
    } 

    //현재 시퀀스  = 불러옴
    //현재 시퀀스가 컴뱃일 경우,
    //그에 맞는 컴뱃을 셋업
    //현재 시퀀스가 대사일경우,
    //IngameMnaager.DialogueDisplay 에 값을 지정하고 불러옴


    public void Restart()
    {
        _currentProgressIndex = 0;
    }
}
