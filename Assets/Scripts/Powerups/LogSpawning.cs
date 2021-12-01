using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogSpawning : MonoBehaviour
{
    public GameObject Log;

    public int spawnRate;

    private bool spawned;
    private bool canSpawn;

    private Transform[] spawnPoints;
    private int spawnPosCount;

    public int spawnedLogs;

    [SerializeField]
    public int maxLogs;

    void Awake()
    {
        canSpawn = true;

        spawnPosCount = transform.childCount;
        spawnPoints = new Transform[spawnPosCount];

        for (int i = 0; i < spawnPosCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        if (canSpawn)
        {
            Spawn();
        }
    }

    public void Collected()
    {
        Debug.Log("collected");

        spawnedLogs -= 1;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(spawnRate);

        canSpawn = true;
    }

    void Spawn()
    {
        canSpawn = false;

        var randSpawn = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPositionVec = spawnPoints[randSpawn].position;
        Quaternion spawnPositionRot = spawnPoints[randSpawn].rotation;

        if (spawnedLogs < maxLogs)
        {
            Instantiate(Log, spawnPositionVec, spawnPositionRot);
        }

        spawnedLogs += 1;

        StartCoroutine(Cooldown());
    }
}
