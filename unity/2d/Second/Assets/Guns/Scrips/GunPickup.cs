using UnityEngine;

public class GunPickup : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Distance;
    [SerializeField] float RotationSpeed;
    [SerializeField] Gun GunPrefab;

    public Gun PickUp()
    {
        Destroy(gameObject, 0.1f);
        return GunPrefab;
    }

    void Start()
    {
        InvokeRepeating("ChangeSpeed", 0f, Distance);
    }

    void Update()
    {
        transform.Translate(new Vector3(0f, Speed, 0f) * Time.deltaTime);
        transform.Rotate(new Vector3(0f, RotationSpeed, 0f) * Time.deltaTime);
    }
    void ChangeSpeed()
    {
        Speed *= -1f;
    }
}
