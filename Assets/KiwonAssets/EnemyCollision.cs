using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public static GameObject[] snowmen;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Snowman")
        {

        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
