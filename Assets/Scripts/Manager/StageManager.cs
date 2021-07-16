﻿using System.Collections;
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
