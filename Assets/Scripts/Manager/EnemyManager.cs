﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> _enemyList = new List<GameObject>();

    private void Start()
    {
        IngameManager.instance.SetEnemyManager(this);
    }

    public void SpawnEnemy(int id, Vector2 pos)
    {
        var enemyObject = Global.ObjectManager.SpawnObject(EnemyDatas.DB[id].Name);
        enemyObject.transform.parent = this.transform;
        enemyObject.transform.position = pos;

        _enemyList.Add(enemyObject);
    }

    public void KillAllEnemys()
    {
        foreach (var enemy in _enemyList)
        {
            Global.ObjectPoolManager.ObjectPooling(enemy.GetComponent<EnemyBehaviour>().UnitName, enemy);
        }
    }
    public void EnemyDeath(GameObject enemy)
    {
        _enemyList.Remove(enemy);
        if(_enemyList.Count == 0)
        {
            IngameManager.ProgressManager.NextSequence();
        }
    }
}
