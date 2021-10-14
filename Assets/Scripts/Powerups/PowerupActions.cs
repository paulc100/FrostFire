using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActions : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    public void HighSpeedStartAction()
    {
        playerController.playerSpeed *= 2.0f;
    }

    public void HighSpeedEndAction()
    {
        playerController.playerSpeed /= 2.0f;
    }
}
