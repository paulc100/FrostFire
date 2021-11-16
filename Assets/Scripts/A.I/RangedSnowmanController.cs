using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSnowmanController : SnowmanController
{
    [Header("Snowball")]

    public GameObject snowballPrefab;
    public Transform throwPoint; 
    public float throwRate = 1f;
    private float throwCoolDown = 0f;
    private float throwOffSetMax = 2;
    private float throwOffSetMin = -2;

    public override void Attack(Transform target) {

        if (throwCoolDown <= 0f) {
            //<ANIMATION HERE OR IN METHOD BELOW>
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
