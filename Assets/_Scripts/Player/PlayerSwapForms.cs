using UnityEngine;
using System.Collections;

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
    PlayerController playerController;

    public int currentForm { get; set; }

    void Awake()
    {
        playerController.UpdateForm
            (formsAnimators[0], formsSpeeds[0], formsBackwardSpeeds[0], formsJumpForces[0], formsTimesBetweenAttacks[0], formsTimesWhileAttack[0]);
        currentForm = 0;
    }

    void Update ()
    {
        if (Input.GetButtonDown("DruidForm") || Input.GetButtonDown("CatForm"))
        {
            int i = -1;
            if (Input.GetButtonDown("DruidForm") && currentForm != 0)
                i = 0;
            else if (Input.GetButtonDown("CatForm") && currentForm != 1)
                i = 1;
            if (i != -1)
            {
                currentForm = i;
                foreach (GameObject form in formsObjects)
                    form.SetActive(false);
                formsObjects[i].SetActive(true);
                playerController.UpdateForm
                    (formsAnimators[i], formsSpeeds[i], formsBackwardSpeeds[i], formsJumpForces[i], formsTimesBetweenAttacks[i], formsTimesWhileAttack[i]);
            }
        }
    }
}
