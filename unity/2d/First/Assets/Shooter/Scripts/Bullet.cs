using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed = 40f;
    Vector3 dir;
    void Start()
    {
        dir = new Vector3(speed,0,0);
        Destroy(gameObject, 1f);
    }

    void Update()
    {
        transform.Translate(dir*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindAnyObjectByType<Shooter>().AddPointAndBullet();

        Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}
