using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] float speed = 20f;

    void Start()
    {
        Debug.Log("Start...");
    }

    void Update()
    {
        float x_direction = Input.GetAxisRaw("Camera move Horizontal");
        float y_direction = Input.GetAxisRaw("Camera move Vertical");
        float z_direction = Input.GetAxisRaw("Mouse ScrollWheel");
        transform.Translate(new Vector3(x_direction, y_direction, z_direction) * speed * Time.deltaTime);
    }
}
