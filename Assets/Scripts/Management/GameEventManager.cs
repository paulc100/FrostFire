using UnityEngine;
using UnityEngine.Events;

public class GameEventManager : MonoBehaviour
{
    [Header("Core Game Events")]
    public UnityEvent IsGameOver;
    public UnityEvent IsVictory;

    public int currentSnowmanCollisions = 0;

    [SerializeField]
    private SnowmenSpawner snowmenSpawner;
    [SerializeField]
    private Campfire campfire;

    void Update() 
    {   
        if (campfire.remainingFuel <= 0) 
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
