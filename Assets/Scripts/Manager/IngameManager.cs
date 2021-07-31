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
    public static EncounterManager EncounterManager => _encounterManager;
    public static SkillManager SkillManager => _skillManager;
    public static EnemyManager EnemyManager => _enemyManager;
    public static StageManager StageManager => _stageManager;
    public static RelicManager RelicManager => _relicManager;
    public static PanzeeWindow UIPanzeeWindow { get; set; }
    public static WakWindow UIWakWindow { get; set; }
    public static UIShop UIShop { get; set; }

    private static IngameManager _instance;
    private static UnitManager _unitManager;
    private static TwitchClient _twitchClient;
    private static WakgoodBehaviour _wakgoodBehaviour;
    private static UIInventory _uiInventory;
    private static ProgressManager _progressManager;
    private static EncounterManager _encounterManager;
    private static SkillManager _skillManager;
    private static EnemyManager _enemyManager;
    private static StageManager _stageManager;
    private static RelicManager _relicManager;
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
        _stageManager = new StageManager();
        Global.UIPopupManager.LoadUIs();
    }
    private void Start()
    {
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

    public void SetProgressManager(ProgressManager progressManager)
    {
        _progressManager = progressManager;
    }
    public void SetEncounterManager(EncounterManager encounterManager)
    {
        _encounterManager = encounterManager;
    }
    public void SetSkillManager(SkillManager skillManager)
    {
        _skillManager = skillManager;
    }
    public void SetEnemyManager(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }
    public void SetRelicManager(RelicManager relicManager)
    {
        _relicManager = relicManager;
    }
}
