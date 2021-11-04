using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiTargetLoad : MonoBehaviour
{
    [SerializeField]
    private CinemachineTargetGroup targetGroup = null;

    void OnPlayerJoined(PlayerInput playerInput) 
    {
        targetGroup.AddMember(playerInput.gameObject.transform, 1, 2);
    }
}
