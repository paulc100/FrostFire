using Mirror;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] 
    private Vector2 maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] 
    private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] 
    private Transform playerTransform = null;
    [SerializeField] 
    private CinemachineVirtualCamera virtualCamera = null;

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

    private CinemachineTransposer transposer;

    public override void OnStartAuthority()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        virtualCamera.gameObject.SetActive(true);

        enabled = true;

        PlayerControls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }

    [ClientCallback]

    private void OnEnable() => PlayerControls.Enable();

    [ClientCallback]

    private void OnDisable() => PlayerControls.Disable(); 

    private void Look(Vector2 lookAxis)
    {
        transposer.m_FollowOffset.y  = Mathf.Clamp(
            transposer.m_FollowOffset.y - (lookAxis.y * cameraVelocity.y * Time.deltaTime),
            maxFollowOffset.x,
            maxFollowOffset.y
        );

        playerTransform.Rotate(0f, lookAxis.x * cameraVelocity.x * Time.deltaTime, 0f);
    }
}
