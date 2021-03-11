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

    public static ResourceManager ResourceManager => _resourceManager;
    public static UIManager UIManager => _uiManager;

    private static Global _instance;
    private static ResourceManager _resourceManager;
    private static UIManager _uiManager;

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

        _resourceManager = new ResourceManager();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void SetUIManager(UIManager uiManager)
    {
        _uiManager = uiManager;
    }
}
