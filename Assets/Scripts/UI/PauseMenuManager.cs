using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Game Camera")]
    [SerializeField]
    [Tooltip("Required to disable/enable the game camera while navigating menus")]
    private CinemachineTargetGroup targetGroup;

    [Header("Menu References")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject victoryMenu;

    [Header("Menu Buttons")]
    [Tooltip("Used to select menu options with the gamepad as they appear")]
    [SerializeField]
    private Button pauseMenuSelectedButton = null;
    [SerializeField]
    private Button gameOverMenuSelectedButton = null;
    [SerializeField]
    private Button victoryMenuSelectedButton = null;

    [HideInInspector]
    public List<SplitScreenPlayerController> playerControllers;
    [HideInInspector]
    public static bool isPauseMenuUp = false;
    private static bool isVictoryMenuUp = false;
    private static bool isGameOverMenuUp = false;

    private void Update() 
    {
        if (Input.GetButtonDown("Pause")) 
        {
            if (isPauseMenuUp) 
            {
                ClearPauseMenu();
            } 
            else if (!isVictoryMenuUp && !isGameOverMenuUp)
            {
                ShowPauseMenu();
            }
        }
    }

    public void ShowPauseMenu() 
    {
        pauseMenu.SetActive(true);
        pauseMenuSelectedButton.gameObject.SetActive(true);
        pauseMenuSelectedButton.Select();
        isPauseMenuUp = true;

        Time.timeScale = 0;
    }

    public void ClearPauseMenu() 
    {
        pauseMenu.SetActive(false);
        isPauseMenuUp = false;

        Time.timeScale = 1;
    }

    public void ShowGameOverMenu() 
    {
        gameOverMenu.SetActive(true);
        gameOverMenuSelectedButton.gameObject.SetActive(true);
        gameOverMenuSelectedButton.Select();
        isGameOverMenuUp = true;

        DisableControllerAndCamera();
    }

    public void ShowVictoryMenu() 
    {
        victoryMenu.SetActive(true);
        victoryMenuSelectedButton.gameObject.SetActive(true);
        victoryMenuSelectedButton.Select();
        isVictoryMenuUp = true;

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
