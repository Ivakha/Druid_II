using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float jumpForce;
    float speed;

    Animator anim;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    bool grounded = false;
    bool facingRight = true;
    bool jump = false;

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
    }


    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");

        rigidbody2D.velocity = new Vector2(h * speed, rigidbody2D.velocity.y);

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();

        if (jump)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        if (grounded)
        {
            bool walking = h != 0f;
            anim.SetBool("IsWalking", walking);

            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
        }
        else
        {
            if (rigidbody2D.velocity.y < 0)
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFalling", true);
            }
            else
            {
                anim.SetBool("IsJumping", true);
                anim.SetBool("IsFalling", false);
            }
        }
    }


    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void UpdateForm(Animator anim, float speed, float jumpForce)
    {
        this.anim = anim;
        this.speed = speed;
        this.jumpForce = jumpForce;
    }
}
