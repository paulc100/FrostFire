using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActions : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;

    private PlayerController playerController;
    private SplitScreenPlayerController splitScreenPlayerController;

    private void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();     
        splitScreenPlayerController = playerObject.GetComponentInChildren<SplitScreenPlayerController>();
    }

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

    // TODO: Refactor This 
    public SplitScreenPlayerController[] FindPlayerControllers() 
    {
        return GameObject.FindObjectsOfType<SplitScreenPlayerController>();
    }

    public void SS_HighSpeedStartAction() 
    {
        foreach (SplitScreenPlayerController controller in FindPlayerControllers()) {
            controller.playerSpeed = Mathf.Clamp((controller.playerSpeed *= 2.0f), 10.0f, 20.0f);
        }

        playerObject.GetComponentInChildren<Renderer>().sharedMaterial.color = new Color32(250, 0, 222, 255);
    }

    public void SS_HighSpeedEndAction()
    {
        foreach (SplitScreenPlayerController controller in FindPlayerControllers()) {
            controller.playerSpeed = Mathf.Clamp((controller.playerSpeed /= 2.0f), 10.0f, 20.0f);
        }

        playerObject.GetComponentInChildren<Renderer>().sharedMaterial.color = new Color32(52, 217, 59, 255);
    }

    public void SS_StrongAttackStartAction() 
    {
        foreach (SplitScreenPlayerController controller in FindPlayerControllers()) {
            controller.attackPower = Mathf.Clamp((controller.attackPower*= 2), 1, 2);
        }

        playerObject.GetComponentInChildren<Renderer>().sharedMaterial.color = new Color32(0, 46, 255, 255);
    }

    public void SS_StrongAttackEndAction() 
    {
        foreach (SplitScreenPlayerController controller in FindPlayerControllers()) {
            controller.attackPower = Mathf.Clamp((controller.attackPower/= 2), 1, 2);
        }

        playerObject.GetComponentInChildren<Renderer>().sharedMaterial.color = new Color32(52, 217, 59, 255);
    }
}
