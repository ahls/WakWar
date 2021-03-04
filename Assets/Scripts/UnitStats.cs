using UnityEngine;
using UnityEngine.UI;
public class UnitStats : MonoBehaviour
{
    #region 변수
    public int healthMax { get; set; }
    public int moveSpeed { get; set; }
    public int attackSpeed { get; set; }
    public bool playerOwned { get; set; }

    private int healthCurrent;

    [SerializeField]private Slider healthBar;
    [SerializeField] private GameObject selectionCircle;
    #endregion

    // Start is called before the first frame update
    void Start()
    {

        healthCurrent = healthMax;
        healthBar.maxValue = healthMax;
        healthBarUpdate();

        if (selectionCircle != null)//다른 유닛이랑 공용으로 쓰려면 이거 넣어야 할거같아요!
        {
            playerOwned = true;
            selectionCircle.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setSelectionCircleState(bool value)
    {
        if (!playerOwned)
        {
            return;
        }

        selectionCircle.SetActive(value);
    }
    public void takeDamage(int damageAmount)
    {
        healthCurrent -= damageAmount;
        healthBarUpdate();
    }
    private void healthBarUpdate()
    {
        healthBar.value = healthCurrent;
    }

}
