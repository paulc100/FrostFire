using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmenSpawner : MonoBehaviour
{
    private bool waveInProgress = false;

    private Transform[] spawnPoints;

    public RegularSnowmenManager regularSnowman;
    public RangedSnowmenManager rangedSnowman;

    [System.Serializable]
    public class Wave
    {
        public int regularCount;
        public int rangedCount;
    }

    public Wave[] waves;
    private int waveNumber;

    public int spawnRate;

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
        if (!waveInProgress)
        {
            StartCoroutine(spawnWave(waves[waveNumber], spawnRate));
        } 
        else if (regularSnowman.GetActiveCount() + rangedSnowman.GetActiveCount() == 0)
        {
            regularIndex = 0;
            rangedIndex = 0;
            waveNumber += 1;
            waveInProgress = false;
            Debug.Log("Wave complete");
        }
    }

    private int regularIndex;
    private int rangedIndex;
    private int snowmanID = 0;

    IEnumerator spawnWave(Wave wave, int rate)
    {
        waveInProgress = true;
        while (regularIndex + rangedIndex < wave.regularCount + wave.rangedCount)
        {
            for (int i = 0; i < spawnPosCount; i++)
            {
                if (regularIndex < wave.regularCount)
                {
                    spawnSnowman("Regular", spawnPoints[i], waves[waveNumber]);
                }
                else if (rangedIndex < wave.rangedCount)
                {
                    spawnSnowman("Ranged", spawnPoints[i], waves[waveNumber]);
                }
                yield return new WaitForSeconds(rate);
            }
        }
        Debug.Log("Wave in progress");
    }

    void spawnSnowman(string snowmanType, Transform spawnPosition, Wave wave)
    {
        Vector3 spawnPositionVec = spawnPosition.position;

        if (spawnPosition.rotation.y == 0)
        {
            float x = spawnPosition.position.x + Random.Range(-4f, 4f);
            spawnPositionVec[0] = x;
            float z = spawnPosition.position.z + Random.Range(-0.5f, 0.5f);
            spawnPositionVec[2] = z;
        }
        else
        {
            float z = spawnPosition.position.z + Random.Range(-4f, 4f);
            spawnPositionVec[2] = z;
            float x = spawnPosition.position.x + Random.Range(-0.5f, 0.5f);
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
    }
}