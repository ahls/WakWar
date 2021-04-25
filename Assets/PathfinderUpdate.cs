using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class PathfinderUpdate : MonoBehaviour
{
    public float UpdateRate;
    private int _counterMax;
    private int _counter;
    // Start is called before the first frame update
    void Start()
    {
        _counterMax = (int)(50 * UpdateRate);
    }

    private void FixedUpdate()
    {
        if(_counter <= 0)
        {
            _counter = _counterMax;

            var graphToScan = AstarPath.active.data.gridGraph;
            AstarPath.active.Scan(graphToScan);
            Debug.Log("graph updated");
        }
        _counter--;
    }
}
