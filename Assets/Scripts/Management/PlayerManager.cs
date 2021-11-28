using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Snowman Management")]
    [SerializeField]
    private SnowmenSpawner snowmenSpawner = null;

    [Header("UI Management")]
    [SerializeField]
    [Tooltip("Required to disable and enable player controllers when the game is paused")]
    private PauseMenuManager menuManager = null;
    [SerializeField]
    [Tooltip("Required to connect player data to HUD components")]
    private HUDManager hudManager = null; 

    [Header("Camera Management")]
    [SerializeField]
    private CinemachineTargetGroup targetGroup = null;
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;

    private static List<Transform> spawnPoints = new List<Transform>();
    private int nextSpawnIndex = 0;

    private CharacterSwitcher characterSwitcher = null;

    private void Awake() => characterSwitcher = GetComponent<CharacterSwitcher>();

    public static void AddSpawnPoint(Transform transform) 
    {
        spawnPoints.Add(transform);

        spawnPoints = spawnPoints.OrderBy(x => x.GetSiblingIndex()).ToList();
    }

    public static void RemoveSpawnPoint(Transform transform) => spawnPoints.Remove(transform);

    private void SpawnPlayer(GameObject playerInstance) 
    {
        Transform spawnPoint  = spawnPoints.ElementAtOrDefault(nextSpawnIndex);

        if (spawnPoint == null) 
        {
            Debug.LogError("Missing spawn point for player " + nextSpawnIndex);
            return;
        }

        // Character Controller component must be disabled in order for transform changes to take effect
        playerInstance.GetComponent<CharacterController>().enabled = false;
        playerInstance.transform.position = spawnPoints[nextSpawnIndex].position;
        playerInstance.transform.rotation = spawnPoints[nextSpawnIndex].rotation;
        playerInstance.GetComponent<CharacterController>().enabled = true;

        nextSpawnIndex++;
    }

    private void SetupPlayerCameras(GameObject playerInstance)
    {
        targetGroup.AddMember(playerInstance.transform, 1, 2);

        // Configures player indicators to rotate to camera on spawn
        playerInstance.GetComponentInChildren<RotateUIToCamera>().cinemachineVirtualCamera = cinemachineVirtualCamera;
    }

    public void OnPlayerJoined(PlayerInput playerInput) 
    {
        var spawnedPlayer = playerInput.gameObject;

        // Give spwaned player an id based on their spawn index [0 -> 3]
        spawnedPlayer.GetComponent<SplitScreenPlayerController>().pid = nextSpawnIndex; 

        // Spawn player within scene
        SpawnPlayer(spawnedPlayer);

        // Setup player HUD
        hudManager.AddPlayerReferenceToHUD(spawnedPlayer);

        // Add player to camera target group
        SetupPlayerCameras(spawnedPlayer);

        // Subscribe player's controller to the pause menu controller list 
        menuManager.playerControllers.Add(playerInput.gameObject.GetComponent<SplitScreenPlayerController>());

        // Switch to next character for next player to load in
        characterSwitcher.SwitchNextSpawnCharacter();

        // Start the snowmen spawner if the first player spawned in
        if (!snowmenSpawner.gameObject.activeInHierarchy)    
            snowmenSpawner.gameObject.SetActive(true);
    }
}
