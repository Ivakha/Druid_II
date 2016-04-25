using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    Saver saver;

    void Start()
    {
        saver = GameObject.FindWithTag("Saver").GetComponent<Saver>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            saver.UpdateSpawnPoint(transform.position);
            Destroy(gameObject);
        }
    }
}
