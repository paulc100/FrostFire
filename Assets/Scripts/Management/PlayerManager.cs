using System.Collections.Generic;
using System.Linq;
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

    private static List<Transform> spawnPoints = new List<Transform>();
    private int nextSpawnIndex = 0;

    public static void AddSpawnPoint(Transform transform) 
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    public void SpawnPlayer(GameObject playerInstance) 
    {
        Transform spawnPoint  = spawnPoints.ElementAtOrDefault(nextSpawnIndex);

        if (spawnPoint == null) 
        {
            Debug.LogError("Missing spawn point for player " + nextSpawnIndex);
            return;
        }

        playerInstance.transform.position = spawnPoints[nextSpawnIndex].position;
        playerInstance.transform.rotation = spawnPoints[nextSpawnIndex].rotation;

        nextSpawnIndex++;
    }

    public void OnPlayerJoined(PlayerInput playerInput) 
    {
        var spawnedPlayer = playerInput.gameObject;

        spawnedPlayer.GetComponent<CharacterController>().enabled = false;

        SpawnPlayer(spawnedPlayer);

        spawnedPlayer.GetComponent<CharacterController>().enabled = true;

        // Add player to camera target group
        targetGroup.AddMember(spawnedPlayer.transform, 1, 2);

        // Subscribe player's controller to the pause menu controller list 
        menuManager.playerControllers.Add(playerInput.gameObject.GetComponent<SplitScreenPlayerController>());
    }
}
