using Mirror;
using UnityEngine;

public class NetworkedPlayerController : NetworkBehaviour 
{
    [Header("Movement Values")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private CharacterController controller = null;

    private Vector2 previousInput;

    private PlayerControls playerControls;
    private PlayerControls PlayerControls
    {
        get
        {
            if (playerControls != null)
                return playerControls;

            return playerControls = new PlayerControls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        PlayerControls.Player.Movement.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        PlayerControls.Player.Movement.canceled += ctx => ResetMovement();
    }

    [ClientCallback]

    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]

    private void OnDisable() => PlayerControls.Disable();

    [ClientCallback]
    private void Update() => MovePlayer();

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void MovePlayer()
    {
        Vector3 right = controller.transform.right;
        Vector3 forward = controller.transform.forward;

        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * previousInput.x + forward.normalized * previousInput.y;

        controller.Move(movement * moveSpeed * Time.deltaTime);
    }
}
