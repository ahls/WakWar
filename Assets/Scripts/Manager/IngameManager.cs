using UnityEngine;

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
    public static ProgressManager ProgressManager => _progressManager;
    public static DialogueDisplay DialogueDisplay => _dialogueDisplay;
    private static IngameManager _instance;
    private static UnitManager _unitManager;
    private static TwitchClient _twitchClient;
    private static WakgoodBehaviour _wakgoodBehaviour;
    private static UIInventory _uiInventory;
    private static ProgressManager _progressManager;
    private static DialogueDisplay _dialogueDisplay;
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
}
