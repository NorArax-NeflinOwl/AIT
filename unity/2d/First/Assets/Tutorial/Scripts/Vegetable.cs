using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    [SerializeField] Sprite[] spritesTab;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        GetRandomObject();
    }

    private void OnMouseDown()
    {
        GetRandomObject();
    }

    private void GetRandomObject()
    {
        sr.sprite = spritesTab[Random.Range(0, spritesTab.Length)];
        transform.position = new Vector3(Random.Range(-8, 8), Random.Range(-4, 4), 0);
    }
}
