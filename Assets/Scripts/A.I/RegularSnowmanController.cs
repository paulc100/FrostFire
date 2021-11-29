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
    //private Collider currentCollision;

    /// <summary>
    /// NOTE TO SELF, TRIGGER KNOCK BACK INSIDE ATTACK FUNCTION ONLY WHEN ONTRIGGER HAS HIT PLAYER. 
    /// CREATE BOOLEAN VALUE TO CHECK THIS IN ATTACK (ISKNOCKED or smthing) THEN TRIGGER KNOCK BACK METHOD INSIDE ATTACK
    /// AND MAKE SURE ITS SIMULTANEIOUS WITH HEAT LOSS
    /// </summary>
    /// <param name="target"></param>
    public override void Attack(Transform target) {
        if (atkCoolDown <= 0f)
        {
            /////////// added and modified code
            //Player takes damage
            animator.SetTrigger("Attack");

            target.gameObject.GetComponent<Warmth>().removeWarmth(attackDamage, true);
            Debug.Log("warmth= " + target.GetComponent<Warmth>().warmth);
            //////////

            //calculate direction
            Vector3 dir = target.transform.position - transform.position;
            dir = -dir;

            //Attacking Player <ANIMATION NEEDED>
            knockBack(target, dir, atkPower, knockbackDuration);

            //reset cooldown
            atkCoolDown = 1f / atkRate;
        }
        atkCoolDown -= Time.deltaTime;
    }

    /*
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Activated");
        if (isAttacking && other.gameObject.tag == "Player") {
            //Player takes damage
            currentCollision = other;
            currentCollision.GetComponent<Warmth>().removeWarmth(attackDamage, true);
            Debug.Log("warmth= " + other.GetComponent<Warmth>().warmth);

            GameObject target = other.gameObject;
            Attack(target.transform);
        }
    }*/

    private void knockBack(Transform target, Vector3 direction, float length, float overTime) {
        direction = direction.normalized;
        Vector3 effectLocation = new Vector3(transform.position.x, 0, transform.position.z); 
        GameObject effectIns = (GameObject)Instantiate(snowParticle, effectLocation, transform.rotation);
        Destroy(effectIns, 2f);
        StartCoroutine(knockBackCoroutine(target, direction, length, overTime));
    }

    IEnumerator knockBackCoroutine(Transform target, Vector3 direction, float length, float overTime) {
        float timeleft = overTime;
        while (timeleft > 0) {
            if (timeleft > Time.deltaTime)
                target.Translate(direction * Time.deltaTime / overTime * length, target);
            else
                target.Translate(direction * timeleft / overTime * length, target);
            timeleft -= Time.deltaTime;
            yield return null;
        }
    }
}
