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

    public void PlayAnimation(string animationName)
    {

        _transitionScreen.SetTrigger(animationName);
    }
    public void ToNextStep()
    {
        if (IngameManager.ProgressManager == null) return;
        IngameManager.ProgressManager.NextSequence();
    }
}
