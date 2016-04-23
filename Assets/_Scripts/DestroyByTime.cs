using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    [SerializeField]
    float time = 5f;

	void Start ()
    {
        Destroy(gameObject, time);
	}
}
