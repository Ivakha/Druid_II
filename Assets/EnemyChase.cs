using UnityEngine;
using System.Collections;

public class EnemyChase : MonoBehaviour {

    [SerializeField]
    Transform target;

    [SerializeField]
    float speed;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float maxDistanceToChase;

    [SerializeField]
    float maxDistanceToStop;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    [SerializeField]
    Transform sightStart;

    [SerializeField]
    Transform sigthEnd;

    [SerializeField]
    Transform groundCheck;

    bool grounded;
    bool collided = false;
    bool chasing = false;
    bool isFacingRight = true;
    float xSpeed = 0;

    void Start()
    {
        xSpeed = speed;
    }

	void Update ()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < maxDistanceToChase && !chasing)
        {
            chasing = true;
            xSpeed = speed;
            float difference = target.position.x - transform.position.x; // more than 0 - enemy must run right
            /*if (difference > 0)
            {
                xSpeed = -speed;
                Debug.Log(xSpeed);

            }*/
            rigidbody2D.velocity = new Vector2(xSpeed, rigidbody2D.velocity.y);
        }
        else if (chasing)
        {
            Debug.Log("Chasing...");
            if (distance > maxDistanceToChase)
            {
                chasing = false;
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                xSpeed = speed;
            }
            else
            {
                float difference = target.position.x - transform.position.x; // more than 0 - enemy must run right
                if (difference > 0 && !isFacingRight || difference < 0 && isFacingRight)
                    Flip();
                rigidbody2D.velocity = new Vector2(xSpeed, rigidbody2D.velocity.y);
                if (distance < maxDistanceToStop)
                    rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            }
        }
        else //if patrolling
        {
            Debug.Log("Investigating...");
            if (xSpeed > 0 && !isFacingRight)
                xSpeed = -xSpeed;
            rigidbody2D.velocity = new Vector2(xSpeed, rigidbody2D.velocity.y);
        }
    }

    void Flip()
    {
        xSpeed = -xSpeed;
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "JumpTrigger" && chasing)
        {
            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (grounded)
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
        }
        else if (other.tag == "PatrolTrigger" && !chasing)
        {
            Flip();
        }
    }
}
