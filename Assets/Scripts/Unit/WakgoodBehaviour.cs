﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement; 
public class WakgoodBehaviour : MonoBehaviour
{
    #region 변수
    private int3 _panzees = new int3(0, 0, 0);
    private UnitController unitController;
    public int StatPerUnit { get; set; } = 1;
    public int[] WakStats { get; set; } = new int[3] { 0, 0, 0 }; //힘 민 지 처럼 전사 사수 지원가를 나타냄
    #endregion
    private void Awake()
    {
        unitController= GetComponent<UnitController>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        IngameManager.instance.SetWakgoodBehaviour(this);
        GetComponent<UnitMove>().PlayerUnitInit("우왁굳");
        unitController.panzeeBehavior.UnitClassType = ClassType.Wak;
        unitController.panzeeBehavior.unitController = unitController;
        unitController.panzeeBehavior.UnEquipWeapon();
        //_unitCombat.SoundSetup("wakgoodDeath", "", "");
        IngameManager.UIWakWindow.SetItemSlot();
        DontDestroyOnLoad(gameObject);
    }


    public void AddPanzeeStat(ClassType panzeeClass, int numDelta)
    {
        numDelta *= IngameManager.RelicManager.StatMutliplier;
        switch (panzeeClass)
        {
            case ClassType.Warrior:
                _panzees.x+=numDelta;
                WakStats[0] += numDelta;
                //전사 유닛 추가스탯: 체력, 방어력
                unitController.healthSystem.baseArmor+= numDelta;
                unitController.healthSystem.HealthMax+= numDelta;

                break;
            case ClassType.Shooter:
                _panzees.y += numDelta;
                WakStats[1] += numDelta;
                //사수 유닛 추가스탯: 방어관통, 추뎀
                unitController.unitCombat.BaseAP+= numDelta;
                unitController.unitCombat.BaseDamage+= numDelta;

                break;
            case ClassType.Supporter:
                _panzees.z += numDelta;
                WakStats[2] += numDelta;
                //지원가 유닛 추가스탯:  이속, 공속
                unitController.unitCombat.BaseAS += 0.05f * numDelta;
                unitController.unitCombat.BaseRange += 0.05f * numDelta;
                break;
            default:
                break;
        }
        //Global.UIPopupManager.
        //_unitCombat.UpdateStats();
    }
    private void OnDisable()
    {
        //IngameManager.ProgressManager.EndCombat();
    }
}
