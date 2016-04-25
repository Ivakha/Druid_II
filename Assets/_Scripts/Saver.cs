using UnityEngine;
using System.Collections;

public class Saver : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    new GameObject camera;

    Vector3 spawnPoint;

	void Start () {
        spawnPoint = new Vector3(0,0,0);
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnPlayer()
    {
        player = GameObject.FindWithTag("PlayerObject");
        camera = GameObject.FindWithTag("MainCamera");
        player.transform.position = spawnPoint;
        camera.transform.position = spawnPoint;
    }

    public void UpdateSpawnPoint(Vector3 position)
    {
        spawnPoint = position;
    }
}
