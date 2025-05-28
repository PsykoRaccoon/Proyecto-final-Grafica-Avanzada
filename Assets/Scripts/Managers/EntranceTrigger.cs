using UnityEngine;

public class BuildingEntranceTrigger : MonoBehaviour
{
    public EscapeManager escapeManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            escapeManager.SetInside(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            escapeManager.SetInside(false);
        }
    }
}
