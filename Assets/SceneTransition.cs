using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneTransition : MonoBehaviour
{
    public enum TransitionType
    {
        horizontal
    }
    [SerializeField] private Animator _transitionScreen;

    public void playAnimation(TransitionType whichTransition, bool opening)
    {

        /*
        string openOrClose = opening ? "_open" : "_close";
        _transitionScreen.SetTrigger(whichTransition.ToString() + openOrClose);
        */
    }
    public void playAnimation(bool opening)
    {

        string openOrClose = opening ? "open" : "close";
        _transitionScreen.SetTrigger(openOrClose);
    }
    public void ToNextStep()
    {
        if (IngameManager.ProgressManager == null) return;
        IngameManager.ProgressManager.NextSequence();
    }
}
