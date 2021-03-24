using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testspin : MonoBehaviour
{
    public float torque;
    void Start()
    {
        Rigidbody2D RB = GetComponent<Rigidbody2D>();
        RB.AddTorque(torque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
