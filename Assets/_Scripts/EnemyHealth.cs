using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    int health = 3;

    [SerializeField]
    Animator anim;

    [SerializeField]
    EnemyController enemyChase;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    [SerializeField]
    new AudioSource audio;

    int currentHealth;
    bool dead = false;

    void Start()
    {
        currentHealth = health;
    }

    void TakeDamage(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            rigidbody2D.velocity = new Vector2(0, 0);
            rigidbody2D.isKinematic = true;
            enemyChase.enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            anim.SetTrigger("Die");
            audio.Play();
            Destroy(gameObject, 2.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.tag)
        {
            case "DruidShot":
                int damage = other.GetComponent<ShotDamage>().get_damage();
                TakeDamage(damage);
                break;
            default:
                break;
        }
    }
}
