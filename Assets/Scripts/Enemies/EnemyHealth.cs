using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private WaveManager waveManager;

    void OnEnable()
    {
        ResetHealth();
    }

    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);

        if (waveManager != null)
        {
            waveManager.OnEnemyKilled();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
