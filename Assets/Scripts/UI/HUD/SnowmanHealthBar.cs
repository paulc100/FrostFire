using UnityEngine;
using UnityEngine.UI;

public class SnowmanHealthBar : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    private Image healthFill = null; 

    private Snowman snowmanReference = null;

    private float currentHealth;
    private float healthPercentage;
    private float maxHealth;

    private void Awake() 
    {
        snowmanReference = GetComponent<Snowman>();

        maxHealth = snowmanReference.maxHealth;
    } 

    private void Update() => UpdateHealthIndicator();

    private void UpdateHealthIndicator()
    {
        currentHealth = snowmanReference.health;

        CalculateHealthBarFill();
    }

    private void CalculateHealthBarFill() => healthFill.fillAmount = currentHealth / maxHealth;
}
