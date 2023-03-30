using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float CameraSpeed = 20f;

    void Update()
    {
        float x_direction = Input.GetAxisRaw("Horizontal");
        float y_direction = Input.GetAxisRaw("Vertical");
        //float z_direction = Input.GetAxisRaw("Mouse ScrollWheel"); TODO
        transform.Translate(new Vector3(x_direction, y_direction, 0f) * CameraSpeed * Time.deltaTime);
    }
}
