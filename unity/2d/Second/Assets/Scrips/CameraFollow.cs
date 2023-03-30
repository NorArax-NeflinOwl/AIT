using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private Vector3 offset;

    void Start()
    {
        offset = gameObject.transform.position - playerMovement.transform.position;
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(playerMovement.transform.position.x + offset.x, 
                                                    gameObject.transform.position.y, 
                                                    gameObject.transform.position.z);
    }
}
