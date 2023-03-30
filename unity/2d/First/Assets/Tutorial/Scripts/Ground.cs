using UnityEngine;

public class Ground : MonoBehaviour
{
    void Update()
    {
        transform.position = new Vector3(0, -4f, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
