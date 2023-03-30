using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScene : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    [SerializeField] Text Score;

    private void Start()
    {
        //Score.text = $"Your Score: {shooter.Score}";
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(0); //index of scene from File/Build Settings
    }
}
