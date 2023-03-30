using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;

    void Update()
    {
        transform.Rotate(new Vector3(0f, rotationSpeed, 0f) * Time.deltaTime);
    }
}
