using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpeed: MonoBehaviour
{
    private SplitScreenPlayerController splitScreenPlayerController;

    [SerializeField]
    private float idleRotationSpeed = 50f;
    [SerializeField]
    private float idleBobbingSpeed = 2f;
    [SerializeField]
    private float idleBobbingHeight = 1.1f;


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

        Debug.Log("Powerup is picked up!");

        splitScreenPlayerController.playerSpeed = Mathf.Clamp((splitScreenPlayerController.playerSpeed *= 1.5f), 10.0f, 15.0f);

        gameObject.SetActive(false);

        yield return new WaitForSeconds(10f);

        splitScreenPlayerController.playerSpeed = Mathf.Clamp((splitScreenPlayerController.playerSpeed /= 1.5f), 10.0f, 15.0f);

        Destroy(gameObject);
    
    }
}
