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
        StartCoroutine(ActiveLifeTime());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator ActiveLifeTime()
    {
        yield return new WaitForSeconds(_lifeTime);
        
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
