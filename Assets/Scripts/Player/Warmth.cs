using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warmth : MonoBehaviour
{
    [SerializeField]
    private int warmth = 10;
    [SerializeField]
    private int defaultWarmth = 10;
    [SerializeField]
    private int warmthLostRate = 1;
    [SerializeField]
    private int warmthRecoveryRate = 1;
    [SerializeField]
    private int warmthLostFrequency = 2;
    [SerializeField]
    private int campfireRecoveryFrequency = 1;
    [SerializeField]
    private int invulnerableCD = 2;

    private bool nearCampfire = false;
    private bool nearOtherPlayer = false;
    private bool invulnerable = false;
    private bool isRunning_AwayCampfire = false;
    private bool isRunning_Campfire = false;
    private bool isDowned = false;
    private bool campfireTimer = false;

    private Coroutine lastCampfireCoroutine;

    private SplitScreenPlayerController player;

    private void Awake()
    {
        player = GetComponent<SplitScreenPlayerController>();
    }

    /*Campfire detects the player and confirms player is nearby
     * if the player doesn't receive the campfire's confirmation, it sets nearCampfire as false;
     */
    public void isNearCampfire()
    {
        if (!nearCampfire)
        {
            if (campfireTimer)
            {
                StopCoroutine(noCampfire());
            }
            nearCampfire = true;
            //Debug.Log("Near campfire");
            campfireTimer = true;
            StartCoroutine(noCampfire());
        }
    }

    /*
     * function to remove warmth from player
     * damage: how much warmth is lost
     * snowmanDamage: is it a snowman that damaged the player
     * the player is stated as invulnerable for 2 seconds once damaged by a snowman
     */
    private void removeWarmth(int damage, bool snowmanDamage)
    {
        if (snowmanDamage)
        {
            if (!invulnerable)
            {
                warmthSubtraction(damage);
                invulnerable = true;
                StartCoroutine(invulnerabilityCD());
            }
        } else
        {
            warmthSubtraction(damage);
        }
        Debug.Log(warmth);
    }

    /*
     * calculation for warmth subtraction
     */
    private void warmthSubtraction(int damage)
    {
        if (warmth - damage <= 0)
        {
            warmth = 0;
            isDowned = true;
            player.isDowned(true);
        }
        else
        {
            warmth -= damage;
        }
    }

    /*
     * function to add warmth to player
     * givenWarmth: how much warmth is added
     */
    private void addWarmth(int givenWarmth)
    {
        if (warmth + givenWarmth > defaultWarmth)
        {
            warmth = defaultWarmth;
            isDowned = false;
            player.isDowned(false);
        } else
        {
            warmth += givenWarmth;
        }
        Debug.Log(warmth);
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        //Debug.Log("Near Campfire: " + nearCampfire);
        //Debug.Log("Player warmth level: " + warmth);
        //checks campre proximity
        if (nearCampfire)
        {
            //it disables awayFromCampfire coroutine if active
            if (isRunning_AwayCampfire)
            {
                StopCoroutine(lastCampfireCoroutine);
                isRunning_AwayCampfire = false;
            }
            //it activates campfireWarmth coroutine if not activite
            if (!isRunning_Campfire)
            {
                isRunning_Campfire = true;
                lastCampfireCoroutine = StartCoroutine(campfireWarmth());
            }
        }

        if (!nearCampfire)
        {
            //it disables campfireWarmth coroutine if active
            if (isRunning_Campfire)
            {
                StopCoroutine(lastCampfireCoroutine);
                isRunning_Campfire = false;
            }
            //it activates awayFromCampfire coroutine if not activite
            if (!isRunning_AwayCampfire && !isDowned)
            {
                isRunning_AwayCampfire = true;
                lastCampfireCoroutine = StartCoroutine(awayFromWarmth());
            }
        }
    }

    //it runs 1 second after the campfire confirms proximity
    IEnumerator noCampfire()
    {
        yield return new WaitForSeconds(1);
        nearCampfire = false;
        //Debug.Log("Away from campfire");

        campfireTimer = false;
    }

    /*
     * function that subtracts warmth while player is away from campfire proximity
     * also checks if near other player when it runs
     */
    IEnumerator awayFromWarmth()
    {
        yield return new WaitForSeconds(warmthLostFrequency);
        if (nearOtherPlayer)
        {
            removeWarmth(warmthLostRate / 2, false);
        }
        else
        {
            removeWarmth(warmthLostRate, false);
        }
        isRunning_AwayCampfire = false;
    }
    /*
     * function that adds warmth while player is within campfire proximity
     */
    IEnumerator campfireWarmth()
    {
        yield return new WaitForSeconds(campfireRecoveryFrequency);
        addWarmth(warmthRecoveryRate);
        isRunning_Campfire = false;
    }
    /*
     * player invulnerability cooldown
     */
    IEnumerator invulnerabilityCD()
    {
        yield return new WaitForSeconds(invulnerableCD);
        invulnerable = false;
    }
}
