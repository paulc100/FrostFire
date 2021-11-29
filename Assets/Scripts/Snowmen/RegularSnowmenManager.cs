using UnityEngine;
using Cinemachine;

public class RegularSnowmenManager : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera = null;

    static int _count;
    public int UniqueID;

    private void Awake() => cinemachineVirtualCamera = GameObject.Find("MultiTargetCamera/CMVCam1").GetComponent<CinemachineVirtualCamera>();

    private void OnEnable() 
    {
        _count++;
        gameObject.GetComponentInChildren<RotateUIToCamera>().cinemachineVirtualCamera = cinemachineVirtualCamera;
    }

    private void OnDestroy() 
    {
        _count--;
        SnowmenSpawner.waveTotalSnowmanCount--;
    }

    public int GetActiveCount() 
    {
        return _count;
    }
}
