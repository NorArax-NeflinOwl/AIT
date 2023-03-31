using UnityEngine;
using UnityEngine.UI;

public class UIGunInfo : MonoBehaviour
{
    public Image ImagageMagazineCapacity;
    public Image ImagageBulletsInMagazine;
    public Image ImageGun;

    private void Start()
    {
        GunHolder.InformAboutActiveGunAction += UpdateUIMagazineCapacity;
        GunHolder.InformAboutBulletsLeftInMagazineActiveGunAction += UpdateUIBulletsLeftInMagazine;

        UpdateUIBulletsLeftInMagazine(0);
        UpdateUIMagazineCapacity(0, null);
    }

    private void UpdateUIBulletsLeftInMagazine(int bulletInMagazine)
    {
        if (null != ImagageBulletsInMagazine)
        {
            ImagageBulletsInMagazine.fillAmount = bulletInMagazine / 50f;
        }
    }

    private void UpdateUIMagazineCapacity(int magazineCapasity, Sprite gunSprite)
    {
        if(null != ImagageMagazineCapacity)
        {
            ImagageMagazineCapacity.fillAmount = magazineCapasity / 50f;
        }

        if(null != gunSprite)
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
