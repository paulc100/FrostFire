using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
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
    }

    public void ClearPauseMenu() {
        pauseMenu.SetActive(false);
        isPauseMenuUp = false;
    }

    // These are currently displayed on the pause menu for testing purposes until more game logic is created 
    public void ShowGameOverMenu() {
        gameOverMenu.SetActive(true);
        ClearPauseMenu();
    }

    public void ShowVictoryMenu() {
        victoryMenu.SetActive(true);
        ClearPauseMenu();
    }
}
