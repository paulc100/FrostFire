using UnityEngine;
using UnityEngine.AI;

public class SnowmanController : MonoBehaviour
{
    // The AI object.
    public NavMeshAgent agent;

    // The target location for snowmen.
    private Transform target;

    // default range
    public int range = 1;

    // Start is called before the first frame update.
    void Start()
    {
        // Setting target location as campfire.
        target = GameObject.Find("Campfire").transform;
        Move();
    }   

    // Update is called once per frame
    void Update()
    {
        //if (range >= Vector3.Distance(transform.position, "ENTER PLAYER OBJETCS/TAG HERE".transform.position) {
        //    Stop();
		//}
        //
        //if (agent.isStopped && range < Vector3.Distance(transform.position, "ENTER PLAYER OBJETCS/TAG HERE".transform.position) {
        //    Move();
		//}
    }
    
    private void Move() {
        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    private void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
    }   

    private void checkForPlayer() {
            
	}
}
