using Cinemachine;
using UnityEngine;

public class CoreCamera : MonoBehaviour
{
    [HideInInspector]
    public static CinemachineVirtualCamera Reference = null; 

    private void Awake() => Reference = GetComponent<CinemachineVirtualCamera>();
}
