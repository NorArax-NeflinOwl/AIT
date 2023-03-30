using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int Damage = 1;
    [SerializeField] int Speed = 30;

    Rigidbody2D rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 1f);
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector2.right * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints healthPoints = collision.gameObject.GetComponent<HealthPoints>();
        if(null != healthPoints )
        {
            healthPoints.TakeDamege(Damage);
            Destroy(gameObject, 0.01f);
        }
    }
}
