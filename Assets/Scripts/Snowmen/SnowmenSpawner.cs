using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SnowmenSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int regularCount;
        public int rangedCount;
        public int bossCount;
        public int smallCount;
    }

    [System.Serializable]
    public class WaveCooldownEvent : UnityEvent<int> {}

    private bool waveInProgress = true;
    private bool isFirstWave = true;
    private Transform[] spawnPoints;
    public static int waveTotalSnowmanCount = 0;

    [Header("Snowmen Types")]
    public RegularSnowmenManager regularSnowman;
    public RangedSnowmenManager rangedSnowman;
    public BossSnowmanManager bossSnowman;
    public SmallSnowmenManager smallSnowman;

    [Header("Alert Events")]
    [SerializeField]
    private UnityEvent OnWaveStart = null;
    [SerializeField]
    private UnityEvent OnWaveComplete = null;
    [SerializeField]
    private WaveCooldownEvent OnWaveCooldown = null;

    public Wave[] waves;
    public int waveNumber;

    public int spawnRate;
    public int waveCooldown;

    private int spawnPosCount;

    void Start()
    {
        spawnPosCount = transform.childCount;
        spawnPoints = new Transform[spawnPosCount];

        for (int i = 0; i < spawnPosCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        if (isFirstWave)
        {
            StartCoroutine(startWaveCooldown());
            isFirstWave = false;
        }
        else if (!waveInProgress)
        {
            StartCoroutine(spawnWave(waves[waveNumber], spawnRate));
        } 
        else if (IsSnowmanActiveCountZero() && DoWaveCountsMatchIndexes())
        {
            Debug.Log("Wave complete");
            OnWaveComplete?.Invoke();

            // Reset indices
            regularIndex = 0;
            rangedIndex = 0;
            bossIndex = 0;
            smallIndex = 0;

            waveNumber += 1;
            StartCoroutine(startWaveCooldown());
        }
    }

    private int regularIndex;
    private int rangedIndex;
    private int bossIndex;
    private int smallIndex;
    private int snowmanID = 0;

    IEnumerator spawnWave(Wave wave, int rate)
    {
        waveInProgress = true;
        Debug.Log("Wave in progress");
        OnWaveStart?.Invoke();

        // Update total snowman count for the wave
        waveTotalSnowmanCount = wave.regularCount + wave.rangedCount + wave.bossCount + wave.smallCount;

        while (regularIndex + rangedIndex + bossIndex + smallIndex < wave.regularCount + wave.rangedCount + wave.bossCount + wave.smallCount)
        {
            for (int i = 0; i < spawnPosCount; i++)
            {
                // Random snowman type
                var rand = Random.Range(1,5);

                if (regularIndex < wave.regularCount && rand == 1)
                {
                    spawnSnowman("Regular", spawnPoints[i], waves[waveNumber]);
                }
                else if (rangedIndex < wave.rangedCount && rand == 2)
                {
                    spawnSnowman("Ranged", spawnPoints[i], waves[waveNumber]);
                }
                else if (bossIndex < wave.bossCount && rand == 3)
                {
                    spawnSnowman("Boss", spawnPoints[i], waves[waveNumber]);
                }
                else if (smallIndex < wave.smallCount && rand == 4)
                {
                    spawnSnowman("Small", spawnPoints[i], waves[waveNumber]);
                }
                yield return new WaitForSeconds(rate);
            }
        }
    }

    IEnumerator startWaveCooldown()
    {
        OnWaveCooldown?.Invoke(waveCooldown);
        yield return new WaitForSeconds(waveCooldown);
        waveInProgress = false;
    }

    void spawnSnowman(string snowmanType, Transform spawnPosition, Wave wave)
    {
        Vector3 spawnPositionVec = spawnPosition.position;

        if (spawnPosition.rotation.y == 0 || spawnPosition.rotation.y == 180)
        {
            float x = spawnPosition.position.x + Random.Range(-50f, 50f);
            spawnPositionVec[0] = x;
            float z = spawnPosition.position.z + Random.Range(-1.5f, 1.5f);
            spawnPositionVec[2] = z;
        }
        else
        {
            float z = spawnPosition.position.z + Random.Range(-50f, 50f);
            spawnPositionVec[2] = z;
            float x = spawnPosition.position.x + Random.Range(-1.5f, 1.5f);
            spawnPositionVec[0] = x;
        }

        if (snowmanType == "Regular")
        {
            regularIndex += 1;
            snowmanID += 1;
            Instantiate(regularSnowman, spawnPositionVec, regularSnowman.transform.rotation);
            regularSnowman.UniqueID = snowmanID;
            Debug.Log("Spawned Regular Snowman");
        }
        if (snowmanType == "Ranged")
        {
            rangedIndex += 1;
            snowmanID += 1;
            Instantiate(rangedSnowman, spawnPositionVec, rangedSnowman.transform.rotation);
            rangedSnowman.UniqueID = snowmanID;
            Debug.Log("Spawned Ranged Snowman");
        }
        if (snowmanType == "Boss")
        {
            bossIndex += 1;
            snowmanID += 1;
            Instantiate(bossSnowman, spawnPositionVec, bossSnowman.transform.rotation);
            bossSnowman.UniqueID = snowmanID;
            Debug.Log("Spawned Boss Snowman");
        }
        if (snowmanType == "Small")
        {
            smallIndex += 1;
            snowmanID += 1;
            Instantiate(smallSnowman, spawnPositionVec, smallSnowman.transform.rotation);
            smallSnowman.UniqueID = snowmanID;
            Debug.Log("Spawned Small Snowman");
        }
    }

    private bool IsSnowmanActiveCountZero() => regularSnowman.GetActiveCount() + rangedSnowman.GetActiveCount() + bossSnowman.GetActiveCount() + smallSnowman.GetActiveCount() == 0; 

    private bool DoWaveCountsMatchIndexes() => regularIndex + rangedIndex + bossIndex + smallIndex == waves[waveNumber].regularCount + waves[waveNumber].rangedCount + waves[waveNumber].bossCount + waves[waveNumber].smallCount;
}