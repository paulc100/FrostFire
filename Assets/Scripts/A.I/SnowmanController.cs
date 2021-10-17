using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SnowmanController : MonoBehaviour
{
    // The AI object.
    public NavMeshAgent agent;

    // The campfire position.
    private Transform campfire;

    // range that snowman will stop from player.
    private int sightRadius;
    public int fightRadius = 2;

    private bool isLockedOnPlayer;

    // Start is called before the first frame update.
    void Start()
    {
        sightRadius = 6;
        // Setting target location as campfire.
        campfire = GameObject.Find("Campfire").transform;
        Move(campfire);
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        checkForPlayers(transform.position);
    }
    
    private void Move(Transform target) {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
    }   

    private void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)  {
            if (hitCollider.tag == "Player") {
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                } else {
                    //isLockedOnPlayer = true;
                    Move(hitCollider.transform);
				}
                return;
            }
        }
        //isLockedOnPlayer = false;
        Move(campfire);
    }
}