using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrapScript : MonoBehaviour {

    [SerializeField]
    GameObject[] enemies;

    [SerializeField]
    Text text;

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            enemies[i].SetActive(true);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(5);
        text.text = "'My allies UNDER this platform will kill you...'";
        yield return new WaitForSeconds(3);
        text.text = "";
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(SpawnEnemies());
        }
    }
}
