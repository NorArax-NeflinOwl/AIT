using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float speed = 10f;
    float direction;
    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        transform.Translate(new Vector3(direction * speed, 0, 0) * Time.deltaTime);
    }
}
