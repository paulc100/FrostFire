using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Required to disable and enable player controllers when the game is paused")]
    private PauseMenuManager menuManager;
    [SerializeField]
    private CinemachineTargetGroup targetGroup = null;

    void OnPlayerJoined(PlayerInput playerInput) 
    {
        var spawnedPlayer = playerInput.gameObject;

        // Add player to camera target group
        targetGroup.AddMember(spawnedPlayer.transform, 1, 2);

        spawnedPlayer.GetComponent<SplitScreenPlayerController>().UniqueID += 1;

        // Subscribe player's controller to the pause menu controller list 
        menuManager.playerControllers.Add(spawnedPlayer.GetComponent<SplitScreenPlayerController>());
    }
}
