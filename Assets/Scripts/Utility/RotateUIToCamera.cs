using Cinemachine;
using UnityEngine;

public class RotateUIToCamera : MonoBehaviour
{
    [SerializeField]
    private bool bobCanvas = false; 
    [SerializeField]
    private float idleBobbingSpeed = 2f;
    [SerializeField]
    private float idleBobbingHeight = 1.1f;

    [HideInInspector]
    public CinemachineVirtualCamera cinemachineVirtualCamera = null;

    void Update()  
    {
        transform.LookAt(transform.position + cinemachineVirtualCamera.transform.rotation * Vector3.forward, cinemachineVirtualCamera.transform.rotation * Vector3.up);        

        if (bobCanvas)
        {
            Vector3 pos = transform.position;
            float newYPos = Mathf.Sin(Time.time * idleBobbingSpeed) + 6f;
            transform.position = new Vector3(pos.x, newYPos * idleBobbingHeight, pos.z);
        }
    }
}
