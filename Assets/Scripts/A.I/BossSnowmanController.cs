using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowmanController : SnowmanController
{
    [Header("Attributes")]
    public float atkRate = 0.3f;
    public float knockbackPower = 15f;
    public float knockbackDuration = 0.5f;
    public float throwRadius = 20f;

    [Header("Effects")]
    public GameObject snowParticle;
    public GameObject snowBallParticle;

    [Header("Snowball")]
    public GameObject snowballPrefab;
    public Transform throwPoint;
    private float rangedAtkCooldown = 0f;
    public float throwRate = 0.1f;
    public float snowballDamage = 10;

    public new void FixedUpdate() {
        base.FixedUpdate();
        rangedAtkCooldown -= Time.deltaTime;
        checkForSnowballTargets();

    }

	public override void Attack(Transform target) {
        GameObject player = target.gameObject;
        Warmth warmth = player.GetComponent<Warmth>();

        if (isAttacking && !warmth.isDowned && atkCoolDown <= 0f) {
            animator.SetTrigger("Attack");
            knockBack(target, knockbackPower, knockbackDuration);

            //Do damage if player is not invulnerable
            if (!player.GetComponent<Warmth>().invulnerable) warmth.removeWarmth(attackDamage, true);
            atkCoolDown = 1f / atkRate; 
        }
    }
    public void rangedAttack(Transform target) {
        if (rangedAtkCooldown <= 0f) {
            //<ANIMATION HERE>
            SimulateProjectile(target);

            // reset cooldown
            rangedAtkCooldown = 1f / throwRate;
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

        //Knockback Animation
        while (timeleft > 0) {
            target.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -power); 
            timeleft -= Time.deltaTime;
            yield return null;
        }
        target.gameObject.GetComponent<CharacterController>().enabled = true;
    }

    void SimulateProjectile(Transform target) {
        GameObject snowballClone = (GameObject)Instantiate(snowballPrefab, throwPoint.position, throwPoint.rotation);
        Snowball snowball = snowballClone.GetComponent<Snowball>();
        snowball.snowballParticle = snowBallParticle;

        if (snowball != null) {
            snowball.Seek(target.position, snowballDamage);
        }
    }

    private void checkForSnowballTargets() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, throwRadius);
        foreach (var hitCollider in hitColliders) {
            // validating target
            float dis = Vector3.Distance(hitCollider.transform.position, transform.position);
            if (hitCollider.tag == "Player" && !hitCollider.gameObject.GetComponent<Warmth>().isDowned && dis > fightRadius) {
                rangedAttack(hitCollider.transform);
            }
        }
    }
}

