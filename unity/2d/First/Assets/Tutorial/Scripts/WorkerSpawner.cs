using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] GameObject workerPrefab;
    void Start()
    {
        InvokeRepeating("AddWorker", 1f, 1f);
    }

    void AddWorker()
    {
        Vector3 spawningPosition = new Vector3(-8f, Random.Range(-4f, 4f), 0);
        Instantiate(workerPrefab, spawningPosition, Quaternion.identity);
    }
}
