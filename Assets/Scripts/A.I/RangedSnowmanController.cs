using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSnowmanController : SnowmanController
{


    //public float firingAngle = 45.0f;
    //public float firingGravity = 9.8f;
    //private GameObject clone;

    [Header("Snowball")]

    public GameObject snowballPrefab;
    public Transform throwPoint; 
    public float throwRate = 1f;
    private float throwCoolDown = 0f;
    private float throwOffSetMax = 2;
    private float throwOffSetMin = -2;

    public override void Attack(Transform target) {

        if (throwCoolDown <= 0f) {
            SimulateProjectile(target);
            throwCoolDown = 1f / throwRate;
        }

        throwCoolDown -= Time.deltaTime;
        }

	void Start() {
       //Transform snowball = Instantiate(snowballPrefab.transform) as Transform;
        //Physics.IgnoreCollision(snowball.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void SimulateProjectile(Transform target) {
        GameObject snowballClone = (GameObject)Instantiate(snowballPrefab, throwPoint.position, throwPoint.rotation);
        Snowball snowball = snowballClone.GetComponent<Snowball>();

        if (snowball != null) {
            snowball.Seek(target.position, attackDamage);
        }
	}

    Vector3 GetRandomVector() {
        return new Vector3(UnityEngine.Random.Range(throwOffSetMin, throwOffSetMax), 
            UnityEngine.Random.Range(throwOffSetMin, throwOffSetMax), 
            UnityEngine.Random.Range(throwOffSetMin, throwOffSetMax));
    }
}


/*IEnumerator SimulateProjectile(Transform target) {
        
        // Short delay added before Projectile is thrown
        createProjectile();
        yield return new WaitForSeconds(1.5f);
        Transform Projectile = clone.transform;

        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / firingGravity);

        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        float flightDuration = target_Distance / Vx;

        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);

        float elapse_time = 0; 
        Debug.Log("start of loop");
        while (elapse_time < flightDuration) {
            Debug.Log("LOOP OCCURING");

            Projectile.Translate(10, (Vy - (firingGravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);

            elapse_time += Time.deltaTime;
            yield return null;
        }
        Debug.Log("end of snowball");
        removeProjectile();
        isAttacking = false;
    }*/

