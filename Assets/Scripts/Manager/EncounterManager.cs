using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    private List<GameObject> _enemyList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        IngameManager.instance.SetEncounterManager(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void EnemyDisabled(GameObject disabledEnemy)
    {
        _enemyList.Remove(disabledEnemy);
        if (_enemyList.Count == 0)
        {
            IngameManager.ProgressManager.NextSequence();
        }
    }
    public void AddEnemyToList(GameObject addedEnemy)
    {
        _enemyList.Add(addedEnemy);
    }
}
