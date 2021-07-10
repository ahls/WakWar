using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    private string _prefabName = "";
    private ParticleSystem _particle = null;
    private Animator _anime = null; 
    private float _lifeTime = 0f;

    private void OnEnable()
    {
        _particle = GetComponent<ParticleSystem>();
        _anime = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _prefabName = "";
        _particle = null;
        _anime = null;
        _lifeTime = 0f;
    }

    public void SetEffectInfo(string name, float lifeTime = 0f)
    {
        _lifeTime = lifeTime;

        if (_lifeTime != 0f)
        {
            StartCoroutine(ActiveLifeTime());
        }
    }

    private IEnumerator ActiveLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);

        DestroyEffect();

        yield return null;
    }

    private void Update()
    {
        if (_particle != null && !_particle.isPlaying)
        {
            DestroyEffect();
        }

        if (_anime != null && _anime.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            DestroyEffect();
        }
    }

    private void DestroyEffect()
    {
        StopAllCoroutines();

        Global.ObjectPoolManager.ObjectPooling(_prefabName, gameObject);
    }
}
