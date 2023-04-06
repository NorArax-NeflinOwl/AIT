using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
