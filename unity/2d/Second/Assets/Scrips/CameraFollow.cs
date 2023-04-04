using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;

    public float zoomSpeed = 1f;
    public float minZoom = 10f;      // Minimalna odległość kamery
    public float maxZoom = 50f;     // Maksymalna odległość kamery

    private Vector3 offset;
    private Camera m_Camera;

    void Start()
    {
        offset = new Vector3(3,2,-10f);
        m_Camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (playerMovement && null != offset)
        {
            transform.position = playerMovement.transform.position + offset;
        }


        // Pobranie wartości pokrętła myszy
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0f)
        {
            if (scroll > 0 && m_Camera.orthographicSize <= maxZoom)
            {
                m_Camera.orthographicSize += zoomSpeed;
            }
            else if (scroll < 0 && m_Camera.orthographicSize > minZoom)
            {
                m_Camera.orthographicSize -= zoomSpeed;
            }
        }
    }
}
