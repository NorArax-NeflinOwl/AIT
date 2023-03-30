using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    [SerializeField] float Speed = 5f;
    Vector3 direction;
    SpriteRenderer sr;
    void Start()
    {
        direction = new Vector3(Speed, 0, 0);
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.x >= 10)
        {
            sr.flipX = false;
            direction = new Vector3(-Speed, 0, 0);
        }
        if(transform.position.x <= -10)
        {
            sr.flipX = true;
            direction = new Vector3(Speed, 0, 0);
        }

        transform.Translate(direction * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        Destroy(gameObject); // ToDo do not destroying, better hidding it.
    }
}
