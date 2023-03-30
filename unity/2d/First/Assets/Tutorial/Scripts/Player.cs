using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] Sprite sprite;

    private SpriteRenderer sr;

    void Start()
    {
        sr =  GetComponent<SpriteRenderer>();
        if (sprite)
        {
            sr.sprite = sprite;
        }
    }

    void Update()
    {
        float x_direction = Input.GetAxis("Horizontal");
        float y_direction = Input.GetAxis("Vertical");

        transform.Rotate(new Vector3(0f, 0f, y_direction) * rotationSpeed * Time.deltaTime);
        transform.Translate(new Vector3(x_direction, 0f, 0f) * speed * Time.deltaTime);
        sr.color = new Color(Math.Abs(x_direction), Math.Abs(x_direction), Math.Abs(x_direction), 1);
    }
}
