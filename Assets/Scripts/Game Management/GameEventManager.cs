using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    public UnityEvent isGameOver;
    public UnityEvent isVictory;
    public int collisionsToEndGame = 3;

    [SerializeField]
    private SnowmenSpawner snowmenSpawner;

    private Renderer campfireRenderer;
    private int currentSnowmanCollisions = 0;

    void Awake() {
        campfireRenderer = GetComponent<Renderer>();
    }

    void Update() {
        if (currentSnowmanCollisions >= collisionsToEndGame) {
            GameOver();
        }

        if (snowmenSpawner.waveNumber == snowmenSpawner.waves.Length) {
            Victory();
        }
    }

    void GameOver() {
        isGameOver?.Invoke();
    }

    void Victory() {
        isVictory?.Invoke();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Snowman") {
            currentSnowmanCollisions += 1;
            Destroy(other.gameObject);
        }

        switch(currentSnowmanCollisions) {
            case 1:
                campfireRenderer.material.color = new Color(0.9f, 0.9f, 0.2f);
                break;
            case 2:
                campfireRenderer.material.color = new Color(1.0f, 0.64f, 0f);
                break;
            case 3:
                campfireRenderer.material.color = Color.red;
                break;
            default:
                campfireRenderer.material.color = Color.grey;
                break;
        }
    }
}
