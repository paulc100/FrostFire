using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSnowmanController : SnowmanController
{
    [Header("Attributes")]
    public float atkRate = 0.3f;
    public float knockbackPower = 15f;
    public float knockbackDuration = 0.5f;

    [Header("Effects")]
    public GameObject snowParticle;

    public override void Attack(Transform target) {
        GameObject player = target.gameObject;
        Warmth warmth = player.GetComponent<Warmth>();

        if (isAttacking && !warmth.isDowned && atkCoolDown <= 0f) {
            animator.SetTrigger("Attack");
            knockBack(target, knockbackPower, knockbackDuration);

            //Do damage if player is not invulnerable
            if (!player.GetComponent<Warmth>().invulnerable) warmth.removeWarmth(attackDamage, true);
                
            //reset cooldowns
            atkCoolDown = 1f / atkRate; 
        }
    }

    private void knockBack(Transform target, float power, float overTime) {
        //Attack Particles
        Vector3 effectLocation = new Vector3(transform.position.x, 0, transform.position.z);
        GameObject effectIns = (GameObject)Instantiate(snowParticle, effectLocation, transform.rotation);
        Destroy(effectIns, 2f);

        //start of knockback
        StartCoroutine(knockBackCoroutine(target, power, overTime));
    }

    IEnumerator knockBackCoroutine(Transform target, float power, float overTime) {
        float timeleft = overTime;
        target.gameObject.GetComponent<CharacterController>().enabled = false;
        Vector3 moveDirection = transform.position - target.position;
        // Player knockback
        while (timeleft > 0) {
            
            target.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -power);
            //target.gameObject.GetComponent<Rigidbody>().velocity = (moveDirection.normalized * -power);
            timeleft -= Time.deltaTime;
            yield return null;
        }
        target.gameObject.GetComponent<CharacterController>().enabled = true;
        yield return null;

    }


    /*    OLD TRANSLATE VERSION
     *    IEnumerator knockBackCoroutine(Transform target, Vector3 direction, float length, float overTime) {
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
