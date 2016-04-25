using UnityEngine;
using System.Collections;

public class ShotDamage : MonoBehaviour {

    [SerializeField]
    int damage = 1;

    public int get_damage()
    {
        return damage;
    }
}
