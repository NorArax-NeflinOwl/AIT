using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] float Speed;
    [SerializeField] float Distance;
    void Start()
    {
        InvokeRepeating("ChangeSpeed", 0f, Distance);
    }

    void Update()
    {
        transform.Translate(new Vector3(0f, Speed * Time.deltaTime, 0f));
    }

    void ChangeSpeed()
    {
        Speed *= -1f;
    }
}
