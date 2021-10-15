using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SnowmanController : MonoBehaviour
{
    // The AI object.
    public NavMeshAgent agent;

    // The target location for snowmen.
    private Transform target;

    // range that snowman will stop from player.
    public int range;

    // Start is called before the first frame update.
    void Start()
    {
        range = 2;
        // Setting target location as campfire.
        target = GameObject.Find("Campfire").transform;
        Move();
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        checkForPlayers(transform.position);
    }
    
    private void Move() {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
    }   

    private void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, range);
        foreach (var hitCollider in hitColliders)  {
            if (hitCollider.tag == "Player") {
                Stop();
                return;
            }
        }
        Debug.Log("moving");
        Move(); 
    }
}