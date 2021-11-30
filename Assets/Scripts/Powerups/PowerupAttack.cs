using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAttack: MonoBehaviour
{
    private SplitScreenPlayerController splitScreenPlayerController;

    [SerializeField]
    private float idleRotationSpeed = 50f;
    [SerializeField]
    private float idleBobbingSpeed = 2f;
    [SerializeField]
    private float idleBobbingHeight = 1.1f;

    Vector3 bigPlayer = new Vector3(1,1,5);

    [SerializeField]
    private float timePowerup = 5.0f;

    private Light[] lights;
    

    private void Update() {
        IdleAnimation();
    }

    private void IdleAnimation() {
        transform.Rotate(Vector3.down, idleRotationSpeed * Time.deltaTime);
        
        // Bob powerup 'up and down' on a loop
        Vector3 pos = transform.position;
        // sin wave goes (-1, 1) + 2 = (1, 3) 
        float newYPos = Mathf.Sin(Time.time * idleBobbingSpeed) + 2f;
        transform.position = new Vector3(pos.x, newYPos * idleBobbingHeight, pos.z);
    }

    private void OnTriggerEnter (Collider other) {
        splitScreenPlayerController = other.gameObject.GetComponentInChildren<SplitScreenPlayerController>();
        
        if (other.CompareTag("Player")) {
            StartCoroutine( Pickup(other) );
        }
    }

    IEnumerator Pickup(Collider player) {

        splitScreenPlayerController = player.gameObject.GetComponentInChildren<SplitScreenPlayerController>();

        splitScreenPlayerController.attackPower = Mathf.Clamp((splitScreenPlayerController.attackPower *= 2), 1, 2);
        player.gameObject.GetComponent<PowerupTrigger>().Trigger("sword");

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponentInChildren<Canvas>().enabled = false;

        lights = gameObject.GetComponentsInChildren<Light>();
        foreach (var light in lights) {
            light.enabled = false;
        }

        FindObjectOfType<AudioManager>().Play("Powerup");
        FindObjectOfType<PowerupSpawner>().Collected();

        yield return new WaitForSeconds(timePowerup);
   
        splitScreenPlayerController.attackPower = Mathf.Clamp((splitScreenPlayerController.attackPower /= 2), 1, 2);
        player.gameObject.GetComponent<PowerupTrigger>().Trigger("stick");

        Destroy(gameObject);
    }
}
