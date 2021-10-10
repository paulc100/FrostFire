using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSnowmenCount : MonoBehaviour
{
    static int _count;

    void OnEnable() { _count++; }

    void OnDestroy() { _count--; }

    public int GetActiveCount() { return _count; }
}
