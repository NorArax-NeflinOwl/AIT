using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlayer : MonoBehaviour
{
    public float ResetDelay;

    private bool isGameOver = false;

    private void Start()
    {
        // Blokada rotacji postacji
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Metoda wywoływana, gdy gracz umiera
    public void GameOver()
    {
        if (!isGameOver)
        {
            // Ustawienie flagi "isGameOver" na true, aby zapobiec ponownemu wywołaniu tej metody
            isGameOver = true;

            // Wywołanie metody "ResetGame" z opóźnieniem
            Invoke("ResetGame", ResetDelay);
        }
    }

    // Metoda resetująca grę
    void ResetGame()
    {
        // Pobranie numeru aktualnej sceny
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Ładowanie sceny o danym numerze po opóźnieniu
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
    }
}
