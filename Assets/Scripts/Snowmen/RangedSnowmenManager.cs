using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSnowmenManager : MonoBehaviour
{
    static int _count;
    public int UniqueID;

    void OnEnable() {
        _count++;
        //UniqueID = Random.Range(100, 200);
    }

    void OnDestroy() {
        _count--;
        SnowmenSpawner.waveTotalSnowmanCount--;
    }

    public int GetActiveCount() {
        return _count;
    }
}
