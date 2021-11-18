using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupTrigger : MonoBehaviour
{
    public GameObject stick;
    public GameObject sword;
    private Vector3 vectorstick = new Vector3(4, 4, 4);
    private Vector3 vectorsword = new Vector3(1, 1, 1);

    public void Trigger(string name)
    {

        stick = GameObject.Find("Stick");
        sword = GameObject.Find("SwordModel");

        if(name == "sword")
        {
            stick.transform.localScale -= vectorstick;
            sword.transform.localScale += vectorsword;
        }
        else if (name == "stick")
        {
            stick.transform.localScale += vectorstick;
            sword.transform.localScale -= vectorsword;
        }
        else if (name == "speedon")
        {

        }
        else if (name == "speedoff")
        {

        }
    }
}
