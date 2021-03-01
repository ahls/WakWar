using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public static IngameManager instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }

            return _instance;
        }
    }
    public static UnitSelector UnitSelector => _unitSelector;

    private static IngameManager _instance;
    private static UnitSelector _unitSelector;

    private void Awake()
    {
        _instance = this;
    }

    public void SetUnitSelector(UnitSelector unitSelector)
    {
        _unitSelector = unitSelector;
    }
}
