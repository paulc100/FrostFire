using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject speedPowerup;
    public GameObject damagePowerup;

    public int spawnRate;

    private bool spawned;
    private bool canSpawn;

    private Transform[] spawnPoints;
    private int spawnPosCount;

    void Awake()
    {
        spawned = false;
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
        if (!spawned && canSpawn)
        {
            Spawn();
        }
    }

    public void Collected()
    {
        Debug.Log("collected");
        spawned = false;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(spawnRate);

        canSpawn = true;
    }

    void Spawn()
    {
        canSpawn = false;

        var randPowerup = Random.Range(1,3);
        var randSpawn = Random.Range(0,3);
        Vector3 spawnPositionVec = spawnPoints[randSpawn].position;

        GameObject powerupGameObject;

        if (randPowerup == 1)
        {
            powerupGameObject = Instantiate(speedPowerup, spawnPositionVec, Quaternion.identity);
            SetupPowerupUI(powerupGameObject);
        } 
        else if (randPowerup == 2)
        {
            powerupGameObject = Instantiate(damagePowerup, spawnPositionVec, Quaternion.identity);
            SetupPowerupUI(powerupGameObject);
        }

        spawned = true;
    }

    private void SetupPowerupUI(GameObject powerupGameObject) =>  powerupGameObject.GetComponentInChildren<RotateUIToCamera>().cinemachineVirtualCamera = CoreCamera.Reference; 
}
