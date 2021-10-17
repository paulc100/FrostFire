using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages game events
public class GameEventManager : MonoBehaviour
{
    // when snowman reaches campfire
    public UnityEvent isGameOver;

    void awake() {

        isGameOver = new UnityEvent();

    }

    void gameOver() {
        isGameOver.Invoke();
    }

    // event for game over
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Snowman") {
            Debug.Log("Snowman reached campfire");
            gameOver();
        }
    }
}
