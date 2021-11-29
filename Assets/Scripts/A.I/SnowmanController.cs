using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SnowmanController : MonoBehaviour
{
    [Header("general")]
    // The AI object.
    public NavMeshAgent agent;

    // The campfire position.
    public GameObject campfire;

    [Header("Attributes")]
    // range that snowman will stop from player.
    public int sightRadius = 6;
    public int fightRadius = 2;
    public float attackDamage = 2;
    protected bool isAttacking;

    public Animator animator;

    // Start is called before the first frame update.
    void Start()
    {
        isAttacking = false;
        // Setting target location as campfire.
        //campfire = GameObject.Find("Campfire").transform;
        Move(campfire.transform);
    }   

    // Update is called once per frame
    protected void FixedUpdate()
    {
        checkForPlayers(transform.position);
    }
    
    protected void Move(Transform target) {
        agent.isStopped = false;
        agent.SetDestination(target.position);
        animator.SetBool("Moving", true);
    }

    private void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
        animator.SetBool("Moving", false);
    }

	public virtual void Attack(Transform target) {
        //Refer to child class for code
    }

    protected virtual void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)  {
            // checks if collision was a player
            if (hitCollider.tag == "Player" && !hitCollider.transform.gameObject.GetComponent<SplitScreenPlayerController>().downed) {

                //If they are close enough to atk, stop and atk. If they are not, move closer to player.
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                    //isAttacking = true; //for collision class
                    Attack(hitCollider.transform);
                } else {
                    Move(hitCollider.transform);

                }
                return;
            }
        }
        //isAttacking = false;
        Move(campfire.transform);
    }
}