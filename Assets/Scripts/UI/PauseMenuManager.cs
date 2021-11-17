using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Required to disable/enable the game camera while navigating menus")]
    private CinemachineTargetGroup targetGroup;
    [Space]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject victoryMenu;

    [Tooltip("Required to pause/unpause the player controller while navigating menus, controllers added upon loading the game")]
    public List<SplitScreenPlayerController> playerControllers;
    public static bool isPauseMenuUp = false;

    private void Update() 
    {
        if (Input.GetButtonDown("Pause")) 
        {
            if (isPauseMenuUp) 
            {
                ClearPauseMenu();
            } 
            else 
            {
                ShowPauseMenu();
            }
        }
    }

    public void ShowPauseMenu() 
    {
        pauseMenu.SetActive(true);
        isPauseMenuUp = true;
        DisableControllerAndCamera();
    }

    public void ClearPauseMenu() 
    {
        pauseMenu.SetActive(false);
        isPauseMenuUp = false;
        EnableControllerAndCamera();
    }

    public void ShowGameOverMenu() 
    {
        gameOverMenu.SetActive(true);
        DisableControllerAndCamera();
    }

    public void ShowVictoryMenu() 
    {
        victoryMenu.SetActive(true);
        DisableControllerAndCamera();
    }

    private void DisableControllerAndCamera() 
    {
        foreach(SplitScreenPlayerController playerController in playerControllers) 
        {
            playerController.OnDisable();
        }

        targetGroup.enabled = false;
    }

    private void EnableControllerAndCamera() 
    {
        foreach(SplitScreenPlayerController playerController in playerControllers) 
        {
            playerController.OnEnable();
        }

        targetGroup.enabled = true;
    }
}
