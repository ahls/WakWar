using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    public int CurrentStageFloor { get; private set; } = 0;
    public int CurrentStageRoom { get; private set; } = 0;
    private UnitSpawnLocations _unitSpawnLocations;
    public void SetStage(int id)
    {
        IngameManager.EnemyManager.KillAllEnemys();

        foreach (var enemy in Stages.DB[id].EnemyDatas)
        {
            Vector2 enemyPos = new Vector2(enemy.Position_x, enemy.Position_y);
            IngameManager.EnemyManager.SpawnEnemy(enemy.EnemyID, enemyPos);
        }
        foreach(var panzee in IngameManager.UnitManager.AllPlayerUnits)
        {
            switch (panzee.GetComponent<UnitCombat>().GetWeaponType())
            {
                case WeaponType.Sword:
                    panzee.transform.position = _unitSpawnLocations.Sword.position;
                    break;
                case WeaponType.Axe:
                    panzee.transform.position = _unitSpawnLocations.Axe.position;
                    break;
                case WeaponType.Shield:
                    panzee.transform.position = _unitSpawnLocations.Shield.position;
                    break;
                case WeaponType.Bow:
                    panzee.transform.position = _unitSpawnLocations.Bow.position;
                    break;
                case WeaponType.Gun:
                    panzee.transform.position = _unitSpawnLocations.Gun.position;
                    break;
                case WeaponType.Throw:
                    panzee.transform.position = _unitSpawnLocations.Throw.position;
                    break;
                case WeaponType.Blunt:
                    panzee.transform.position = _unitSpawnLocations.Blunt.position;
                    break;
                case WeaponType.Wand:
                    panzee.transform.position = _unitSpawnLocations.Heal.position;
                    break;
                case WeaponType.Instrument:
                    panzee.transform.position = _unitSpawnLocations.Inst.position;
                    break;
            }


        }
        IngameManager.WakgoodBehaviour.transform.position = _unitSpawnLocations.Wak.position;
        IngameManager.ProgressManager.NextSequence();
    }
    public void SetSpawnLocations(UnitSpawnLocations unitSpawn)
    {
        if(_unitSpawnLocations != null)
        {
            MonoBehaviour.Destroy(_unitSpawnLocations);
        }
        _unitSpawnLocations = unitSpawn;
    }

}
