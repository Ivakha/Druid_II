using UnityEngine;
using System.Collections;

public class LifeBonus : MonoBehaviour {

    [SerializeField]
    int healthPoints = 2;

    public int get_healthPoints()
    {
        return healthPoints;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
