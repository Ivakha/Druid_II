using UnityEngine;
using System.Collections;

public class ShotMove : MonoBehaviour {

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    new Rigidbody2D rigidbody2D;

    void Update()
    {
        rigidbody2D.velocity = transform.right * speed;
    }
}
