using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform Aim;
    [SerializeField] Transform AmmoHolder;
    [SerializeField] Sprite Sprite;

    public int MagazineCapasity;
    public int BulletSpeed;
    public int BulletDamage;

    private int BulletsLeftInMagazine;
    private Animator animator;
    private GunPickup gunPickup;
    private GunHolder gunHolder;

    private int poolQuantity;
    private List<Bullet> bullets;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.keepAnimatorStateOnDisable = true;

        gunHolder = GetComponentInParent<GunHolder>();

        BulletsLeftInMagazine = MagazineCapasity;

        gunHolder.InformAboutBulletsLeftInMagazineActiveGun(BulletsLeftInMagazine);

        poolQuantity = 15;
        bullets = new List<Bullet>();
        CreatePool();
    }

    private void Update()
    {
        if (IsGunAnimationInProccess())
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
                return;
            }
        }
    }

    private bool TryShoot()
    {
        if (BulletsLeftInMagazine == 0)
            return false;

        if (Input.GetMouseButtonDown(0))
        {
            TrySetAnimation("GunShoot");
            BulletsLeftInMagazine--;

            gunHolder.InformAboutBulletsLeftInMagazineActiveGun(BulletsLeftInMagazine);

            GetBullet();

            if (BulletsLeftInMagazine == 0)
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
            BulletsLeftInMagazine = MagazineCapasity;

            gunHolder.InformAboutBulletsLeftInMagazineActiveGun(BulletsLeftInMagazine);
            return true;
        }
        return false;
    }

    private bool TrySetAnimation(string animationName)
    {
        if(null != animator)
        {
            animator.SetTrigger(animationName);
            return true;
        }
        return false;
    }

    public bool IsGunAnimationInProccess()
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
        Destroy(gameObject, 0.1f);
    }

    public int GetBulletsLeftInMagazine()
    {
        return BulletsLeftInMagazine;
    }

    public Sprite GetGunSprite()
    {
        return Sprite;
    }
}
