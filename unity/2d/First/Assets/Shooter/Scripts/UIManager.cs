using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Text BulletsInfo;
    [SerializeField] Text ScoreInfo;
    
    public void UpdateBulletsInfo(int bullets)
    {
        BulletsInfo.text = $"Bullets left: {bullets}";
    }

    public void UpdateScoreInfo(int score)
    {
        ScoreInfo.text = $"Score: {score}";
    }
}
