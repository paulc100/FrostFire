using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField]
    private float fuelLossFrequency = 0.1f;
    [SerializeField]
    private float fuelLossRate = 0.01f;

    [SerializeField]
    private GameEventManager gameState;

    private List<GameObject> players = new List<GameObject>();
    //private Renderer campfireRenderer;

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
        //campfireRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snowman")
        {
            FindObjectOfType<AudioManager>().Play("CampfireLife");
            removeFuel(1);
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
}
