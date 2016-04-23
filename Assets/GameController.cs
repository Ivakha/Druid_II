using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject player;

    bool gameOver = false;

	void Start()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }

    public void GameOver()
    {
        gameOver = true;
    }

    public bool get_gameOver()
    {
        return gameOver;
    }
}
