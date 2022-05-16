using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skillTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            List<GameObject> skillUnitList = IngameManager.UnitManager.GetSelectedUnits();
            foreach (var unit in skillUnitList)
            {
                PanzeeBehaviour unitCombat = unit.GetComponent<PanzeeBehaviour>();
                unitCombat.Skill.UseSkill(unitCombat.unitController);
            }
        }
    }
}
