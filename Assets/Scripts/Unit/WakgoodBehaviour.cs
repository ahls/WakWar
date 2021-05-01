using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement; 
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
        _unitCombat.weaponType = ClassType.Wak;
    }

    private void Awake()
    {
        _unitCombat = GetComponent<UnitCombat>();
    }

    public void AddPanzeeStat(ClassType panzeeClass, int numDelta)
    {
        switch (panzeeClass)
        {
            case ClassType.Warrior:
                _panzees.x+=numDelta;
                WakStats[0] += numDelta;
                //전사 유닛 추가스탯: 체력, 방어력
                _unitCombat.BaseArmor+= numDelta;
                _unitCombat.HealthMax+= numDelta;

                break;
            case ClassType.Shooter:
                _panzees.y += numDelta;
                WakStats[1] += numDelta;
                //사수 유닛 추가스탯: 방어관통, 추뎀
                _unitCombat.BaseAP+= numDelta;
                _unitCombat.BaseDamage+= numDelta;

                break;
            case ClassType.Supporter:
                _panzees.z += numDelta;
                WakStats[2] += numDelta;
                //지원가 유닛 추가스탯:  이속, 공속
                _unitCombat.BaseAS += 0.05f * numDelta;
                _unitCombat.BaseRange += 0.05f * numDelta;
                break;
            default:
                break;
        }
        //_unitCombat.UpdateStats();
    }
    private void OnDisable()
    {
        //IngameManager.ProgressManager.EndCombat();
    }
}
