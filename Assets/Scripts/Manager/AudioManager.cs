using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager
{
    public const string AUDIO_PLAYER_NAME = "AudioPlayer";
    // Start is called before the first frame update

    public void PlayOnce(string audioName)
    {
        AudioPlayer audioPlayer = Global.ResourceManager.LoadPrefab(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.init(audioName);
    }
    public void PlayOnceAt(string audioName,Vector2 location)
    {
        AudioPlayer audioPlayer = Global.ResourceManager.LoadPrefab(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.transform.position = location;
        audioPlayer.init(audioName);
    }
    public void PlayLoop(string audioName, float duration)
    {
        AudioPlayer audioPlayer = Global.ResourceManager.LoadPrefab(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.init(audioName, true, duration);
    }
    
}
