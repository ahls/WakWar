using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] private string _prefabName;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private Animator _anime;
    [SerializeField] private float _lifeTime = 0f;

    private void OnEnable()
    {
        if(_particle != null)
        {
            _particle.Play();
        }
        if (_anime != null)
        {
            _anime.SetTrigger("Play");
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void PlayAnimation(float duration = -1)
    {
        StartCoroutine(ActiveLifeTime(duration));
    }
    private IEnumerator ActiveLifeTime(float duration)
    {
        if(duration == -1)
        {
            yield return new WaitForSeconds(_lifeTime);
        }
        else
        {
            yield return new WaitForSeconds(duration);
        }
        DestroyEffect();

        yield return null;
    }

    private void DestroyEffect()
    {
        StopAllCoroutines();

        Global.ObjectManager.ReleaseObject(_prefabName, gameObject);
        gameObject.SetActive(false);
    }
}
