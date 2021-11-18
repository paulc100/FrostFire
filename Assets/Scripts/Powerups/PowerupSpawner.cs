using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    private Transform[] spawnPoints;

    public int spawnRate;

    private int spawnPosCount;

    private bool spawned;

    void Start()
    {
        spawnPosCount = transform.childCount;
        spawnPoints = new Transform[spawnPosCount];

        for (int i = 0; i < spawnPosCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            CheckSpawn();
        }
    }

    void Spawn()
    {

    }

    IEnumerator CheckSpawn()
    {
        yield return new WaitForSeconds(60f);
    }
}
