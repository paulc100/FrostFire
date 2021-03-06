using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupBehaviour : MonoBehaviour
{
    public PowerupController controller;
    [SerializeField]
    private Powerup powerup;
    [SerializeField]
    private float idleRotationSpeed = 50f;
    [SerializeField]
    private float idleBobbingSpeed = 2f;
    [SerializeField]
    private float idleBobbingHeight = 1.1f;

    private Transform transform_;
    // Start is called before the first frame update
    private void Awake()
    {
        transform_ = transform;
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ActivatePowerup();
            gameObject.SetActive(false);
        }
    }
    private void ActivatePowerup()
    {
        controller.ActivatePowerup(powerup);
    }

    public void SetPowerup(Powerup powerup)
    {
        this.powerup = powerup;
        gameObject.name = powerup.name;
    }
}
