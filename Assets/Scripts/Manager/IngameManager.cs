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
    public static WakgoodBehaviour WakgoodBehaviour => _wakgoodBehaviour;
    public static UIInventory UIInventory => _uiInventory;
    public static UIShop UIShop => _uiShop;
    public static ProgressManager ProgressManager => _progressManager;
    public static DialogueDisplay DialogueDisplay => _dialogueDisplay;
    public static EncounterManager encounterManager => _encounterManager;


    private static IngameManager _instance;
    private static UnitManager _unitManager;
    private static TwitchClient _twitchClient;
    private static WakgoodBehaviour _wakgoodBehaviour;
    private static UIInventory _uiInventory;
    private static UIShop _uiShop;
    private static ProgressManager _progressManager;
    private static DialogueDisplay _dialogueDisplay;
    private static EncounterManager _encounterManager;
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

    public void SetWakgoodBehaviour(WakgoodBehaviour wakgoodBehaviour)
    {
        _wakgoodBehaviour = wakgoodBehaviour;
    }
    public void SetInventory(UIInventory inventory)
    {
        _uiInventory = inventory;
    }
    public void SetDialogue(DialogueDisplay dialogue)
    {
        _dialogueDisplay = dialogue;
    }
    public void SetProgressManager(ProgressManager progressManager)
    {
        _progressManager = progressManager;
    }
    public void SetEncounterManager(EncounterManager encounterManager)
    {
        _encounterManager = encounterManager;
    }
    public void SetShop(UIShop uiShop)
    {
        _uiShop = uiShop;
    }
}
