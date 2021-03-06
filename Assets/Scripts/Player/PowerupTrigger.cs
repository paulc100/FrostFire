using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTrigger : MonoBehaviour
{
    public GameObject stick;
    public GameObject sword;
    public GameObject particle;
    public GameObject log;

    private Vector3 vectorstick = new Vector3(4, 4, 4);
    private Vector3 vectorsword = new Vector3(1, 1, 1);
    private Vector3 vectorlog = new Vector3(0.012f, 0.012f, 0.012f);
    private Vector3 disabled = new Vector3(0, 0, 0);

    public void Trigger(string name)
    {

        if(name == "sword")
        {
            stick.transform.localScale = disabled;
            sword.transform.localScale = vectorsword;
        }
        else if (name == "stick")
        {
            stick.transform.localScale = vectorstick;
            sword.transform.localScale = disabled;
        }
        else if (name == "speedon")
        {
            particle.transform.localScale = vectorsword;
        }
        else if (name == "speedoff")
        {
            particle.transform.localScale = disabled;
        }
        else if (name == "logon")
        {
            log.transform.localScale = vectorlog;
            stick.transform.localScale = disabled;
        }
        else if (name == "logoff")
        {
            log.transform.localScale = disabled;
            stick.transform.localScale = vectorstick;
        }
    }
}
