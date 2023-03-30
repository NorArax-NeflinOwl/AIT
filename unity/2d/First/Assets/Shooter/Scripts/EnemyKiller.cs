using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKiller : MonoBehaviour
{
    [SerializeField] Shooter shooter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        shooter.DecreaseScore();
        Destroy(collision.gameObject);
    }
}
