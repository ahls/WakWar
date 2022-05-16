using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public enum AudioType { sfx,bgm,none}
public class AudioManager
{
    private AudioPlayer _bgmPlayer;
    private AudioPlayer[] _instrumPlayers = new AudioPlayer[5];
    private ushort[] _numInstrumPlayers = { 0, 0, 0, 0, 0 };

    public Dictionary<AudioType, float> volume;


    public const string BGM_NAME = "bgm";
    public const string AUDIO_PLAYER_NAME = "AudioPlayer";
    // Start is called before the first frame update
    public AudioManager()
    {
        Debug.Log("AUDIO MANAGER IS CALLED");
        volume = new Dictionary<AudioType, float>();
        volume[AudioType.bgm] = 1;
        volume[AudioType.sfx] = 1;
        volume[AudioType.none] = 1;

        _bgmPlayer = Global.ObjectManager.SpawnObject(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        _bgmPlayer.initLoop(BGM_NAME);
        _bgmPlayer.SetVolume(volume[AudioType.bgm]);
        for (int i = 0; i < 5; i++)
        {
            _instrumPlayers[i] = Global.ObjectManager.SpawnObject(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
            _instrumPlayers[i].initLoop($"instrument_{i}");
            _instrumPlayers[i].SetVolume(0);
        }
    }
    public void PlayOnce(string audioName, bool randomPitch = false, AudioType audioType = AudioType.sfx)
    {
        if(audioName == "")
        {
            return;
        }
        AudioPlayer audioPlayer = Global.ObjectManager.SpawnObject(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.init(audioName,audioType ,randomPitch:randomPitch);
    }
    public void PlayOnceAt(string audioName,Vector2 location, bool randomPitch =false, AudioType audioType = AudioType.sfx)
    {
        if (audioName == "")
        {
            return;
        }
        AudioPlayer audioPlayer = Global.ObjectManager.SpawnObject(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.transform.position = location;
        audioPlayer.init(audioName, audioType, randomPitch: randomPitch);
    }
    public void PlayLoop(string audioName, float duration, AudioType audioType = AudioType.sfx)
    {
        AudioPlayer audioPlayer = Global.ObjectManager.SpawnObject(AUDIO_PLAYER_NAME).GetComponent<AudioPlayer>();
        audioPlayer.init(audioName, audioType,true, duration);
    }
    
    public void AddInstrumentPlayer(int itemRank)
    {
        Debug.Log($"instruemnt for {itemRank} has been added");
        if (++_numInstrumPlayers[itemRank] ==1 )
        {
            _instrumPlayers[itemRank].SetVolume(volume[AudioType.bgm]);
        }
    }
    public void RemoveInstrumentPlayer(int itemRank)
    {
        Debug.Log($"instruemnt for {itemRank} has been removed");
        _numInstrumPlayers[itemRank] = (ushort)Mathf.Max((int)_numInstrumPlayers[itemRank] - 1, 0);
        if (_numInstrumPlayers[itemRank] == 0)
        {
            _instrumPlayers[itemRank].SetVolume(0);
        }
    }
    public void SetInstrumPitch(int which)
    {
        _instrumPlayers[which].SetPitch(2);
    }
    public void SetVolumeBGM(float value)
    {
        volume[AudioType.bgm] = value;
        _bgmPlayer.SetVolume(volume[AudioType.bgm]);
        for (int i = 0; i < 5; i++)
        {
            if(_numInstrumPlayers[i] > 0)
            {
                _instrumPlayers[i].SetVolume(volume[AudioType.bgm]);
            }
        }
    }
}
