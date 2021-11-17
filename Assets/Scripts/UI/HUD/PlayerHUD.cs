using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Active/Inactive HUD References")]
    public GameObject activeHUD = null;
    public GameObject inactiveHUD = null;

    [Header("Warmth")]
    [SerializeField]
    private Text warmthText = null;
    [SerializeField]
    private Image warmthBar = null; 

    [Header("HUD Manipulation")]
    [SerializeField]
    private GameObject warmthButton = null; 
    [SerializeField]
    private GameObject plusWarmth = null;
    [SerializeField]
    private GameObject minusWarmth = null;

    [HideInInspector]
    public Warmth playerWarmthReference = null;

    private float currentWarmth;
    private float warmthPercentage;
    private float maxWarmth = 10f;

    private void Update() 
    {
        if (playerWarmthReference != null)
        {
            UpdatePlayerWarmthIndicator();
            CheckShareWarmth();
            CheckWarmthStatus();
        }
    }

    private void UpdatePlayerWarmthIndicator()
    {
        currentWarmth = playerWarmthReference.warmth;

        CalculateWarmthBarFill();

        warmthPercentage = (int) CalculateWarmthBarPercentage();
        warmthText.text = warmthPercentage + "%";
    }

    private void CalculateWarmthBarFill() => warmthBar.fillAmount = currentWarmth / maxWarmth;

    private float CalculateWarmthBarPercentage() => (currentWarmth / maxWarmth) * 100;

    private void CheckShareWarmth()
    {
        if (playerWarmthReference.nearPlayer)
        {
            warmthButton.SetActive(true);
        } 
        else
        {
            warmthButton.SetActive(false);
        }
    }

    private void CheckWarmthStatus()
    {
        if (playerWarmthReference.nearCampfire)
        {
            plusWarmth.SetActive(true);
        }
        else
        {
            plusWarmth.SetActive(false);
        }
    }
}
