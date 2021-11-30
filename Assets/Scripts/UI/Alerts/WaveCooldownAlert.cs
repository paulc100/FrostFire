using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveCooldownAlert : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Text countdownHeading;

    private int countdownTime;
    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>(); 

    public void StartWaveCooldown(int waveCooldownTime) 
    {
        countdownTime = waveCooldownTime; 
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        animator.Play("FadeInWaveCooldown");

        while(countdownTime > 0)
        {
            countdownHeading.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        animator.Play("FadeOutWaveCooldown");
    }
}
