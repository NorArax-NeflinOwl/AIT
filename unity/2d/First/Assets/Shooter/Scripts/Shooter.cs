using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooter : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform aim;
    [SerializeField] int BulletsAmount;

    public int Score;

    UIManager manager;

    private void Start()
    {
        Score = 0;

        manager = FindAnyObjectByType<UIManager>();
        manager.UpdateBulletsInfo(BulletsAmount);
        manager.UpdateScoreInfo(Score);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
            Thread.Sleep(50);
        }
    }

    void Shoot()
    {
        if(0 < BulletsAmount)
        {
            Instantiate(bulletPrefab, aim.transform.position, Quaternion.identity);
            manager.UpdateBulletsInfo(--BulletsAmount);
        }
        else
        {
            //End Game
            SceneManager.LoadScene(1);
        }
    }

    public void AddPointAndBullet()
    {
        manager.UpdateBulletsInfo(++BulletsAmount);
        manager.UpdateScoreInfo(++Score);
    }

    public void DecreaseScore()
    {
        manager.UpdateScoreInfo(--Score);
    }
}
