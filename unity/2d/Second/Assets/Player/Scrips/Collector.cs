using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] Transform GunHolder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GunPickup gunPickup = collision.GetComponent<GunPickup>();
        if(null != gunPickup)
        {
            Gun gun = gunPickup.PickUp();
            GunHolder holder = GunHolder.GetComponent<GunHolder>();
            if(!holder.HasGunAlready(gun.name))
            {
                Gun newGun = Instantiate(gun, GunHolder);
                newGun.SetPickUp(gunPickup);
                holder.AddGun(newGun);
            }
        }
    }
}
