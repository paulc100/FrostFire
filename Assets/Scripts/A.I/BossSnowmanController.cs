using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSnowmanController : RegularSnowmanController
{
    protected override void checkForPlayers(Vector3 center)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, sightRadius);
        foreach (var hitCollider in hitColliders)
        {
            // checks if collision was a player
            if (hitCollider.tag == "Player")
            {
                //If they are close enough to atk, stop and atk. If they are not, move closer to player.
                if (Vector3.Distance(hitCollider.transform.position, transform.position) <= fightRadius)
                {
                    isAttacking = true; //for collision class
                    Attack(hitCollider.transform);
                }
                break;
            }
        }
        isAttacking = false;
        Move(campfire.transform);
    }
}
