using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> _enemyList;

    private void Awake()
    {
        IngameManager.instance.SetEnemyManager(this);
    }

    public void SpawnEnemy(int id, Vector2 pos)
    {
        var enemyObject = Global.ResourceManager.LoadPrefab(EnemyDatas.DB[id].Name);
        enemyObject.transform.parent = this.transform;
        enemyObject.transform.position = pos;

        _enemyList.Add(enemyObject);
    }

    public void KillAllEnemys()
    {
        foreach (var enemy in _enemyList)
        {
            // 여기에 적 이름 넣어줘야함 적 이름을 저장하고 있는 스크립트가 필요
            //Global.ObjectPoolManager.ObjectPooling(, enemy);
        }
    }
}
