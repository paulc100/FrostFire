using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSnowmanController : SnowmanController
{
    [Header("Attributes")]
    private float atkCoolDown = 0f;
    public float atkRate = 0.7f;
    public float atkPower = 5f;
    public float knockbackDuration = 0.3f;

    [Header("Effects")]
    public GameObject snowParticle;
    private Collider currentCollision;

    /// <summary>
    /// NOTE TO SELF, TRIGGER KNOCK BACK INSIDE ATTACK FUNCTION ONLY WHEN ONTRIGGER HAS HIT PLAYER. 
    /// CREATE BOOLEAN VALUE TO CHECK THIS IN ATTACK (ISKNOCKED or smthing) THEN TRIGGER KNOCK BACK METHOD INSIDE ATTACK
    /// AND MAKE SURE ITS SIMULTANEIOUS WITH HEAT LOSS
    /// </summary>
    /// <param name="target"></param>

    public override void Attack(Transform target) {

        
    }

    private void OnTriggerStay(Collider other) {
        if (isAttacking && other.gameObject.tag == "Player") {

            push(other.transform);
            
            other.GetComponent<Warmth>().removeWarmth(attackDamage, true);
            
            //GameObject target = other.gameObject;
            //Attack(target.transform);
        }
    }

    void push(Transform target) {
        GameObject player = target.gameObject;

        //Check to see if player is invulnerable
        if (!player.GetComponent<Warmth>().invulnerable && !player.GetComponent<Warmth>().isDowned) {
            if (atkCoolDown <= 0f) {
                //calculate direction
                Vector3 dir = target.transform.position - transform.position;
                dir = -dir;

                Debug.Log("KNOCKED BACK STARTED");
                //Attacking Player <ANIMATION NEEDED>
                knockBack(target, dir, atkPower, knockbackDuration);

                //reset cooldown
                atkCoolDown = 1f / atkRate;
            }
        }
        atkCoolDown -= Time.deltaTime;
    }

    private void knockBack(Transform target, Vector3 direction, float length, float overTime) {

        //Attack Particles
        Vector3 effectLocation = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject effectIns = (GameObject)Instantiate(snowParticle, effectLocation, transform.rotation);
        Destroy(effectIns, 2f);

        Debug.Log("KNOCKED BACK STARTED2");

        StartCoroutine(knockBackCoroutine(target, length, overTime));

        //Start knockback coroutine
        /*direction = direction.normalized;
        StartCoroutine(knockBackCoroutine(target, direction, length, overTime));*/
    }

    IEnumerator knockBackCoroutine(Transform target, float length, float overTime) {
        Debug.Log("KNOCKED BACK 000000000000");
        float timeleft = overTime;
        target.gameObject.GetComponent<CharacterController>().enabled = false;
        while (timeleft > 0) {
            Debug.Log("KNOCKED BACK");
           
            timeleft -= Time.deltaTime;
            Vector3 moveDirection = transform.position - target.position;
            target.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -100f);
         
            timeleft -= Time.deltaTime;
            yield return null;
        }
        target.gameObject.GetComponent<CharacterController>().enabled = true;
        //target.gameObject.GetComponent<SplitScreenPlayerController>().isKnocked = false;
    }

    private void physicsKnockback(Transform target, Vector3 direction, float length, float overTime) {
        target.gameObject.GetComponent<CharacterController>().enabled = false;
        Debug.Log("KNOCKED BACK");
        Vector3 moveDirection = transform.position - target.position;
        target.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -500f);
        target.gameObject.GetComponent<CharacterController>().enabled = true;

    }

/*    IEnumerator knockBackCoroutine(Transform target, Vector3 direction, float length, float overTime) {
        float timeleft = overTime;
        while (timeleft > 0) {
            if (timeleft > Time.deltaTime)
                target.Translate(direction * Time.deltaTime / overTime * length, target);
            else
                target.Translate(direction * timeleft / overTime * length, target);
            timeleft -= Time.deltaTime;
            yield return null;
        }
        target.gameObject.GetComponent<SplitScreenPlayerController>().isKnocked = false;
    }*/
}
