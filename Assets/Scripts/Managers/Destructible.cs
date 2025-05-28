using UnityEngine;

public class Destructible : MonoBehaviour
{
    public ObjectiveManager objectiveManager;

    public float maxHealth;
    public float currentHealth;

    private bool isDestroyed = false; 

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed) return; 

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            DestroyObject();
        }
    }

    public void DestroyObject()
    {
        if (isDestroyed) return; 

        isDestroyed = true;
        objectiveManager.ObjectiveDestroyed();
        Destroy(gameObject);
    }
}

