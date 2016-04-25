using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Setting variables

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
    int mana = 5;

    [SerializeField]
    int manaPerShot = 1;

    int currentMana;

    [SerializeField]
    GameObject druidShot;

    [SerializeField]
    Transform firePoint;

    [SerializeField]
    GameObject claws;

    [SerializeField]
    GameObject bearClaws;

    [SerializeField]
    Transform attackPoint;

    [SerializeField]
    PlayerSwapForms playerSwapForms; //contains parameters of current form (speed, jumpForce etc.)

    [SerializeField]
    Slider manaBar;

    [SerializeField]
    AudioSource piyClip;

    [SerializeField]
    AudioSource clawsClip;

    [SerializeField]
    AudioSource fallClip;

    [SerializeField]
    GameController gameController;

    float time = 10f;
    float attackTime;

    bool grounded = false;
    bool facingRight = true;
    bool jump = false;
    bool isDruidForm = false;
    bool dontMove = false;
    int playerLayer;
    int groundLayer;
    bool isIgnoringGround = false;
    float fallTime = 0;

    #endregion

    void Start()
    {
        currentMana = mana;
        manaBar.value = 1f;

        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");

        fallClip.enabled = false;
    }

    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        //draws a line between player and groundCheck. If it hits something - player is on the ground

        if (Input.GetButtonDown("Jump") && grounded)
        {
            if (Input.GetKey(KeyCode.S)/* && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)*/)
            {
                StartCoroutine(IgnoreLayerForTime());
            }
            else
            {
                jump = true;
            }
        }

        #region Attack

        if (dontMove)
            if (time > attackTime)
                dontMove = false;

        if (/*grounded && */Input.GetButtonDown("Fire") && time > timeBetweenAttacks)
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
        if (h > 0 && !facingRight || h < 0 && facingRight && grounded) // if player moves backwards his speed is slower
            currentSpeed = backwardSpeed;
        rigidbody2D.velocity = new Vector2(h * currentSpeed, rigidbody2D.velocity.y);
        //if (!grounded && !isDruidForm)
        //    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x / 2, rigidbody2D.velocity.y);

        #endregion

        #region Jump

        if (jump)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }

        #endregion

        #region Animations

        if (grounded && !anim.GetBool("IsJumping"))
        {
            //Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
            bool walking = h != 0f;
            anim.SetBool("IsWalking", walking);
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
        }
        else
        {

            if (rigidbody2D.velocity.y <= 0) // velocity.y < 0 means we are moving down - falling
            {
                if (isIgnoringGround)
                {
                    isIgnoringGround = false;
                    anim.SetBool("IsJumping", false);
                    anim.SetBool("IsFalling", true);
                    Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
                    fallTime = 0;
                }
                else
                {
                    fallTime += Time.fixedDeltaTime;
                    
                    anim.SetBool("IsFalling", true);
                }
                if(fallTime > 2.5f)
                {
                    clawsClip.enabled = false;
                    piyClip.enabled = false;
                    playerSwapForms.TurnOffSounds();
                    fallClip.enabled = true;
                    if (fallTime > 3.5f)
                    {
                        gameController.GameOver();
                    }
                }
            }
            else // if velocity.y > 0
            {
                fallTime = 0;
                if (!isIgnoringGround)
                {
                    isIgnoringGround = true;
                    anim.SetBool("IsJumping", true);
                    anim.SetBool("IsFalling", false);
                    Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
                }
            }
        }

        #endregion
    }

    IEnumerator IgnoreLayerForTime()
    {
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
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
        if (grounded)
        {
            if (Random.Range(0,2) == 0)
                anim.SetTrigger("Attack-1");
            else
                anim.SetTrigger("Attack-2");
            
            rigidbody2D.velocity = new Vector2(0, 0);
            
        }
        dontMove = true; //player can't move while attack                                           
        //AnimatorClipInfo animatorClipInfo = anim.GetCurrentAnimatorClipInfo(0)[0];  //get current animation clip ...
        //attackTime = animatorClipInfo.clip.length;                                  //to get its lenght
        attackTime = formsTimesWhileAttack;
        if (isDruidForm)
        {
            if (currentMana >= manaPerShot)
            {
                currentMana -= manaPerShot;
                UpdateManaBar();
                DruidAttack();
            }
        }
        else AnimalAttack(); 
    }

    void AnimalAttack()
    {
        clawsClip.Play();
        Vector3 rotation = Vector3.zero;
        if (!facingRight)
            rotation += new Vector3(0, 0, 180);
        if (playerSwapForms.currentForm == 1)
            Instantiate(claws, attackPoint.position, Quaternion.Euler(rotation));
        if (playerSwapForms.currentForm == 2)
            Instantiate(bearClaws, attackPoint.position, Quaternion.Euler(rotation));
    }

    void DruidAttack()
    {
        piyClip.Play();

        Vector3 difference = camera.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        Instantiate(druidShot, firePoint.position, Quaternion.Euler(0f, 0f, rotZ));
    }

    void AddMana(int value)
    {
        currentMana += value;
        if (currentMana > mana)
            currentMana = mana;
        UpdateManaBar();
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

    void UpdateManaBar()
    {
        manaBar.value = (float)currentMana / mana;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "ManaBonus":
                int value = other.GetComponent<ManaBonus>().get_manaPoints();
                AddMana(value);
                break;
            default:
                break;
        }
    }
}
