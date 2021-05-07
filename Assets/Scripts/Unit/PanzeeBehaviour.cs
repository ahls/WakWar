using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanzeeBehaviour : MonoBehaviour
{

    private panzeeInventory _pi;
    private UnitCombat _uc;

    // 레벨 및 스탯 관련
    public int Level { get; set; } = 1;
    public int Str { get; set; } = 1;
    public int Agi { get; set; } = 1;
    public int Int { get; set; } = 1;
    private int remainingPoint = 0;


    //레벨 당 스탯 양 --- 유물로 업그레이드 가능하게 할거라 퍼블릭으로 두겟습니다.
    public int StrMaxHP = 5;
    public float StrRegen = 0.05f;

    public float AgiAS = 0.05f;

    public float IntCD = 0.05f;


    //비전투 관련 스탯
    public float RegenRate { get; set; } = 0;
    public float SkillHaste { get; set; } = 0;

    public void setup(panzeeInventory panInv)
    {
        _uc = GetComponent<UnitCombat>();

        _pi = panInv;
        _pi.updateStat(DisplayStat.Level, Level);
        _pi.updateStat(DisplayStat.Str, Str);
        _pi.updateStat(DisplayStat.Agi, Agi);
        _pi.updateStat(DisplayStat.Inte, Int);
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
            _uc.HealthMax += StrMaxHP;
            RegenRate += StrRegen;
            _uc.UpdateStats();

            _pi.updateStat(DisplayStat.Str, Str);
            _pi.updateStat(DisplayStat.Mxhealth, _uc.HealthMax);
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
            _uc.BaseAS += AgiAS;
            _uc.UpdateStats();

            _pi.updateStat(DisplayStat.Agi, Agi);
            _pi.updateStat(DisplayStat.AtkSpd, _uc.TotalAS);
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
            _uc.UpdateStats();

            _pi.updateStat(DisplayStat.Inte, Int);
            _pi.updateStat(DisplayStat.CD,_uc.Skill.TotalCD);
        }
    }
}
