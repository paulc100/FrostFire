using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerDefaultSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 1000f;

    public int attackPower = 1;
    public float playerSpeed = 2.0f;
    private bool attackAvailable = true;

    private CharacterController controller;
    private PlayerInput playerInput;
    private EnemyCollision enemyCollision;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        enemyCollision = GetComponent<EnemyCollision>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    public void OnEnable()
    {
        if(moveAction != null && jumpAction != null && attackAction != null)
        {
            moveAction.Enable();
            jumpAction.Enable();
            attackAction.Enable();
        }
    }

    public void OnDisable()
    {
        moveAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();
    }

    public void OnAttack(InputValue val)
    {
        if (attackAvailable)
        {
            //disable attack once called
            attackAvailable = false;
            enemyCollision.killSnowman(attackPower);
            //enable attack after one second
            StartCoroutine(attackCoroutine());
        }
    }
    IEnumerator attackCoroutine()
    {
        Debug.Log("Attack cooldown started at timestamp: " + Time.time);
        yield return new WaitForSeconds(1);
        Debug.Log("Attack available at: " + Time.time);
        attackAvailable = true;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);


        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        

        //playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //Rotate toward camera
        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}