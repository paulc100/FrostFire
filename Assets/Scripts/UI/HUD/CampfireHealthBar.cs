using System;
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
    private Campfire campfire;

    private float currentHealth;
    private int healthPercentage;
    private float maxHealth;

    private void Awake()
    {
        maxHealth = campfire.fuelCapacity;
    }

    private void Update() => UpdateHealthIndicator();

    private void UpdateHealthIndicator()
    {
        currentHealth = campfire.remainingFuel;
        healthPercentage = (int) Math.Floor((currentHealth / maxHealth) * 100);

        CalculateHealthBarFill();

        healthText.text = healthPercentage.ToString() + "%";
    }

    private void CalculateHealthBarFill() => healthFill.fillAmount = currentHealth / maxHealth;
}
