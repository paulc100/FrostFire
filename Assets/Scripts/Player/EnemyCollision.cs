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
            //Debug.Log(snowmen.Count);
            //Debug.Log(snowmen[0]);
            FindObjectOfType<AudioManager>().Play("SnowmanHit");
            try
            {
                if (snowmen[0] == null)
                {
                    snowmen.RemoveAt(0);
                }
                if (snowmen[0] != null && snowmen[0].GetComponent<Snowman>().damage(attackPower))
                {
                    snowmen.RemoveAt(0);
                    StartCoroutine(knockBackCoroutine(snowmen[0], 15000, 0.2f));
                    FindObjectOfType<AudioManager>().Play("SnowmanDeath");
                    FindObjectOfType<AudioManager>().Stop("SnowmanHit");
                }
            } catch(Exception e)
            {
                Debug.Log(e);
            }
            
        }
    }

    IEnumerator knockBackCoroutine(GameObject target, float power, float overTime) {
        float timeleft = overTime;
        target.GetComponent<RangedSnowmanController>().Stop();
        //Knockback Animation
        while (timeleft > 0) {
            Vector3 moveDirection = transform.position - target.transform.position;
            target.gameObject.GetComponent<Rigidbody>().AddForce(moveDirection.normalized * -power);
            timeleft -= Time.deltaTime;
            yield return null;
        }
        target.GetComponent<RangedSnowmanController>().Move();
    }
}
