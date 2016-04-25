using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSwapForms : MonoBehaviour {

    [SerializeField, Tooltip("Place objects to enable and disable when shapeshifting. Each of them must contain Sprite Renderer, Animator and Collider")]
    GameObject[] formsObjects;

    [SerializeField, Tooltip("Place Animator components of formObject here")]
    Animator[] formsAnimators;

    [SerializeField, Tooltip("Set speed values of different forms here")]
    float[] formsSpeeds;

    [SerializeField, Tooltip("Set backwards speed values of different forms here")]
    float[] formsBackwardSpeeds;

    [SerializeField, Tooltip("Set jump force values of different forms here")]
    float[] formsJumpForces;

    [SerializeField, Tooltip("Set time between attacks of different forms here")]
    float[] formsTimesBetweenAttacks;

    [SerializeField, Tooltip("Set time while attacking of different forms here")]
    float[] formsTimesWhileAttack;

    [SerializeField]
    float rageTime = 5f;

    [SerializeField]
    PlayerController playerController;

    [SerializeField]
    PlayerHealth playerHealth;

    [SerializeField]
    Slider rageBar;

    [SerializeField]
    new AudioSource audio;

    bool rageReady = true;
    bool inRage = false;
    float time;

    public int currentForm { get; set; }

    void Awake()
    {
        playerController.UpdateForm
            (formsAnimators[0], formsSpeeds[0], formsBackwardSpeeds[0], formsJumpForces[0], formsTimesBetweenAttacks[0], formsTimesWhileAttack[0]);
        playerHealth.UpdateForm(formsAnimators[0]);
        currentForm = 0;
        time = rageTime;
        rageBar.value = time / rageTime;
    }

    void Update ()
    {
        int i = 0;
        if (Input.GetButtonDown("DruidForm") || Input.GetButtonDown("CatForm") || Input.GetButtonDown("BearForm"))
        {
            i = -1;
            if (Input.GetButtonDown("DruidForm") && currentForm != 0)
                i = 0;
            else if (Input.GetButtonDown("CatForm") && currentForm != 1)
                i = 1;
            else if (Input.GetButtonDown("BearForm") && currentForm != 2 && rageReady)
            {
                i = 2;
                rageReady = false;
                time = rageTime;
                inRage = true;
            }
            if (i != -1)
            {
                ChangeForm(i);
            }
        }

        if (inRage)
        {
            if (currentForm != 2)
            {
                inRage = false;
                time = 0;
                UpdateRageBar();
            }
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                inRage = false;
                ChangeForm(i);
            }
            UpdateRageBar();
        }
    }

    void ChangeForm(int i)
    {
        currentForm = i;
        foreach (GameObject form in formsObjects)
            form.SetActive(false);
        formsObjects[i].SetActive(true);
        playerController.UpdateForm
            (formsAnimators[i], formsSpeeds[i], formsBackwardSpeeds[i], formsJumpForces[i], formsTimesBetweenAttacks[i], formsTimesWhileAttack[i]);
        playerHealth.UpdateForm(formsAnimators[i]);
        audio.Play();
    }

    void UpdateRageBar()
    {
        rageBar.value = time / rageTime;
    }

    public void TurnOffSounds()
    {
        audio.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RageBonus")
        {
            if(!inRage)
                rageReady = true;
            time = rageTime;
            UpdateRageBar();
        }
    }

}
