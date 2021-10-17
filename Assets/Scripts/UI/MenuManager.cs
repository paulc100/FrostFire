using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Required to pause/unpause the player controller while navigating menus")]
    private PlayerController playerController;
    [SerializeField]
    [Tooltip("Required to disable/enable the player camera while navigating menus")]
    private CinemachineVirtualCamera playerCamera;
    [Space]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject victoryMenu;

    public static bool isPauseMenuUp = false;

    void Update() {
        if (Input.GetButtonDown("Pause")) {
            if (isPauseMenuUp) {
                ClearPauseMenu();
            } else {
                ShowPauseMenu();
            }
        }
    }

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
        isPauseMenuUp = true;
        DisableControllerAndCamera();
    }

    public void ClearPauseMenu() {
        pauseMenu.SetActive(false);
        isPauseMenuUp = false;
        EnableControllerAndCamera();
    }

    public void ShowGameOverMenu() {
        gameOverMenu.SetActive(true);
        DisableControllerAndCamera();
    }

    public void ShowVictoryMenu() {
        victoryMenu.SetActive(true);
        DisableControllerAndCamera();
    }

    private void DisableControllerAndCamera() {
        playerController.OnDisable();
        playerCamera.enabled = false;
    }

    private void EnableControllerAndCamera() {
        playerController.OnEnable();
        playerCamera.enabled = true;
    }
}
