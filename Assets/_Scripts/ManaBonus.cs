using UnityEngine;
using System.Collections;

public class ManaBonus : MonoBehaviour {

    [SerializeField]
    int manaPoints = 2;

    public int get_manaPoints()
    {
        return manaPoints;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
