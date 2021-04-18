using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class WakgoodBehaviour : MonoBehaviour
{
    #region 함수
    private int3 _panzees = new int3(0, 0, 0);
    private UnitCombat _unitCombat;
    public int StatPerUnit { get; set; } = 1;
    public int[] WakStats { get; set; } = new int[3] { 0, 0, 0 };
    #endregion
    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetWakgoodBehaviour(this);
        GetComponent<UnitStats>().PlayerUnitInit("우왁굳");
        _unitCombat.weaponType = WeaponType.Wak;
    }

    private void Awake()
    {
        _unitCombat = GetComponent<UnitCombat>();
    }

    public void AddPanzeeStat(WeaponType panzeeClass, int numDelta)
    {
        switch (panzeeClass)
        {
            case WeaponType.Warrior:
                _panzees.x+=numDelta;
                WakStats[0] += numDelta;
                //전사 유닛 추가스탯: 체력, 방어력
                _unitCombat.armor+= numDelta;
                _unitCombat.healthMax+= numDelta;

                break;
            case WeaponType.Shooter:
                _panzees.y += numDelta;
                WakStats[1] += numDelta;
                //사수 유닛 추가스탯: 방어관통, 추뎀
                _unitCombat.armorPiercing+= numDelta;
                _unitCombat.attackDamage+= numDelta;

                break;
            case WeaponType.Supporter:
                _panzees.z += numDelta;
                WakStats[2] += numDelta;
                //지원가 유닛 추가스탯:  사거리, 공속
                _unitCombat.attackSpeed += 0.05f * numDelta;
                _unitCombat.attackRange += 0.05f * numDelta;
                break;
            default:
                break;
        }
        //_unitCombat.UpdateStats();
    }
}
