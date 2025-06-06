using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    public int currentWave = 1;
    public int enemiesAlive = 0;
    private bool isSpawning = false;
    private bool playerIsDead = false;

    private List<GameObject> enemyPool = new List<GameObject>();

    void OnEnable()
    {
        PlayerHealth.OnPlayerDied += OnPlayerDeath;
        EscapeManager.OnPlayerDied += OnPlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDied -= OnPlayerDeath;
        EscapeManager.OnPlayerDied -= OnPlayerDeath;
    }

    void Start()
    {
        Debug.Log($"Empieza la oleada {currentWave}");
        StartCoroutine(SpawnWave(currentWave));
    }

    void Update()
    {
        if (playerIsDead) return;
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
            if (playerIsDead) break;
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

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        health.ResetHealth();
        health.SetWaveManager(this);

        enemiesAlive++;
    }
    void DeactiveAllBots()
    {
        foreach (var enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                enemy.SetActive(false);
            }
        }
        enemiesAlive = 0;
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

        GameObject newEnemy = Instantiate(enemyPrefab);
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive--;
    }
    public void OnPlayerDeath()
    {
        if (playerIsDead) return;

        Debug.Log("Jugador ha muerto. Deteniendo oleadas y desactivando enemigos.");
        playerIsDead = true;
        StopAllCoroutines(); // Detiene cualquier SpawnWave en progreso
        DeactiveAllBots();
    }
}
