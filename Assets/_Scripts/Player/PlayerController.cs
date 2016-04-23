using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    float jumpForce;
    float frontSpeed;
    float backwardSpeed;
    float timeBetweenAttacks;
    float formsTimesWhileAttack;
    Animator anim;

    [SerializeField, Tooltip("Place it under player")]
    Transform groundCheck;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    [SerializeField, Tooltip("Place Camera Object that will be following the player")]
    new Camera camera;

    [SerializeField]
    GameObject druidShot;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    GameObject claws;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    PlayerSwapForms playerSwapForms; //contains parameters of current form (speed, jumpForce etc.)

    float time = 10f;
    float attackTime;

    bool grounded = false;
    bool facingRight = true;
    bool jump = false;
    bool isDruidForm = false;
    bool dontMove = false;

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //draws a line between player and groundCheck. If it hits something - player is on the ground

        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;

        #region Attack

        if (dontMove)
            if (time > attackTime)
                dontMove = false;

        if (grounded && Input.GetButtonDown("Fire") && time > timeBetweenAttacks)
            Attack();

        time += Time.deltaTime;

        #endregion
    }

    void FixedUpdate()
    {
        if (dontMove)
            return;

        float h = Input.GetAxis("Horizontal");

        #region Facing

        if (!isDruidForm) // Non-Druid player form faces the direction it moves in
        {
            if (h > 0 && !facingRight || h < 0 && facingRight)
                Flip();
        }
        else // Player in druid form can move backwards (depends on cursor position)
        {
            if (camera.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x && facingRight)
                Flip();
            else if (camera.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x && !facingRight)
                Flip();
        }

        #endregion

        #region Speed

        float currentSpeed = frontSpeed;
        if (h > 0 && !facingRight || h < 0 && facingRight) // if player moves backwards his speed is slower
            currentSpeed = backwardSpeed;
        rigidbody2D.velocity = new Vector2(h * currentSpeed, rigidbody2D.velocity.y);

        #endregion

        #region Jump

        if (jump)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        #endregion

        #region Animations

        if (grounded)
        {
            bool walking = h != 0f;
            anim.SetBool("IsWalking", walking);

            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
        }
        else
        {
            if (rigidbody2D.velocity.y < 0) // velocity.y < 0 means we are moving down - falling
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFalling", true);
            }
            else // if velocity.y > 0
            {
                anim.SetBool("IsJumping", true);
                anim.SetBool("IsFalling", false);
            }
        }

        #endregion
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Attack()
    {
        time = 0f;
        anim.SetTrigger("Attack-1");

        rigidbody2D.velocity = new Vector2(0, 0);
        dontMove = true;                                                            //player can't move while attack
        //AnimatorClipInfo animatorClipInfo = anim.GetCurrentAnimatorClipInfo(0)[0];  //get current animation clip ...
        //attackTime = animatorClipInfo.clip.length;                                  //to get its lenght
        attackTime = formsTimesWhileAttack;
        if (isDruidForm)
            DruidAttack();
        else AnimalAttack(); 
    }

    void AnimalAttack()
    {
        Vector3 rotation = Vector3.zero;
        if (!facingRight)
            rotation += new Vector3(0, 0, 180);
        Instantiate(claws, attackPoint.position, Quaternion.Euler(rotation));
    }

    void DruidAttack()
    {
        Vector3 difference = camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Instantiate(druidShot, firePoint.position, Quaternion.Euler(0f, 0f, rotZ));
    }

    public void UpdateForm(Animator anim, float speed, float backwardSpeed, float jumpForce, float timeBetweenAttacks, float formsTimesWhileAttack)
        // Get parameters of current form (for example panther moves faster than druid)
    {
        this.anim = anim;
        this.frontSpeed = speed;
        this.backwardSpeed = backwardSpeed;
        this.jumpForce = jumpForce;
        this.timeBetweenAttacks = timeBetweenAttacks;
        this.formsTimesWhileAttack = formsTimesWhileAttack;
        if (playerSwapForms.currentForm == 0)
            isDruidForm = true;
        else
            isDruidForm = false;
    }
}
