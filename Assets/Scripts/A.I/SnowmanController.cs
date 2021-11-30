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
    protected float atkCoolDown = 0f;
    protected bool isAttacking;

    //[Header("Animations")]
    //public Animator animator;

    // Start is called before the first frame update.
    void Start()
    {
        isAttacking = false;
        Move();
    }   

    // Update is called once per frame
    protected void FixedUpdate()
    {
        checkForPlayers();
        atkCoolDown -= Time.deltaTime;
    }
    
    public void Move(Transform target) {
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(target.position);
        //animator.SetBool("Moving", true);
    }
    //default move set to "campfire"
    public void Move() {
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(campfire.transform.position);
        //animator.SetBool("Moving", true);

    }

    public void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
        //animator.SetBool("Moving", false);

    }

    public virtual void Attack(Transform target) {
        // Refer to child attack
    }

    protected void checkForPlayers() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRadius);
        
        foreach (var hitCollider in hitColliders)  {
            // validating target
            if (hitCollider.tag == "Player" && !hitCollider.gameObject.GetComponent<Warmth>().isDowned) {
                
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                    isAttacking = true;
                    Attack(hitCollider.transform);
                } else {
                    isAttacking = false;
                    Move(hitCollider.transform);
                }
                return;
            }
        }
        isAttacking = false;
        Move(campfire.transform);
    }
}