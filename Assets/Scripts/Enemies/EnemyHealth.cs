using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public GameObject explosionEffectPrefab; 

    private WaveManager waveManager;

    public AudioClip explosion;
    public AudioSource audioSource;

    void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
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
    }
}
