using UnityEngine;

public class RobotSpawner : MonoBehaviour
{
    [Header("Prefab del robot")]
    public GameObject robotPrefab;

    [Header("Puntos de spawn disponibles")]
    public Transform[] spawnPoints;

    [Header("Aparición Automática")]
    public bool autoSpawnOnStart = true;

    void Start()
    {
        if (autoSpawnOnStart)
        {
            SpawnRobot();
        }
    }

    public void SpawnRobot()
    {
        foreach (Transform point in spawnPoints)
        {
            GameObject existing = point.childCount > 0 ? point.GetChild(0).gameObject : null;

            if (existing != null && !existing.activeInHierarchy)
            {
                existing.SetActive(true);

                EnemyHealth health = existing.GetComponent<EnemyHealth>();
                if (health != null)
                    health.ResetHealth();

                Debug.Log("Reactivando robot en " + point.name);
            }
            else if (existing == null)
            {
                GameObject newRobot = Instantiate(robotPrefab, point.position, point.rotation, point);
                Debug.Log("Nuevo robot en " + point.name);
            }
        }
    }
}
