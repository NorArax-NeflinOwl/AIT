using UnityEngine;

public class HealthPoints : MonoBehaviour
{
    [SerializeField] int MaxHealthPoints = 10;

    int healthPoints;
    void Start()
    {
        healthPoints = MaxHealthPoints;
    }

    public void TakeDamege(int damage)
    {
        healthPoints -= damage;
        if( healthPoints <= 0 )
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
