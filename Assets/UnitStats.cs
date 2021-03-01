using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour
{
    #region 변수
    public int healthMax { get; set; }
    private int healthCurrent;
    public int moveSpeed { get; set; }
    public int attackSpeed { get; set; }
    public bool playerOwned { get; set; }
    private GameObject selectionCircle;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        healthCurrent = healthMax;
        Transform tempCircle = transform.Find("selection circle");
        if(tempCircle != null)
        {
            playerOwned = true;
            selectionCircle = tempCircle.gameObject;
            selectionCircle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSelectionCircleState(bool value)
    {
        if (!playerOwned) return;    
        selectionCircle.SetActive(value);
    }
}
