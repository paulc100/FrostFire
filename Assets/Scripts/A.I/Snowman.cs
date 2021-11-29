using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public int health;
    public int maxHealth = 2;
    //private Powerup speedPowerup;

    private void Awake() {
        health = maxHealth;
        //speedPowerup = GetComponentInChildren<Powerup>();
    }

    public bool damage(int damageValue)
    {
        health -= damageValue;
        if (health == 1) {
            takeDamage();
		} else if (health <= 0) {
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    void takeDamage () {
        GetComponent<Renderer>().material.color = Color.red;
    }
}
