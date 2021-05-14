﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private const float PITCH_RANGE = 0.15f;
    /// <summary>
    /// duration 에 값을 지정 안하면 기본 클립 길이만큼 재생하고 다시 풀링
    /// 루프로 재생될경우, loop = true, duraion = 길이 로 정해줘야함
    /// </summary>
    /// <param name="soundName"></param>
    /// <param name="duration"></param>
    public void init(string soundName, bool loop = false,float duration = -1, bool randomPitch = false)
    {
        _audioSource.clip = Global.ResourceManager.LoadAudio(soundName);
        _audioSource.loop = loop;
        if(randomPitch)
        {
            _audioSource.pitch = 1 + Random.Range(-PITCH_RANGE, PITCH_RANGE);
        }
        else
        {
            _audioSource.pitch = 1;
        }

        if (loop)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        if (duration == -1)
        {
            StartCoroutine(DelayedPooling(_audioSource.clip.length, loop));
        }
        else
        {
            StartCoroutine(DelayedPooling(duration, loop));
        }
    }
    IEnumerator DelayedPooling(float time, bool wasLoop)
    {
        yield return new WaitForSeconds(time);
        if (wasLoop)            _audioSource.Stop();
        Global.ObjectPoolManager.ObjectPooling(AudioManager.AUDIO_PLAYER_NAME, gameObject);

    }
}