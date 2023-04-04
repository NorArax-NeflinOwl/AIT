using UnityEngine;
using UnityEngine.UI;

public class UIGunInfo : MonoBehaviour
{
    public Image ImageHeartsFull;
    public Image ImageHearts;
    public Image ImageMagazineCapacity;
    public Image ImageBulletsInMagazine;
    public Image ImageGun;

    private void Start()
    {
        GunHolder.InformAboutActiveGunAction += UpdateUIMagazineCapacity;
        GunHolder.InformAboutBulletsLeftInMagazineActiveGunAction += UpdateUIBulletsLeftInMagazine;
        HealthPoints.InformAboutHeartsFullAction += UpdateUIHeartsFull;
        HealthPoints.InformAboutHeartsLeftAction += UpdateUIHearts;

        UpdateUIBulletsLeftInMagazine(0);
        UpdateUIMagazineCapacity(0, null);
    }

    private void UpdateUIHeartsFull(int hearts)
    {
        if (ImageHeartsFull)
        {
            ImageHeartsFull.fillAmount = hearts / 10f;
        }
    }

    private void UpdateUIHearts(int hearts)
    {
        if (ImageHearts)
        {
            ImageHearts.fillAmount = hearts / 10f;
        }
    }

    private void UpdateUIBulletsLeftInMagazine(int bulletInMagazine)
    {
        if (ImageBulletsInMagazine)
        {
            ImageBulletsInMagazine.fillAmount = bulletInMagazine / 50f;
        }
    }

    private void UpdateUIMagazineCapacity(int magazineCapasity, Sprite gunSprite)
    {
        if(ImageMagazineCapacity)
        {
            ImageMagazineCapacity.fillAmount = magazineCapasity / 50f;
        }

        if(ImageGun)
        {
            if (gunSprite)
            {
                ImageGun.gameObject.SetActive(true);
                ImageGun.sprite = gunSprite;
                ImageGun.rectTransform.sizeDelta = gunSprite.bounds.size * 50;
            }
            else
            {
                ImageGun.gameObject.SetActive(false);
            }
        }
    }
}
