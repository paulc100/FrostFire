using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerControls playerControl;
    InputAction movement;
    Rigidbody rigid;
    GameObject playerBody;
    public float playerSpeed = 10f;

    // Start is called before the first frame update
    private void Awake()
    {
        playerControl = new PlayerControls();
        playerBody = GameObject.Find("Player");
        rigid = playerBody.GetComponent<Rigidbody>();
    }

    //if script is enabled
    private void OnEnable()
    {
        //Movement initialization
        movement = playerControl.Player.Movement;
        movement.Enable();

        //Jump Bind
        playerControl.Player.Jump.performed += DoJump;
        playerControl.Player.Jump.Enable();
    }
    //when script is disabled
    private void OnDisable()
    {
        movement.Disable();
    }
    
    private void DoJump(InputAction.CallbackContext obj)
    {
        if (rigid.velocity.y <= 0 && rigid.velocity.y * 1000 > -1)
        {
            rigid.AddForce(new Vector3(0,5,0), ForceMode.Impulse);
        }
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 v2 = movement.ReadValue<Vector2>();//extracting player control data
        Vector3 v3 = new Vector3(v2.x * playerSpeed, 0, v2.y * playerSpeed);//translating player control into 3d space
        //rigid.velocity = v3;
        rigid.velocity = new Vector3(0, rigid.velocity.y, 0);
        rigid.AddForce(v3, ForceMode.Impulse);
    }
}
