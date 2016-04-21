using UnityEngine;
using System.Collections;

public class PlayerSwapForms : MonoBehaviour {

    [SerializeField]
    GameObject[] formsObjects;

    [SerializeField]
    Animator[] formsAnimators;

    [SerializeField]
    float[] formsSpeeds;

    [SerializeField]
    float[] formsJumpForces;

    [SerializeField]
    PlayerController playerController;

    int currentForm;

    void Awake()
    {
        playerController.UpdateForm(formsAnimators[0], formsSpeeds[0], formsJumpForces[0]);
        currentForm = 0;
    }

    void Update () {
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
                playerController.UpdateForm(formsAnimators[i], formsSpeeds[i], formsJumpForces[i]);
            }
        }
    }
}
