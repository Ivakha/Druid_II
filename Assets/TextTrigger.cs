using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextTrigger : MonoBehaviour {

    [SerializeField]
    string message;

    [SerializeField]
    Text text;

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                text.text = message;
                break;
            default:
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Player":
                text.text = "";
                break;
            default:
                break;
        }
    }
}
