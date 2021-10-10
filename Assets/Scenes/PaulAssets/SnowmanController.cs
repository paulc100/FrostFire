using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanController : MonoBehaviour
{
    //public SnowmenSpawner SnowmenSpawner;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Spawned");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        //SnowmenSpawner.Script.snowmanCount -= 1;
    }
}
