﻿using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }
    public static UnitManager UnitManager => _unitManager;
    public static TwitchClient TwitchClient => _twitchClient;

    private static IngameManager _instance;
    private static UnitManager _unitManager;
    private static TwitchClient _twitchClient;

    private void Awake()
    {
        _instance = this;
    }

    public void SetUnitManager(UnitManager unitManager)
    {
        _unitManager = unitManager;
    }

    public void SetTwitchClient(TwitchClient twitchClient)
    {
        _twitchClient = twitchClient;
    }
}