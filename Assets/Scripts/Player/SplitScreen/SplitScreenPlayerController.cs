using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class SplitScreenPlayerController : MonoBehaviour
{
    [SerializeField]
    public float playerDefaultSpeed = 10.0f;
    [SerializeField]
    public int playerDefaultAttack = 1;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 1000f;
    [SerializeField]
    private float playerDetectionRadius = 5f;

    public int attackPower = 1;
    public float playerSpeed = 10.0f;
    private bool attackAvailable = true;
    private bool downed = false;

    private CharacterController controller;
    private PlayerInput playerInput;
    private EnemyCollision enemyCollision;
    private Warmth warmth;
    private List<GameObject> players = new List<GameObject>();

    public Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction attackAction;
    private InputAction shareAction;

    private void Awake() {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        enemyCollision = GetComponent<EnemyCollision>();
        warmth = GetComponent<Warmth>();

        moveAction = playerInput.actions["Movement"];
        jumpAction = playerInput.actions["Jump"];
        attackAction = playerInput.actions["Attack"];
        shareAction = playerInput.actions["ShareHP"];
    }

    void Update() {
        move();
        if(shareAction.ReadValue<float>() == 1 && !downed)
        {
            shareWarmth();
        }
    }

    private void shareWarmth()
    {
        players.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, playerDetectionRadius);
        foreach (Collider hit in hits)
        {
            if (hit.tag == "Player" && gameObject.GetInstanceID() != hit.transform.gameObject.GetInstanceID())
            {
                hit.transform.gameObject.GetComponent<Warmth>().isNearPlayer();
                players.Add(hit.transform.gameObject);
            }
        }
        if (players.Count > 0)
        {
            warmth.shareWarmth(players);
        }
    }

    //Player movement
    private void move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float rotationAngle = cameraTransform == null ? 0f : cameraTransform.eulerAngles.y;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + rotationAngle;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // Rotate player in direction of camera
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * Time.deltaTime * playerSpeed);
        }

        // Changes the height position of the player../ Jump
        if (jumpAction.triggered && groundedPlayer && !downed)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        //gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        //controller
        controller.Move(playerVelocity * Time.deltaTime);
    }

    //when player controllers are reenabled
    public void OnEnable() {
        if(moveAction != null && jumpAction != null && attackAction != null) {
            moveAction.Enable();
            jumpAction.Enable();
            attackAction.Enable();
        }
    }
    //when player controllers are disabled
    public void OnDisable() {
        moveAction.Disable();
        jumpAction.Disable();
        attackAction.Disable();
    }

    //listen to attack call
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

    //when warmth is zero, player is downed, function is called
    //when player is revived, function is called
    public void isDowned(bool status)
    {
        downed = status;
        if (downed)
        {
            playerSpeed = 2f;
            attackAvailable = false;
        } else
        {
            //Debug.Log("isDowned() ran");
            playerSpeed = playerDefaultSpeed;
            attackAvailable = true;
        }
    }

    //cooldown for each attack
    IEnumerator attackCoroutine()
    {
        //Debug.Log("Attack cooldown started at timestamp: " + Time.time);
        yield return new WaitForSeconds(1);
        //Debug.Log("Attack available at: " + Time.time);
        attackAvailable = true;
    }
}