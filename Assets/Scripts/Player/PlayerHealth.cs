using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth;
    public float currentHealth;
    public TextMeshProUGUI healthText;

    [Header("Regen Health")]
    public float regenDelay; 
    public float regenRate;  
    private float timeSinceLastDamage;
    private bool isRegenerating = false;

    [Header("Canvas")]
    public GameObject resultTextObject;
    public GameObject healthGameOverCanvas;
    private TextMeshProUGUI resultText;


    void Start()
    {
        healthGameOverCanvas.SetActive(false);
        currentHealth = maxHealth;
        resultText = resultTextObject.GetComponent<TextMeshProUGUI>();
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

    void ShowResult(string message, Color color)
    {
        resultText.text = message;
        resultText.color = color;
        resultTextObject.SetActive(true);
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        healthGameOverCanvas.SetActive(true);
        ShowResult("Perdiste", Color.red);
        Time.timeScale = 0f;
    }
}