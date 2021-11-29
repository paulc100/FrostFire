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

    [Header("Animations")]
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
        atkCoolDown -= Time.deltaTime;
    }
    
    public void Move(Transform target) {
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(target.position);
        animator.SetBool("Moving", true);
    }
    public void Move() {
        agent.isStopped = false;
        agent.ResetPath();
        agent.SetDestination(campfire.transform.position);
        animator.SetBool("Moving", true);

    }

    public void Stop() {
        agent.isStopped = true;
        agent.ResetPath();
        animator.SetBool("Moving", false);

    }

    public virtual void Attack(Transform target) {
        //Refer to child class for code
    }

    private void checkForPlayers(Vector3 center) {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)  {
            // checks if collision was a player
            if (hitCollider.tag == "Player" && !hitCollider.gameObject.GetComponent<Warmth>().isDowned) {

                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius ) {
                    Stop();
                    //attack phase start
                    isAttacking = true; 
                    Attack(hitCollider.transform);
                } else {
                    //IF NOT CLOSE ENOUGH WALK CLOSER
                    isAttacking = false;
                    Move(hitCollider.transform);
                }
                return;
            }
        }
        //no players in sight radius
        isAttacking = false;
        Move(campfire.transform);
    }
}