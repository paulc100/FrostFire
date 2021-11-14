using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSnowmanController : SnowmanController
{
    
    public override void Attack(Transform target) {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (isAttacking && other.gameObject.tag == "Player") {
            //Player takes damage
            other.GetComponent<Warmth>().removeWarmth(attackDamage);
            Debug.Log("warmth= " + other.GetComponent<Warmth>().warmth);
        }
    }
}
