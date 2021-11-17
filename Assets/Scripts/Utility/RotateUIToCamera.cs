using Cinemachine;
using UnityEngine;

public class RotateUIToCamera : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineVirtualCamera = null;

    void Update() => transform.LookAt(transform.position + cinemachineVirtualCamera.transform.rotation * Vector3.forward, cinemachineVirtualCamera.transform.rotation * Vector3.up);        
}
