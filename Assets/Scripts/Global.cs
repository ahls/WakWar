using UnityEngine;
using UnityEngine.SceneManagement;

public class Global : MonoBehaviour
{
    public static Global instance
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

    public static XMLLoader XMLLoader => _xmlLoader;
    public static ResourceManager ResourceManager => _resourceManager;
    public static ObjectPoolManager ObjectPoolManager => _objectPoolManager;
    public static UIManager UIManager => _uiManager;
    public static UIPopupManager UIPopupManager => _uiPopupManager;

    private static Global _instance;
    private static XMLLoader _xmlLoader;
    private static ResourceManager _resourceManager;
    private static ObjectPoolManager _objectPoolManager;
    private static UIManager _uiManager;
    private static UIPopupManager _uiPopupManager;

    [SerializeField] private GameObject _disableObject;
    [SerializeField] private GameObject _disableCanvas;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        _xmlLoader = new XMLLoader();
        _resourceManager = new ResourceManager();
        _objectPoolManager = new ObjectPoolManager(_disableObject, _disableCanvas);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void SetUIPopupManager(UIPopupManager uiPopupManager)
    {
        _uiPopupManager = uiPopupManager;
    }
}
