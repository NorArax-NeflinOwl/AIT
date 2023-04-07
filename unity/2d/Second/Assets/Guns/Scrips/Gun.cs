using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform Aim;
    [SerializeField] Transform AmmoHolder;
    [SerializeField] Sprite Sprite;
    [SerializeField] AudioClip ShootAudioClip;
    [SerializeField] AudioClip EmptyAudioClip;
    [SerializeField] AudioClip ReloadClip;
    [SerializeField] ParticleSystem Muzzle;
    [SerializeField] int GunMass = 10;

    public int MagazineCapasity;
    public int BulletSpeed;
    public int BulletDamage;
    public bool IsMachineGun;

    private int bulletsLeftInMagazine;
    private Animator animator;
    private new AudioSource audio;
    private GunPickup gunPickup;
    private GunHolder gunHolder;

    private int poolQuantity;
    private List<Bullet> bullets;

    private void Start()
    {
        bulletsLeftInMagazine = MagazineCapasity;

        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = true;

        gunHolder = GetComponentInParent<GunHolder>();
        gunHolder.InformAboutBulletsLeftInMagazineActiveGun(bulletsLeftInMagazine);

        poolQuantity = 15;
        bullets = new List<Bullet>();
        CreatePool();
    }

    private void Update()
    {
        if (IsBusy())
            return;

        TryShoot();
        TryReload();
    }

    private void CreatePool()
    {
        for(int i = 0; i < poolQuantity; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab, new Vector3(), Quaternion.identity, AmmoHolder);
            bullet.GetComponent<Bullet>().Initialize(BulletDamage, BulletSpeed, AmmoHolder);
            bullets.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
    }

    private void GetBullet()
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeInHierarchy)
            {
                bullets[i].gameObject.SetActive(true);
                bullets[i].transform.parent = null;
                bullets[i].transform.position = Aim.position;
                bullets[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
                return;
            }
        }
    }

    private void Shoot()
    {
        TrySetAnimation("GunShoot");
        gunHolder.InformAboutBulletsLeftInMagazineActiveGun(--bulletsLeftInMagazine);
        audio.Stop();
        audio.PlayOneShot(ShootAudioClip);
        Muzzle.Stop();
        Muzzle.Play();
        GetBullet();
    }

    private bool TryShoot()
    {
        if (Input.GetMouseButtonDown(0) || (Input.GetMouseButton(0) && IsMachineGun))
        {
            if(bulletsLeftInMagazine == 0)
            {
                audio.Stop();
                audio.PlayOneShot(EmptyAudioClip);
            }
            else
            {
                Shoot();
            }

            if (bulletsLeftInMagazine == 0)
            {
                TrySetAnimation("GunEmpty");
            }
            return true;
        }
        return false;
    }

    private bool TryReload()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.R))
        {
            TrySetAnimation("GunReload");
            bulletsLeftInMagazine = MagazineCapasity;

            gunHolder.InformAboutBulletsLeftInMagazineActiveGun(bulletsLeftInMagazine);
            audio.Stop();
            audio.PlayOneShot(ReloadClip);
            return true;
        }
        return false;
    }

    private bool TrySetAnimation(string animationName)
    {
        if(animator)
        {
            animator.SetTrigger(animationName);
            return true;
        }
        return false;
    }

    public bool IsBusy()
    {
        return animator != null && animator.GetCurrentAnimatorStateInfo(0).IsTag("GunAnimation");
    }

    public void SetPickUp(GunPickup gunPickup)
    {
        this.gunPickup = gunPickup;
    }

    public void Drop()
    {
        float pickupSize = gunPickup.GetComponent<SpriteRenderer>().size.x;
        Vector3 position = new Vector3(transform.position.x - (pickupSize * 2), transform.position.y, 0f);
        gunPickup.Drop(position);
        gameObject.SetActive(false);
    }

    public int GetBulletsLeftInMagazine()
    {
        return bulletsLeftInMagazine;
    }

    public Sprite GetGunSprite()
    {
        return Sprite;
    }

    public int GetGunMass()
    {
        return GunMass;
    }
}
