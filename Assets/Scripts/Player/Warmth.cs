using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmth : MonoBehaviour
{
    [SerializeField]
    private int warmth = 10;
    [SerializeField]
    private int warmthLostRate = 1;
    [SerializeField]
    private int warmthLostFrequency = 5;

    public bool nearCampfire;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    IEnumerator loseWarmth()
    {
        while(!nearCampfire)
        {
            yield return new WaitForSeconds(warmthLostFrequency);
            warmth -= warmthLostRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
