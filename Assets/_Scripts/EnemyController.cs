using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

    #region Setting variables

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

    [SerializeField]
    Animator anim;

    [SerializeField]
    GameObject shot;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    float timeBetweenAttacks = 2f;

    GameController gameController;

    bool grounded;
    bool collided = false;
    bool chasing = false;
    bool isFacingRight = true;
    float xSpeed = 0;
    bool onEdge = false;

    #endregion

    void Start()
    {
        target = GameObject.FindWithTag("PlayerObject").transform;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        xSpeed = speed;
    }

	void Update ()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance < maxDistanceToChase && !chasing)
        {
            if (target.position.x - transform.position.x > 0 && isFacingRight || target.position.x - transform.position.x < 0 && !isFacingRight || distance < maxDistanceToStop)
            {
                chasing = true;
                StartCoroutine(Shoot(target));
            }
        }
        else if (chasing && !onEdge)
        {
            if (distance > maxDistanceToChase)
            {
                chasing = false;
                rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                xSpeed = speed;
                if (!isFacingRight)
                    xSpeed = -xSpeed;
                StopAllCoroutines();
            }
            else
            {
                float difference = target.position.x - transform.position.x; // more than 0 - enemy must run right
                if (difference > 0 && !isFacingRight || difference < 0 && isFacingRight)
                    Flip();
                rigidbody2D.velocity = new Vector2(xSpeed, rigidbody2D.velocity.y);
                if (Mathf.Abs(difference) < maxDistanceToStop && grounded)              // stop if enemy is close to player
                {
                    rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
                }
            }
        }
        else //if patrolling
        {
            if (xSpeed > 0 && !isFacingRight)
                xSpeed = -xSpeed;
            rigidbody2D.velocity = new Vector2(xSpeed, rigidbody2D.velocity.y);
        }

        //Animation
        if (!grounded)
        {
            if (rigidbody2D.velocity.y > 0)
            {
                //play jump animation
            }
            else
            {
                //play fall animation
            }
        }
        else
        {
            bool walking = rigidbody2D.velocity.x != 0;
            anim.SetBool("IsWalking", walking);
        }
    }

    IEnumerator Shoot(Transform target)
    {
        while(true)
        {
            if(gameController.get_gameOver() || !enabled)
            {
                break;
            }

            Vector3 difference = target.position - firePoint.position;
            difference.Normalize();

            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Instantiate(shot, firePoint.position, Quaternion.Euler(0f, 0f, rotZ));

            anim.SetTrigger("Attack-1");

            yield return new WaitForSeconds(timeBetweenAttacks);
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
            if (grounded)
            {
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
            }
        }
        else if (other.tag == "PatrolTrigger" && !chasing)
        {
            Flip();
        }
        else if (other.tag == "StopTrigger" && chasing)
        {
            StartCoroutine(GiveUpChasing());
            onEdge = true;
            chasing = false;
            rigidbody2D.velocity = new Vector2(0, 0);
        }
    }

    IEnumerator GiveUpChasing()
    {
        while (true)
        {
            Debug.Log("lol");
            yield return new WaitForSeconds(1);
            if (Vector3.Distance(target.position, transform.position) > maxDistanceToChase)
            {
                break;
            }
        }
        onEdge = false;
    }
}
