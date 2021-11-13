using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSnowmenManager : MonoBehaviour
{
    static int _count;
    public int UniqueID;

    void OnEnable() {
        _count++;
    }

    void OnDestroy() {
        _count--;
        SnowmenSpawner.waveTotalSnowmanCount--;
    }

    public int GetActiveCount() {
        return _count;
    }
}
