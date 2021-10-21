using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenCamera : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public GameObject target;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    public PlayerInput playerInput;

    private float targetDistance;
    private float xRotation;
    private float yRotation;
    private InputAction lookAction;
    private Vector2 mouseDelta = Vector2.zero;

    void Start () {
        targetDistance = Vector3.Distance(transform.position, target.transform.position);
        lookAction = playerInput.actions["Look"];
    }

    void Update () {
        mouseDelta = lookAction.ReadValue<Vector2>();

        yRotation = mouseDelta.x * turnSpeed * Time.deltaTime;
        xRotation += mouseDelta.y * turnSpeed * Time.deltaTime;
        

        xRotation = Mathf.Clamp(xRotation, minTurnAngle, maxTurnAngle);

        transform.eulerAngles = new Vector3(-xRotation, transform.eulerAngles.y + yRotation, 0);
        transform.position = target.transform.position - (transform.forward * targetDistance);
    }
}
