using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heightTester : MonoBehaviour
{
    public float startingHeight;
    public float heightDelta;
    public float initDelta;
    public float deduction;
    public int lifeTime;
    public int Timer;
    public float Multiplier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            Timer = lifeTime;
            heightDelta = initDelta;
            transform.position = new Vector3(0, startingHeight, 0);
            deduction = initDelta * Multiplier / lifeTime;

        }
    }
    private void FixedUpdate()
    {
        if (Timer > 0)
        {
            Timer--;
            transform.position += new Vector3(0, heightDelta, 0);
            heightDelta += deduction;
        }
    }

}
