using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmth : MonoBehaviour
{
    [SerializeField]
    public int warmth = 10;
    [SerializeField]
    private int warmthLostRate = 1;
    [SerializeField]
    private int warmthLostFrequency = 5;

    private bool nearCampfire = true;
    private bool nearOtherPlayer = false;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(loseWarmth());
    }
    
    IEnumerator loseWarmth()
    {
        while(!nearCampfire)
        {
            yield return new WaitForSeconds(warmthLostFrequency);
            if (nearOtherPlayer)
            {
                removeWarmth(warmthLostRate / 2);
            } else
            {
                removeWarmth(warmthLostRate);
            }
        }
    }

    private void removeWarmth(int damage)
    {
        warmth -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (warmth == 0)
        {
            //player death fall over
        }
    }
}
