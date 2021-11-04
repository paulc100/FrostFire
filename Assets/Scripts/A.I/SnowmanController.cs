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

    private bool isLockedOnPlayer;
    private IEnumerator attackDelay;

    private Collider[] colliders;
    bool isAttacking;

    // Start is called before the first frame update.
    void Start()
    {
        isAttacking = false;
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

	private void OnTriggerEnter(Collider other) {
		if (isAttacking && other.tag == "player" ) {
            //other.takeDamage

		}
	}
	public void Attack(Vector3 position) {
        isAttacking = true;
        attackDelay = delayAttack(0.5f);
        StartCoroutine(attackDelay);
        colliders[1].enabled = true;
    }

    private IEnumerator delayAttack(float delayTime) {
        while (true) {
            yield return new WaitForSeconds(delayTime);
		}
	}

    private void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)  {
            if (hitCollider.tag == "Player") {
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                    Attack(hitCollider.transform.position);
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