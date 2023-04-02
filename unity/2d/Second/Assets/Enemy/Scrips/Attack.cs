using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int AttackDamage = 1;

    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthPoints hp = collision.gameObject.GetComponent<HealthPoints>();
        if(hp)
        {
            hp.TakeDamege(AttackDamage, transform.position);
        }
    }
}
