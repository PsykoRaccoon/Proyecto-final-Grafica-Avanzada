using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private int currentWave = 1;
    private int enemiesAlive = 0;
    private bool isSpawning = false;

    private List<GameObject> enemyPool = new List<GameObject>();

    void Start()
    {
        Debug.Log($"Empieza la oleada {currentWave}");
        StartCoroutine(SpawnWave(currentWave));
    }

    void Update()
    {
        if (!isSpawning && enemiesAlive == 0)
        {
            Debug.Log($"Terminaste la oleada {currentWave}");
            currentWave++;
            Debug.Log($"Empieza la oleada {currentWave}");
            StartCoroutine(SpawnWave(currentWave));
        }
    }

    IEnumerator SpawnWave(int count)
    {
        isSpawning = true;
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }

        isSpawning = false;
    }

    void SpawnEnemy()
    {
        GameObject enemy = GetEnemyFromPool();
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        enemy.transform.position = spawnPoint.position;
        enemy.SetActive(true);

        // Reset health y volver a vincular manager
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        health.ResetHealth();
        health.SetWaveManager(this);

        enemiesAlive++;
    }

    GameObject GetEnemyFromPool()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                return enemy;
            }
        }

        // Si no hay enemigos disponibles, crea uno nuevo y añádelo al pool
        GameObject newEnemy = Instantiate(enemyPrefab);
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
}
