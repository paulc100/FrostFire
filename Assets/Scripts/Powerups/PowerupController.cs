using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupController : MonoBehaviour
{

    public GameObject powerupPrefab;

    public List<Powerup> powerups;

    public Dictionary<Powerup, float> activePowerups = new Dictionary<Powerup, float>();

    private List<Powerup> keys = new List<Powerup>();

    // Update is called once per frame
    void Update()
    {
        HandleActivePowerups();
    }

    public void HandleActivePowerups()
    {
        bool changed = false;

        if (activePowerups.Count > 0)
        {
            foreach(Powerup powerup in keys)
            {
                if (activePowerups[powerup] > 0)
                {
                    activePowerups[powerup] -= Time.deltaTime;
                }
                else
                {
                    changed = true;

                    activePowerups.Remove(powerup);

                    powerup.End();
                }
            }
        }
        if (changed)
        {
            keys = new List<Powerup>(activePowerups.Keys);
        }
    }

    public void ActivatePowerup(Powerup powerup)
    {
        if (!activePowerups.ContainsKey(powerup))
        {
            powerup.Start();
            activePowerups.Add(powerup, powerup.duration);
        }
        else 
        {
            activePowerups[powerup] += powerup.duration;
        }


        keys = new List<Powerup>(activePowerups.Keys);
    }

    public GameObject SpawnPowerup(Powerup powerup, Vector3 position)
    {
        GameObject powerupGameObject = Instantiate(powerupPrefab);

        var powerupBehaviour = powerupGameObject.GetComponent<PowerupBehaviour>();

        powerupBehaviour.controller = this;

        powerupBehaviour.SetPowerup(powerup);

        powerupGameObject.transform.position = position;

        return powerupGameObject;
    }
}
