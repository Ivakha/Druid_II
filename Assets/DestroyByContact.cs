using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(tag == "DruidShot" && other.tag == "Enemy" || tag == "EnemyShot" && other.tag == "Player")
        {
            Destroy(gameObject);
        }

    }
}
