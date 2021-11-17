using UnityEngine;
using UnityEngine.UI;

public class CampfireHealthBar : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private Text healthText = null;
    [SerializeField]
    private Image healthFill = null; 

    [SerializeField]
    private GameEventManager gameState = null;

    private float currentHealth;
    private float healthPercentage;
    private float maxHealth;

    private void Awake() => maxHealth = gameState.collisionsToEndGame;

    private void Update() => UpdateHealthIndicator();

    private void UpdateHealthIndicator()
    {
        currentHealth = maxHealth - gameState.currentSnowmanCollisions;

        CalculateHealthBarFill();

        healthText.text = currentHealth.ToString();
    }

    private void CalculateHealthBarFill() => healthFill.fillAmount = currentHealth / maxHealth;
}
