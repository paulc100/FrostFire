using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class SplitScreenPlayerController : MonoBehaviour
{
    [SerializeField]
    public float playerDefaultSpeed = 2.0f;
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
    public Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;

    private void Start() {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        enemyCollision = GetComponent<EnemyCollision>();

        // Get random player color
        GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);  

        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
    }

    void Update() {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        if (direction.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            
            // Rotate player in direction of camera
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * Time.deltaTime * playerSpeed);
        }

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer) {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void OnEnable() {
        if(moveAction != null && jumpAction != null && attackAction != null) {
            moveAction.Enable();
            jumpAction.Enable();
            attackAction.Enable();
        }
    }

    public void OnDisable() {
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
            if (enemyCollision.snowmen.Count > 0)
            {
                Debug.Log(enemyCollision.snowmen.Count);
                Debug.Log(enemyCollision.snowmen[0]);
                if (enemyCollision.snowmen[0] == null)
                {
                    enemyCollision.snowmen.RemoveAt(0);
                }
                if (enemyCollision.snowmen[0] != null && enemyCollision.snowmen[0].GetComponent<Snowman>().damage(attackPower))
                {
                    enemyCollision.snowmen.RemoveAt(0);
                }
            }
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
}