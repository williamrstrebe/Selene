using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{

    private Rigidbody2D rb;
    [Header("Jump Options")]
    public int jumpPower;
    public float fallMultiplier;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Vector2 vecGravity;
    public int doubleJump = 2;


    // Start is called before the first frame update
    void Start()
    {
        vecGravity = new Vector2(0, Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded())
            {
                doubleJump = 2;
            }
            {
                if (doubleJump > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                    doubleJump -= 1;
                }
            }
        }

        // aumentar velocidade da queda
        if (rb.velocity.y < 0)
        {
            rb.velocity += vecGravity * fallMultiplier * Time.deltaTime;
        }
    }

    private bool isGrounded()
    {

        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(1.0f, 0.3f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

}
