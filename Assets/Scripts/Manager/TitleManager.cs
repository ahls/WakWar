using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButtonHit()
    {
        Global.UIManager.SceneTransition.PlayAnimation("close");
        StartCoroutine(CallScene());
        
    }
    private IEnumerator CallScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GY_CS00");

    }
}
