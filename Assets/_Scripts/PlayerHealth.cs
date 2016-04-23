using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    int health = 5;

    [SerializeField]
    Slider healthBar;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    PlayerSwapForms playerSwapForms;

    [SerializeField]
    GameController gameController;

    Animator anim;

    int currentHealth;
    bool dead = false;

	void Start () {
        currentHealth = health;
        healthBar.value = 1f;
	}
	
	public void TakeDamage(int value)
    {
        if (playerSwapForms.currentForm == 2)
            return;
        currentHealth -= value;
        UpdateHealthBar();
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            playerController.enabled = false;
            playerSwapForms.enabled = false;
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
            anim.SetTrigger("Die");
            gameController.GameOver();
        }
    }

    public void AddHealth(int value)
    {
        if (dead)
            return;
        currentHealth += value;
        if (currentHealth > health)
            currentHealth = health;
        UpdateHealthBar();
    }

    public void UpdateForm(Animator anim)
    {
        this.anim = anim;
    }

    void UpdateHealthBar()
    {
        healthBar.value = (float)currentHealth / health;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "EnemyShot":
                int damage = other.GetComponent<ShotDamage>().get_damage();
                TakeDamage(damage);
                break;
            case "LifeBonus":
                int value = other.GetComponent<LifeBonus>().get_healthPoints();
                AddHealth(value);
                break;
            default:
                break;
        }
    }
}
