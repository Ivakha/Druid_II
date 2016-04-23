using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    [SerializeField]
    int health = 5;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    PlayerSwapForms playerSwapForms;

    Animator anim;

    int currentHealth;
    bool dead = false;

	void Start () {
        currentHealth = health;
	}
	
	public void TakeDamage(int value)
    {
        currentHealth -= value;
        if (currentHealth <= 0 && !dead)
        {
            dead = true;
            playerController.enabled = false;
            playerSwapForms.enabled = false;
            anim.SetTrigger("Die");
        }
    }

    public void UpdateForm(Animator anim)
    {
        this.anim = anim;
    }
}
