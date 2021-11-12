using System;
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
            //Debug.Log("Snowman Detected");
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
            //Debug.Log("Snowmen Left");
            
        }
    }
    public void killSnowman(int attackPower)
    {
        if (snowmen.Count > 0)
        {
            Debug.Log(snowmen.Count);
            Debug.Log(snowmen[0]);
            try
            {
                if (snowmen[0] == null)
                {
                    snowmen.RemoveAt(0);
                }
                if (snowmen[0] != null && snowmen[0].GetComponent<Snowman>().damage(attackPower))
                {
                    snowmen.RemoveAt(0);
                }
            } catch(Exception e)
            {
                Debug.Log(e);
            }
            
        }
    }
}
