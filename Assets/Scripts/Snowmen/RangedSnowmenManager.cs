using UnityEngine;
using Cinemachine;

public class RangedSnowmenManager : MonoBehaviour
{
    static int _count;
    public int UniqueID;

    private void OnEnable() 
    {
        _count++;
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
