using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;

    [Header("Regen Health")]
    public float regenDelay; 
    public float regenRate;  
    private float timeSinceLastDamage;
    private bool isRegenerating = false;


    public TextMeshProUGUI healthText;

    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateHealthUI();

        timeSinceLastDamage += Time.deltaTime;

        if (timeSinceLastDamage >= regenDelay && currentHealth < maxHealth)
        {
            RegenerateHealth();
        }
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        timeSinceLastDamage = 0f; 

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"Health: {Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
        }
    }

    public bool Heal(float amount)
    {
        if (currentHealth >= maxHealth)
            return false;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        return true;
    }

    void RegenerateHealth()
    {
        currentHealth += regenRate * Time.deltaTime;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }


    void Die()
    {
        print("Morido");
        gameObject.SetActive(false);
    }
}