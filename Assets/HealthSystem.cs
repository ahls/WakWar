using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class HealthSystem : MonoBehaviour
{
    [HideInInspector] public UnitController unitController;

    //체력
    public int BaseHP = 10;
    public int HealthMax { get; set; }
    public int _healthCurrent;
    public float _healthInversed;
    public bool IsDead { get; set; } = false;
    public bool CanBeKilled { get; set; } = true;
    public Faction OwnedFaction;
    [SerializeField] private Slider _healthBar;


    //방어력
    public int baseArmor { get; set; }
    public int totalArmor { get; set; }

    [SerializeField] private PanzeeBehaviour panzeeBehavior; //팬치 비헤이비어 없으면 무시


    private string _deathSound = string.Empty;

    public Delegates.SimpleIntDelegate OnUnitTakeDamage;
    public Delegates.SimpleDelegate OnUnitDeath;

    private Coroutine _aliveSecondCoroutine;
    private void Awake()
    {
        _healthCurrent = HealthMax;
        _healthBar.maxValue = HealthMax;
        _healthBar.value = _healthCurrent;
    }

    public void initPlayer(string deathSound)
    {
        OwnedFaction = Faction.Player;
        HealthBarColor(Color.green);
        _deathSound = deathSound;
    }
    public void TakeDamage(int damageAmount)
    {
        int totalDamage = damageAmount;

        if (IsDead)
        {
            return;
        }

        _healthCurrent -= (damageAmount);
        OnUnitTakeDamage?.Invoke(totalDamage);

        if (_healthCurrent <= 0)
        {
            if (CanBeKilled == false)
            {
                _healthCurrent = 1;
            }
            else
            {
                Death();
            }
        }
        HealthBarUpdate();
    }
    public void TakeDamage(int dmg, int armorPierce)
    {
        if (dmg > 0)
        {
            TakeDamage(dmg - Mathf.Clamp(totalArmor - armorPierce, 0, (dmg - 1)));
        }
        else if (dmg < 0)
        {
            Heal(-dmg);
        }

    }
    public void Heal(int amount, string effect = "HealEffect")
    {
        _healthCurrent = Mathf.Min(_healthCurrent + amount, HealthMax);
        GameObject healEffect = Global.ObjectManager.SpawnObject(effect);
        healEffect.transform.position = transform.position;
    }

    private void HealthBarUpdate()
    {
        _healthBar.value = _healthCurrent;
    }

    public void HealthBarColor(Color newColor)
    {
        _healthBar.transform.GetChild(0).GetComponent<Image>().color = newColor;
    }
    public void Death()
    {
        GetComponent<Animator>().SetTrigger("Die");

        if (_aliveSecondCoroutine != null) StopCoroutine(_aliveSecondCoroutine);
        IsDead = true;
        GetComponent<Collider2D>().enabled = false;
        Global.AudioManager.PlayOnceAt(_deathSound, transform.position, true);
        IngameManager.UnitManager.DeselectUnit(gameObject);
        IngameManager.EnemyManager.EnemyDeath(gameObject);
        if (unitController.panzeeBehavior.UnitClassType == ClassType.Wak)
        {
            Global.UIManager.GameOver();
        }
        StartCoroutine(DeathDelay());
        OnUnitDeath?.Invoke();
        OnUnitDeath = null;
    }

    public int GetMissingHealth()
    {
        return HealthMax - _healthCurrent;
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
