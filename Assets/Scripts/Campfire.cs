using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Campfire : MonoBehaviour
{
    [SerializeField]
    private GameEventManager gameState;

    [Header("Fuel Management")]
    [SerializeField]
    private float fuelLossFrequency = 0.1f;
    [SerializeField]
    private float fuelLossRate = 0.01f;

    [Header("Alert Events")]
    [SerializeField]
    private UnityEvent OnCampfireHealth_50 = null;
    [SerializeField]
    private UnityEvent OnCampfireHealth_25 = null;
    [SerializeField]
    private UnityEvent OnCampfireHealth_10 = null;

    private bool canInvokeCampfireHealth_50 = true;
    private bool canInvokeCampfireHealth_25 = true;
    private bool canInvokeCampfireHealth_10 = true;

    private List<GameObject> players = new List<GameObject>();

    private float campfireRadius = 10f;
    public float fuelCapacity = 100f;
    public float remainingFuel = 100f;
    public float logFuel = 5f;
    private bool campfireOut = false;

    private SplitScreenPlayerController splitScreenPlayerController;

    private void Awake()
    {
        remainingFuel = fuelCapacity;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(consumeFuel());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snowman")
        {
            FindObjectOfType<AudioManager>().Play("CampfireLife");
            if (other.gameObject.name.Contains("Snowman")) {
                removeFuel(5);
            }
            else if (other.gameObject.name.Contains("RangedSnowman"))
            {
                removeFuel(5);
            }
            else if (other.gameObject.name.Contains("SmallSnowman"))
            {
                removeFuel(2.5f);
            }
            else if (other.gameObject.name.Contains("BossSnowman"))
            {
                removeFuel(25);
            }
            FindObjectOfType<AudioManager>().Play("CampfireLife");
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            splitScreenPlayerController = other.gameObject.GetComponentInChildren<SplitScreenPlayerController>();

            if(splitScreenPlayerController.carryingLog == true)
            {
                addFuel(logFuel);
                splitScreenPlayerController.carryingLog = false;
            }
        }
        /*
        switch (gameState.currentSnowmanCollisions)
        {
            case 1:
                campfireRenderer.material.color = new Color(0.9f, 0.9f, 0.2f);
                break;
            case 2:
                campfireRenderer.material.color = new Color(1.0f, 0.64f, 0f);
                break;
            case 3:
                campfireRenderer.material.color = Color.red;
                break;
            default:
                campfireRenderer.material.color = Color.grey;
                break;
        }*/
    }

    private void Update()
    {
        if (!campfireOut)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, campfireRadius);
            List<GameObject> newPlayers = new List<GameObject>();
            foreach (Collider hit in hits)
            {
                if (hit.tag == "Player")
                {
                    newPlayers.Add(hit.transform.gameObject);
                    if (!players.Contains(hit.transform.gameObject))
                    {
                        hit.transform.gameObject.GetComponent<Warmth>().isNearCampfire();
                    }
                }
            }
            foreach (GameObject player in players)
            {
                if (!newPlayers.Exists(x => x.GetInstanceID() == player.GetInstanceID()))
                {
                    player.GetComponent<Warmth>().isAwayCampfire();
                }
            }
            players = newPlayers;
        }

        CheckCampfireAlertThresholds();
    }

    private void addFuel(float fuelAdded)
    {
        if(remainingFuel <= fuelCapacity - fuelAdded)
        {
            remainingFuel += fuelAdded;
        }
        else
        {
            remainingFuel = 99.9f;
        }
    }

    private void removeFuel(float fuelLost)
    {
        remainingFuel -= fuelLost;
        if (remainingFuel <= 0)
        {
            campfireOut = true;
            remainingFuel = 0;
        }
    }

    IEnumerator consumeFuel()
    {
        while(!campfireOut)
        {
            yield return new WaitForSeconds(fuelLossFrequency);
            removeFuel(fuelLossRate);
        }
    }

    private void CheckCampfireAlertThresholds()
    {
        if (remainingFuel < 10 && canInvokeCampfireHealth_10) 
        {
            OnCampfireHealth_10?.Invoke();
            canInvokeCampfireHealth_10 = false;
        } 
        else if (remainingFuel < 25 && canInvokeCampfireHealth_25)
        {
            OnCampfireHealth_25?.Invoke();
            canInvokeCampfireHealth_25 = false;
        }
        else if (remainingFuel < 50 && canInvokeCampfireHealth_50)
        {
            OnCampfireHealth_50?.Invoke();
            canInvokeCampfireHealth_50 = false;
        }

        // Check if campfire is raised above thresholds to re-display alerts 
        if (remainingFuel > 10 && !canInvokeCampfireHealth_10)
            canInvokeCampfireHealth_10 = true;

        if (remainingFuel > 25 && !canInvokeCampfireHealth_25)
            canInvokeCampfireHealth_25 = true;

        if (remainingFuel > 50 && !canInvokeCampfireHealth_50)
            canInvokeCampfireHealth_50 = true;
    }
}
