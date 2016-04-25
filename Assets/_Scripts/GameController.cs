using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField]
    GameObject player;

    [SerializeField]
    GameObject boss;

    [SerializeField]
    Text text;

    Saver saver;

    int currentScene = 1;
    bool gameOver = false;

	void Start()
    {
        saver = GameObject.FindWithTag("Saver").GetComponent<Saver>();
        saver.SpawnPlayer();
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer, true);
    }

    void Update()
    {
        if(gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentScene);
        }

        if(boss == null)
        {
            text.text = "You have saved your forest!\nThank you for playing! :)";
        }
    }

    public void GameOver()
    {
        gameOver = true;
        text.text = "Game over!\nPress 'R' to restart";
    }

    public bool get_gameOver()
    {
        return gameOver;
    }
}
