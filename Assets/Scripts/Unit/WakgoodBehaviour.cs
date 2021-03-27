using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public class WakgoodBehaviour : MonoBehaviour
{
    #region 함수
    private int3 _panzees = new int3(0, 0, 0);
    private UnitCombat _unitCombat;
    public int statPerUnit { get; set; } = 1;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        _unitCombat = GetComponent<UnitCombat>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPanzeeStat(WeaponType panzeeClass, int numDelta)
    {
        switch (panzeeClass)
        {
            case WeaponType.Warrior:
                _panzees.x+=numDelta;
                WakWindow.instance.updateStat(panzeeClass, _panzees.x);
                //전사 유닛 추가스탯: 체력, 방어력
                _unitCombat.armor+= numDelta;
                _unitCombat.healthMax+= numDelta;

                break;
            case WeaponType.Shooter:
                _panzees.y += numDelta;
                WakWindow.instance.updateStat(panzeeClass, _panzees.y);
                //사수 유닛 추가스탯: 방어관통, 추뎀
                _unitCombat.armorPiercing+= numDelta;
                _unitCombat.attackDamage+= numDelta;

                break;
            case WeaponType.Supporter:
                _panzees.z += numDelta;
                WakWindow.instance.updateStat(panzeeClass, _panzees.z);
                //지원가 유닛 추가스탯:  사거리, 공속
                _unitCombat.attackSpeed += 0.05f * numDelta;
                _unitCombat.attackRange += 0.05f * numDelta;
                break;
            default:
                break;
        }
        _unitCombat.UpdateStats();
    }
}
