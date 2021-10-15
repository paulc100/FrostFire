using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public List<GameObject> snowmen { get; set; }
    private void Awake()
    {
        snowmen = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Snowman")
        {
            Debug.Log("Snowman Detected");
            snowmen.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Snowman")
        {   
            for (int i = 0; i < snowmen.Count-1; i++)
            {
                if (other.gameObject.GetInstanceID() == snowmen[i].gameObject.GetInstanceID())
                {
                    snowmen.RemoveAt(i);
                }
            }
            Debug.Log("Snowmen Left");
            
        }
    }
}
