using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class SnowmanController : MonoBehaviour
{
    // The AI object.
    public NavMeshAgent agent;

    // The campfire position.
    private Transform campfire;

    // range that snowman will stop from player.
    private int sightRadius;
    public int fightRadius = 2;
    public int attack = 2;

    private bool isLockedOnPlayer;
    private IEnumerator attackDelay;

    private Collider[] colliders;
    private bool isAttacking;
    private bool isHit = false;

    // Start is called before the first frame update.
    void Start()
    {
        isAttacking = false;
        sightRadius = 6;
        fightRadius = 2;
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

	private void OnTriggerEnter(Collider other) {
        if (!isAttacking && other.gameObject.tag == "Player") {
            //Player takes damage
            other.GetComponent<Warmth>().removeWarmth(attack);
            Debug.Log("warmth= " + other.GetComponent<Warmth>().warmth);

            float force = 1000;
       
            
            // Knock back that doesn't work lol
            /*Vector3 dir = other.gameObject.transform.position - transform.position;
            dir = -dir.normalized;
            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * force); */
            
        }
	}

	public void Attack(Vector3 position) {
        //attack start
        isAttacking = true;
        //animation end 
        isAttacking = false;
}

	/*private IEnumerator ChargeAndAttack(float delayTime) {
        yield return new WaitForSeconds(delayTime);
        isAttacking = false;
    }*/

    private void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)  {
            if (hitCollider.tag == "Player") {
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                    if (!isAttacking) {
                        Attack(hitCollider.transform.position);
					}
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