using UnityEngine;
using TMPro; // <-- Importar TMP

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject explosionEffectPrefab;

    private WaveManager waveManager;

    public AudioClip explosion;
    public AudioSource audioSource;

    public TMP_Text healthText; // <-- Referencia al TMP para mostrar vida

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        ResetHealth();
        UpdateHealthText(); 
    }

    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UpdateHealthText(); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        if (explosion != null)
        {
            AudioSource.PlayClipAtPoint(explosion, transform.position);
        }

        gameObject.SetActive(false);

        if (waveManager != null)
        {
            waveManager.OnEnemyKilled();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthText(); 
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + Mathf.Max(currentHealth, 0).ToString("0");
        }
    }
}
