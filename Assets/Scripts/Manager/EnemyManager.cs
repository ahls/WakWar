using System.Collections;
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
        if (!_enemyList.Contains(enemy)) return;
        _enemyList.Remove(enemy);
        IngameManager.UIInventory.AddMoney(IngameManager.RelicManager.GoldPerKill);
        Debug.Log($"remaining enemyies: {_enemyList.Count}");
        if(_enemyList.Count == 0)
        {
            Debug.Log("All enemies are killed");
            IngameManager.ProgressManager.NextSequence();
        }
    }
}
