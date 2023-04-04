using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunHolder : MonoBehaviour
{
    public static System.Action<int, Sprite> InformAboutActiveGunAction;
    public static System.Action<int> InformAboutBulletsLeftInMagazineActiveGunAction;

    private AudioSource audioSource;
    private List<Gun> gunList;
    private int activeGun = 0;

    private void Start()
    {
        gunList = new List<Gun>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchGun();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            DropGun();
        }
    }

    private void SwitchGun()
    {
        if (gunList.Any() && !IsBusy())
        {
            audioSource.Play();

            gunList[activeGun].gameObject.SetActive(false);
            if (activeGun < gunList.Count - 1)
            {
                activeGun++;
            }
            else
            {
                activeGun = 0;
            }
            gunList[activeGun].gameObject.SetActive(true);

            InformAboutActiveGun(gunList[activeGun].MagazineCapasity, gunList[activeGun].GetGunSprite());
            InformAboutBulletsLeftInMagazineActiveGun(gunList[activeGun].GetBulletsLeftInMagazine());
        }
    }

    private bool IsBusy()
    {
        if (!gunList.Any())
            return true;

        return gunList[activeGun].IsBusy();
    }

    private void InformAboutActiveGun(int magazineCapacity, Sprite gunSprite)
    {
        if (null != InformAboutActiveGunAction)
        {
            InformAboutActiveGunAction.Invoke(magazineCapacity, gunSprite);
        }
    }

    private bool HasGunAlready(string gunName)
    {
        for (int i = 0; i < gunList.Count; i++)
        {
            if (gunList[i].name == gunName)
            {
                return true;
            }
        }
        return false;
    }

    public void InformAboutBulletsLeftInMagazineActiveGun(int bulletsLeftInMagazine)
    {
        if (null != InformAboutBulletsLeftInMagazineActiveGunAction)
        {
            InformAboutBulletsLeftInMagazineActiveGunAction.Invoke(bulletsLeftInMagazine);
        }
    }

    public void AddGun(Gun gun)
    {
        gunList.Add(gun);
        SwitchGun();
    }

    public void DropGun()
    {
        if (gunList.Any())
        {
            gunList[activeGun].Drop();
            gunList.RemoveAt(activeGun);
            activeGun = 0;
            SwitchGun();
        }

        if (gunList.Count == 0)
        {
            InformAboutActiveGun(0, null);
            InformAboutBulletsLeftInMagazineActiveGun(0);
        }
    }

    public bool CanAddGun(string gunName)
    {
        return !IsBusy() && HasGunAlready(gunName);
    }
}
