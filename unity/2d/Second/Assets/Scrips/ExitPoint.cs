using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPoint : MonoBehaviour
{
    private int activeScene;
    private int allScenesCount;

    private void Start()
    {
        // Pobranie numeru aktualnej sceny
        activeScene = SceneManager.GetActiveScene().buildIndex;
        allScenesCount = SceneManager.sceneCountInBuildSettings;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(activeScene <= allScenesCount)
        {
            SceneManager.LoadScene(++activeScene, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
