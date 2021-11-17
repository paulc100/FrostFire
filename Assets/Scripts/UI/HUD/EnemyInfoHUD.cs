using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoHUD : MonoBehaviour
{
    [Header("Snowmen Information")]
    [SerializeField]
    private SnowmenSpawner snowmenSpawner = null;

    [Header("UI")]
    [SerializeField]
    private Text waveNumberText = null;
    [SerializeField]
    private Text enemyNumberText = null;

    private int currentWaveNumber;
    private int maximumWaveNumber;
    private int currentEnemyCount;

    private void Awake() => maximumWaveNumber = snowmenSpawner.waves.Length;

    private void Update()
    {
        currentWaveNumber = snowmenSpawner.waveNumber + 1;
        waveNumberText.text = currentWaveNumber + "/" + maximumWaveNumber;

        enemyNumberText.text = SnowmenSpawner.waveTotalSnowmanCount.ToString();
    }
}
