using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PanzeeBehaviour : MonoBehaviour
{

    private panzeeInventory _pi;
    public UnitCombatNew uc;
    [HideInInspector] public UnitController unitController;
    private GameObject _instrumentEffect = null;
    // 레벨 및 스탯 관련
    public int Level { get; set; } = 1;
    public int Str { get; set; } = 1;
    public int Agi { get; set; } = 1;
    public int Int { get; set; } = 1;
    private int remainingPoint = 0;


    //레벨 당 스탯 양 --- 유물로 업그레이드 가능하게 할거라 퍼블릭으로 두겟습니다.
    public static int StrMaxHP = 5;
    public static float StrRegen = 0.05f;

    public static float AgiAS = 0.05f;

    public static float IntCD = 0.05f;


    //비전투 관련 스탯
    public float RegenRate { get; set; } = 0;
    public float SkillHaste { get; set; } = 0;

    //장비

    [HideInInspector] public ClassType UnitClassType = ClassType.Null;       //클래스 타입
    public int weaponIndex;
    [SerializeField] private SpriteRenderer _equippedImage;
    private Animator _animator;

    [HideInInspector] public SkillBase Skill;


    public delegate void UnitCombatEvent(PanzeeBehaviour uc);
    public event UnitCombatEvent OnSkillUse;
    public event UnitCombatEvent OnEachSecondAlive; // 초당 체력회복등에 사용
    public event UnitCombatEvent OnUnequipItem;


    #region 장비관련
    public void setup(panzeeInventory panInv,ClassType inputWeaponType)
    {
        uc = GetComponent<UnitCombatNew>();

        _pi = panInv;
        _pi.updateStat(DisplayStat.Level, Level);
        _pi.updateStat(DisplayStat.Str, Str);
        _pi.updateStat(DisplayStat.Agi, Agi);
        _pi.updateStat(DisplayStat.Inte, Int);

        UnitClassType = inputWeaponType;
        if (unitController == null) unitController = GetComponent<UnitController>();
        unitController.healthSystem.initPlayer("panzeeDeath0");
        

        UnEquipWeapon();
    }

    public void EquipWeapon(int weaponID)
    {
        if (Weapons.DB.ContainsKey(weaponIndex) && Weapons.DB[weaponIndex].weaponType == WeaponType.Instrument)
        {
            if (weaponIndex == 30)
            {
                Global.AudioManager.RemoveInstrumentPlayer(0);
            }
            else
            {
                Global.AudioManager.RemoveInstrumentPlayer(GetItemRank() + 1);
            }
        }
        weaponIndex = weaponID;
        if (Weapons.DB[weaponIndex].projImage != "" && Weapons.DB[weaponIndex].projImage != "null")
        {
            unitController.unitCombat.AttackImage = Global.ResourceManager.LoadTexture(Weapons.DB[weaponIndex].projImage);
        }
        else
        {
            _equippedImage.sprite = null;
        }
        if (Weapons.DB[weaponIndex].equipImage != "" && Weapons.DB[weaponIndex].equipImage != "null")
        {
            _equippedImage.sprite = Global.ResourceManager.LoadTexture(Weapons.DB[weaponIndex].equipImage);
        }
        else
        {
            _equippedImage.sprite = null;
        }
        uc.TargetFaction = Weapons.DB[weaponIndex].targetFaction;
        uc._impactEffect = Weapons.DB[weaponIndex].impactEffect;
        ChangeEquipAnimation();

        if(Weapons.DB[weaponID].weaponType == WeaponType.Instrument)
        {
            if(weaponIndex == 30)
            {
                Global.AudioManager.AddInstrumentPlayer(0);
            }
            else
            {
                Global.AudioManager.AddInstrumentPlayer(GetItemRank() + 1);
            }
        }

        //공격 사운드
        uc._attackAudio = Weapons.DB[weaponIndex].projSound;
        uc._impactAudio = Weapons.DB[weaponIndex].impctSound;
        
        ChangeSkill();
        UpdateStats();
    }

    /// <summary>
    /// 장비 애니메이션 변경
    /// </summary>
    public void ChangeEquipAnimation()
    {
        WeaponType inputWeaponType = GetWeaponType();
        if (_instrumentEffect != null) Global.ObjectManager.ReleaseObject("soundPulse", _instrumentEffect);
        switch (GetWeaponType())
        {
            case WeaponType.Shield:
                unitController.animator.SetTrigger("Shield");
                break;
            case WeaponType.Bow:
                unitController.animator.SetTrigger("Bow");
                break;
            case WeaponType.Gun:
                unitController.animator.SetTrigger("Gun");
                break;
            case WeaponType.Instrument:
                unitController.animator.SetTrigger("Inst");
                _instrumentEffect = Global.ObjectManager.SpawnObject("soundPulse");
                _instrumentEffect.transform.SetParent(transform);
                _instrumentEffect.transform.localPosition = Vector3.zero;
                break;
            default:
                unitController.animator.SetTrigger("Regular");
                break;
        }
    }
    public void UnEquipWeapon(bool replacing = false)
    {
        OnUnequipItem?.Invoke(this);
        OnUnequipItem = null;
        if (replacing) return;
        if (_animator != null)
        {
            unitController.animator.SetTrigger("Regular");
        }
        _equippedImage.sprite = null;

        switch (UnitClassType)
        {
            case ClassType.Warrior:
            case ClassType.Wak:
                EquipWeapon(10);
                break;
            case ClassType.Shooter:
                EquipWeapon(20);
                break;
            case ClassType.Supporter:
                EquipWeapon(30);
                break;
        }
        UpdateStats();
    }


    public void UpdateStats()
    {
        Dictionary<string, float> classModifier = IngameManager.RelicManager.ClassModifiers[UnitClassType].Modifiers;
        Weapon weaponInfo = Weapons.DB[weaponIndex];
        //+classModifier.Modifiers[""]
        uc.TotalDamage = uc.BaseDamage + weaponInfo.damage + (int)classModifier["Damage"];

        uc.TotalRange = uc.BaseRange + weaponInfo.AttackRange;
        uc.TotalAP = uc.BaseAP + weaponInfo.AP + (int)classModifier["AP"];
        uc.TotalAS = uc.BaseAS + weaponInfo.AttackSpeed + classModifier["AttackSpeed"];
        unitController.healthSystem.totalArmor = unitController.healthSystem.baseArmor + weaponInfo.Armor + (int)classModifier["Armor"];
        unitController.healthSystem.HealthMax = unitController.healthSystem.BaseHP + (int)classModifier["MaxHP"];
        uc.CritChance = uc.BaseCrit + classModifier["CritChance"];
        uc.LifeSteal = uc.BaseLD + classModifier["LifeSteal"];
        unitController.unitStats.MoveSpeed = classModifier["MovementSpeed"];

        uc.TotalAOE = uc.BaseAOE + weaponInfo.AttackArea;
        uc.ProjectileSpeed = weaponInfo.projSpeed;
        uc._heightDelta = weaponInfo.heightDelta;
        uc._torque = weaponInfo.torque;
        uc.TargetFaction = weaponInfo.targetFaction;

        unitController.healthSystem._healthInversed = 1 / unitController.healthSystem.HealthMax;
    }
    public int GetItemRank()
    {
        return weaponIndex % 10;
    }
    public WeaponType GetWeaponType()
    {
        return Weapons.DB[weaponIndex].weaponType;
    }

    /// <summary>
    /// 힘을 찍음
    /// 보너스 스탯: 최대 체력, 체력 회복력
    /// </summary>
    public void RaiseStr()
    {
        if (remainingPoint > 0)
        {
            remainingPoint--;

            Str++;
            unitController.healthSystem.HealthMax += StrMaxHP;
            RegenRate += StrRegen;
            UpdateStats();

            _pi.updateStat(DisplayStat.Str, Str);
            _pi.updateStat(DisplayStat.Mxhealth, unitController.healthSystem.HealthMax);
            _pi.updateStat(DisplayStat.Regen, RegenRate);
        }
    }

    /// <summary>
    /// 민첩을 찍음
    /// 보너스 스탯: 공격속도
    /// </summary>
    public void RaiseAgi()
    {
        if (remainingPoint > 0)
        {
            remainingPoint--;

            Agi++;
            uc.BaseAS += AgiAS;
            UpdateStats();

            _pi.updateStat(DisplayStat.Agi, Agi);
            _pi.updateStat(DisplayStat.AtkSpd, uc.TotalAS);
        }
    }

    /// <summary>
    /// 힘을 찍음
    /// 보너스 스탯: 최대 체력, 체력 회복력
    /// </summary>
    public void RaiseInt()
    {
        if (remainingPoint > 0)
        {
            remainingPoint--;

            Int++;
            UpdateStats();

            _pi.updateStat(DisplayStat.Inte, Int);
            _pi.updateStat(DisplayStat.CD,Skill.TotalCD);
        }
    }
    private void ChangeSkill()
    {
        if (Skill != null)
        {
            Destroy(Skill);
        }
        GameObject skillObject = Instantiate(Global.ObjectManager.SpawnObject("skillcarrier"));
        skillObject.transform.SetParent(transform);
        skillObject.transform.localPosition = Vector3.zero;
        switch (Weapons.DB[weaponIndex].weaponType)
        {
            case WeaponType.Axe:
                Skill = skillObject.AddComponent<Bladestorm>();
                break;
            case WeaponType.Sword:
                Skill = skillObject.AddComponent<Berserk>();
                break;
            case WeaponType.Shield:
                Skill = skillObject.AddComponent<Taunt>();
                break;
            case WeaponType.Bow:
                Skill = skillObject.AddComponent<ArrowRain>();
                break;
            case WeaponType.Gun:
                Skill = skillObject.AddComponent<Snipe>();
                break;
            case WeaponType.Throw:
                Skill = skillObject.AddComponent<Rush>();
                break;
            case WeaponType.Blunt:
                Skill = skillObject.AddComponent<Stun>();
                break;
            case WeaponType.Wand:
                Skill = skillObject.AddComponent<MassHeal>();
                break;
            case WeaponType.Instrument:
                Skill = skillObject.AddComponent<Finale>();
                Skill.GetComponent<PolygonCollider2D>().enabled = true;
                break;
            default:
                break;
        }
    }
    public void useSkill()
    {
        Skill.UseSkill(unitController);
    }
    public void PlaySkillAnim()
    {
        unitController.animator.SetTrigger("Skill");
    }
    #endregion
}
