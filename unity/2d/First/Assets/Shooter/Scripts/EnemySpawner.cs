using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy[] enemies;
    void Start()
    {
        InvokeRepeating("Spawn", 1f, 1f);
    }


    void Spawn()
    {
        Vector3 spawnPos = transform.position + new Vector3(Random.Range(-3f,3f),0,0);
        Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPos, Quaternion.identity);
    }
}
