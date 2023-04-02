using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int Damage = 1;
    [SerializeField] int Speed = 30;

    private new Rigidbody2D rigidbody;
    private Transform bulletParent;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        StartCoroutine(Return2Pool(1f));
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = Vector2.right * Speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints healthPoints = collision.gameObject.GetComponent<HealthPoints>();
        if(healthPoints )
        {
            healthPoints.TakeDamege(Damage, transform.position);
            StartCoroutine(Return2Pool(0.01f));
        }
    }

    IEnumerator Return2Pool(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.transform.SetParent(bulletParent);
        gameObject.SetActive(false);
    }

    public void Initialize(int damage, int speed, Transform bulletParent)
    {
        Damage = damage;
        Speed = speed;
        this.bulletParent = bulletParent;
    }
}
