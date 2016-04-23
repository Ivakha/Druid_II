using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	void Start()
    {
        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }
}
