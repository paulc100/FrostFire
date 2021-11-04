using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    [Header("Core Game Events")]
    public UnityEvent IsGameOver;
    public UnityEvent IsVictory;

    [Header("Collisions")]
    public int collisionsToEndGame = 3;
    public int currentSnowmanCollisions = 0;

    [SerializeField]
    private SnowmenSpawner snowmenSpawner;

    void Update() 
    {
        if (currentSnowmanCollisions >= collisionsToEndGame) 
        {
            GameOver();
        }

        if (snowmenSpawner.waveNumber == snowmenSpawner.waves.Length) 
        {
            Victory();
        }
    }

    void GameOver()
    {
        snowmenSpawner.enabled = false;
        IsGameOver?.Invoke();
    }

    void Victory() 
    {
        IsVictory?.Invoke();
    }
}
