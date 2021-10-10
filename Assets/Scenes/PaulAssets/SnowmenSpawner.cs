using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmenSpawner : MonoBehaviour
{
    //public GameObject regularSnowman;
    private Transform[] spawnPoints;

    public RegularSnowmenCount regularSnowman;

    public int waveSnowmanCount;
    public int waveRate;

    private int spawnPosCount;

    void Start()
    {
        spawnPosCount = transform.childCount;
        spawnPoints = new Transform[spawnPosCount];

        for (int i = 0; i < spawnPosCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

        // Depends how we want to do the waves
        InvokeRepeating("spawnWave", 1, waveRate);
    }

    void spawnWave()
    {
        // Depends how we want to do the waves
        while (regularSnowman.GetActiveCount() < waveSnowmanCount)
        {
            for (int i = 0; i < spawnPosCount; i++)
            {
                Vector3 spawnPos = spawnPoints[i].position;

                if (regularSnowman.GetActiveCount() < waveSnowmanCount)
                {
                    if (spawnPoints[i].rotation.y == 0 || spawnPoints[i].rotation.y == 180)
                    {
                        float x = spawnPoints[i].position.x + Random.Range(-4f, 4f);
                        spawnPos[0] = x;
                        float z = spawnPoints[i].position.z + Random.Range(-1f, 1f);
                        spawnPos[2] = z;
                    } else {
                        float z = spawnPoints[i].position.z + Random.Range(-4f, 4f);
                        spawnPos[2] = z;
                        float x = spawnPoints[i].position.x + Random.Range(-1f, 1f);
                        spawnPos[0] = x;
                    }

                    Instantiate(regularSnowman, spawnPos, regularSnowman.transform.rotation);
                    spawnPos = spawnPoints[i].position;
                }
            }
        }
    }
}