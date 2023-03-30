using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform Aim;
    public int MagazineCapasity = 15;

    private int BulletsLeftInMagazine;
    private Animator animator;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        BulletsLeftInMagazine = MagazineCapasity;
    }

    private void Update()
    {
        TryShoot();
        TryReload();
    }

    private bool TryShoot()
    {
        if (BulletsLeftInMagazine == 0)
            return false;

        if (Input.GetMouseButtonDown(0))
        {
            TrySetAnimation("GunShoot");
            BulletsLeftInMagazine--;
            Instantiate(bulletPrefab, Aim.position, Quaternion.identity);

            if(BulletsLeftInMagazine == 0)
            {
                TrySetAnimation("GunEmpty");
            }
            return true;
        }
        return false;
    }

    private bool TryReload()
    {
        if (Input.GetMouseButtonDown(1))
        {
            TrySetAnimation("GunReload");
            BulletsLeftInMagazine = MagazineCapasity;
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
}
