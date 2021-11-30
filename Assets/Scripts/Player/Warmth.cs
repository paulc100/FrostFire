using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WarmthNetGain
{
    LOSS = -1,
    NONE = 0,
    GAIN = 1
}
public class Warmth : MonoBehaviour
{
    [SerializeField]
    public float warmth = 10f;
    [SerializeField]
    private float defaultWarmth = 10f;
    [SerializeField]
    private int invulnerableCD = 2;
    [SerializeField]
    private float revivePercentage = 0.25f;
    

    public bool nearCampfire = false;
    public bool nearPlayer = false;
    public bool invulnerable = false;
    private bool isRunning_AwayCampfire = false;
    private bool isRunning_Campfire = false;
    public bool isDowned = false;
    public bool canShareWarmth = true;


    private float warmthLostRate = 0.02f;
    private float warmthRecoveryRate = 0.1f;
    private float warmthLostFrequency = 0.1f;
    private float campfireRecoveryFrequency = 0.1f;
    private float shareWarmthFrequency = 0.1f;
    private float warmthSharedperMillisecond = 0.1f;
    private WarmthNetGain warmthNet = WarmthNetGain.NONE;

    private float tempWarmthValue;

    private Coroutine lastCampfireCoroutine;

    private SplitScreenPlayerController player;

    private void Awake()
    {
        tempWarmthValue = warmth;
        player = GetComponent<SplitScreenPlayerController>();
    }

    /*Campfire detects the player and confirms player is nearby
     * if the player doesn't receive the campfire's confirmation, it sets nearCampfire as false;
     */
    public void isNearCampfire()
    {
        nearCampfire = true;
    }
    public void isAwayCampfire()
    {
        nearCampfire = false;
    }

    public void isNearPlayer()
    {
        nearPlayer = true;
    }
    public void isAwayPlayer()
    {
        nearPlayer = false;
    }

    public void shareWarmth(List<GameObject> players)
    {
        if (canShareWarmth)
        {
            canShareWarmth = false;
            if (!isDowned && warmth > 0)
            {
                float shareValue = warmthSharedperMillisecond;
                if (warmth - shareValue < 0)
                {
                    shareValue = warmth;

                }

                foreach (GameObject player in players)
                {
                    player.GetComponent<Warmth>().addWarmth(shareValue / players.Count);
                }
                removeWarmth(shareValue, false);
            }
            StartCoroutine(shareWarmthCoolDown());
        }
    }

    /*
     * function to remove warmth from player
     * damage: how much warmth is lost
     * snowmanDamage: is it a snowman that damaged the player
     * the player is stated as invulnerable for 2 seconds once damaged by a snowman
     */
    public void removeWarmth(float damage, bool snowmanDamage)
    {
        if (snowmanDamage)
        {
            if (!invulnerable)
            {
                warmthSubtraction(damage);
                invulnerable = true;
                StartCoroutine(invulnerabilityCD());
                Debug.Log("first Check");
                //player.flicker();

            }
        } else
        {
            warmthSubtraction(damage);
        }
        //Debug.Log(player.name + ": " + warmth);
    }

    /*
     * calculation for warmth subtraction
     */
    private void warmthSubtraction(float damage)
    {
        if (warmth - damage <= 0)
        {
            warmth = 0;
            isDowned = true;
            canShareWarmth = false;
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
    public void addWarmth(float givenWarmth)
    {
        if (warmth + givenWarmth > defaultWarmth)
        {
            warmth = defaultWarmth;
            if(isDowned)
            {
                isDowned = false;
                canShareWarmth = true;
                player.isDowned(false);
            }
            
        } else if (warmth + givenWarmth >= revivePercentage * defaultWarmth) {
            warmth += givenWarmth;
            if (isDowned)
            {
                isDowned = false;
                canShareWarmth = true;
                player.isDowned(false);
            }
        } else
        {
            warmth += givenWarmth;
        }
        //Debug.Log(player.name + ": " + warmth);
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
        if (warmth - tempWarmthValue == 0)
        {
            warmthNet = WarmthNetGain.NONE;
        } else if (warmth - tempWarmthValue > 0)
        {
            warmthNet = WarmthNetGain.GAIN;
        } else
        {
            warmthNet = WarmthNetGain.LOSS;
        }
    }



    /*
     * function that subtracts warmth while player is away from campfire proximity
     * also checks if near other player when it runs
     */
    IEnumerator awayFromWarmth()
    {
        yield return new WaitForSeconds(warmthLostFrequency);
        if (nearPlayer)
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
    IEnumerator shareWarmthCoolDown()
    {
        yield return new WaitForSeconds(shareWarmthFrequency);
        canShareWarmth = true; ;
    }
}