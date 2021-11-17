using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    private Vector3 target;
    private GameObject snowman;
    private float attackDamage;
    public float speed = 20.0f;
    public GameObject snowballParticle;
    


	void Start() {
    }
	public void Seek(Vector3 _target, float _attackDamage)
    {
        target = _target;
        attackDamage = _attackDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target - transform.position;
        float disPerFrame = speed * Time.deltaTime;


        transform.Translate(dir.normalized * disPerFrame, Space.World);

        //For player hit only
        if (dir.magnitude <= disPerFrame) {
            HitTarget();
            return;
        }
    }
    private void OnTriggerEnter(Collider other) {   
        if (other.gameObject.tag == "Player") {
            //Player takes damage
            other.GetComponent<Warmth>().removeWarmth(attackDamage/2, true);
            Debug.Log("warmth= " + other.GetComponent<Warmth>().warmth);
        }

        if (other.gameObject.tag != "Snowman") {
            HitTarget();
        }

    }

    void HitTarget() {
        GameObject effectIns = (GameObject)Instantiate(snowballParticle, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        Destroy(gameObject);
	}
}



