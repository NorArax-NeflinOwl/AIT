using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed = 10f;
    public Vector2 JumpForce;
    public int JumpMaxNumber = 2;

    private new Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private float horizontalMovement;
    private bool canJump;
    private int jumpCounter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        canJump = false;
        jumpCounter = 0;
    }

    private void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            canJump = true;
            jumpCounter++;
        }
    }

    private void FixedUpdate() // FixedUpdate for physics when you are using Rigidbody2D - 50 calls per second
    {
        if (horizontalMovement != 0)
        {
            Flip();
            rigidbody.velocity = new Vector2(horizontalMovement * Speed, rigidbody.velocity.y);
        }

        if(canJump && jumpCounter <= JumpMaxNumber)
        {
            rigidbody.AddForce(JumpForce, ForceMode2D.Impulse);
            canJump = false;
        }

        if(Mathf.Abs(rigidbody.velocity.y) < 0.01f)
        {
            jumpCounter = 0;
            canJump = false;
        }
    }

    private void Flip()
    {
        bool flipX = horizontalMovement < 0;
        if (spriteRenderer.flipX != flipX)
        {
            spriteRenderer.flipX = flipX;
        }
    }
}
