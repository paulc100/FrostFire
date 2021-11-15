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

    [HideInInspector]
    public Warmth playerWarmthReference = null;

    private float currentWarmth;
    private float warmthPercentage;
    private float maxWarmth = 10f;

    private void Update() => UpdatePlayerWarmthIndicator();

    private void UpdatePlayerWarmthIndicator()
    {
        if (playerWarmthReference != null)
        {
            currentWarmth = playerWarmthReference.warmth;

            CalculateWarmthBarFill();

            warmthPercentage = (int) CalculateWarmthBarPercentage();
            warmthText.text = warmthPercentage + "%";
        }
    }

    private void CalculateWarmthBarFill() => warmthBar.fillAmount = currentWarmth / maxWarmth;

    private float CalculateWarmthBarPercentage() => (currentWarmth / maxWarmth) * 100;
}
