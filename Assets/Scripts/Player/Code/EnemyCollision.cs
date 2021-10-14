using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public List<GameObject> snowmen;
    private void Awake()
    {
        snowmen = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Snowman")
        {
            Debug.Log("Snowman Detected");
            //snowmen.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}