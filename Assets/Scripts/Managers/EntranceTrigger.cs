using UnityEngine;

public class BuildingEntranceTrigger : MonoBehaviour
{
    public EscapeManager escapeManager;
    public GameObject playerLight;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            escapeManager.SetInside(true);
            playerLight.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            escapeManager.SetInside(false);
            playerLight.SetActive(false);
        }
    }
}
