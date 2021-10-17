using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActions : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private GameObject playerObject;

    public void HighSpeedStartAction()
    {
        playerController.playerSpeed = Mathf.Clamp((playerController.playerSpeed *= 2.0f), 10.0f, 20.0f);
        playerObject.GetComponent<Renderer>().material.color = new Color32(250, 0, 222, 255);
    }

    public void HighSpeedEndAction()
    {
        playerController.playerSpeed = Mathf.Clamp((playerController.playerSpeed /= 2.0f), 10.0f, 20.0f);
        playerObject.GetComponent<Renderer>().material.color = new Color32(52, 217, 59, 255);
    }

    public void StrongAttackStartAction()
    {
        playerController.attackPower = Mathf.Clamp((playerController.attackPower*= 2), 1, 2);
        playerObject.GetComponent<Renderer>().material.color = new Color32(0, 46, 255, 255);
    }

    public void StrongAttackEndAction()
    {
        playerController.attackPower = Mathf.Clamp((playerController.attackPower/= 2), 1, 2);
        playerObject.GetComponent<Renderer>().material.color = new Color32(52, 217, 59, 255);
    }
}
