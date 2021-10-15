using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public int uniqueID { get; set; }
    private int health;

    Snowman(int id)
    {
        uniqueID = id;
    }
    private void Awake()
    {
        health = 2;
    }
    private void FixedUpdate()
    {
        
    }
    public void damage(int damageValue)
    {
        health -= damageValue;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
