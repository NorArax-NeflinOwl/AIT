using UnityEngine;

public class Collector : MonoBehaviour
{
    [SerializeField] Transform GunHolder;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GunPickup gunPickup = collision.GetComponent<GunPickup>();
        if(null != gunPickup)
        {
            Instantiate(gunPickup.PickUp(), GunHolder);
        }
    }
}
