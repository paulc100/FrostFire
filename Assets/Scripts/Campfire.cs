using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField]
    private GameEventManager gameState;
    private Renderer campfireRenderer;

    private float campfireRadius = 10f;
    private List<GameObject> players = new List<GameObject>();

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        campfireRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snowman")
        {
            FindObjectOfType<AudioManager>().Play("CampfireLife");
            gameState.currentSnowmanCollisions += 1;
            Destroy(other.gameObject);
        }

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
        }
    }
    private void Update()
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
            if(!newPlayers.Exists(x=>x.GetInstanceID()==player.GetInstanceID()))
            {
                player.GetComponent<Warmth>().isAwayCampfire();
            }
        }
        players = newPlayers;
    }
}
