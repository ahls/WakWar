using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SceneTransition : MonoBehaviour
{

    [SerializeField] private Animator _transitionScreen;
    private bool _toNextProgress = false;
    /// <summary>
    /// 가능한 종류 black,clear, fadein, fadeout, open, close
    /// </summary>
    /// <param name="animationName"></param>
    public void PlayAnimation(string animationName, bool NextProgress = false, bool NextDialog = false)
    {

        _transitionScreen.SetTrigger(animationName);
        if(NextProgress)
        {
            _toNextProgress = NextProgress;
        }
        if(NextDialog)
        {
            throw new System.Exception("Next dialog is not implemented");
        }
    }
    public void ToNextStep()
    {
        if (IngameManager.ProgressManager == null) return;
        IngameManager.ProgressManager.NextSequence();
    }
    public void BlackScreen(bool black)
    {
        if (black)
            _transitionScreen.SetTrigger("black");
        else
            _transitionScreen.SetTrigger("clear");
    }
    public void Fade(bool IN)
    {
        if (IN)
            _transitionScreen.SetTrigger("fadein");
        else
            _transitionScreen.SetTrigger("fadeout");
    }
}
