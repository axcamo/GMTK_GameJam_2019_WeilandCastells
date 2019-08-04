using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private SpawnPoint[] spawnPoints;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private bool spawnEnemies;
    [SerializeField] private int minSpawnEnemies = 5;
    [SerializeField] private int maxSpawnEnemies = 10;
    [SerializeField] private float secondsBetweenWaves = 10;
    [SerializeField] private int maxTotalEnemies = 10;

    public static SpawnManager instance;
    private int enemyCount = 0;

    public void Awake() { if (instance == null) instance = this; }

    private void Start()
    {
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public void StartSpawning()
    {
        spawnEnemies = true;
        StartCoroutine("SpawnCoroutine");
    }

    IEnumerator SpawnCoroutine()
    {
        while (spawnEnemies)
        {
            Spawn(Random.Range(minSpawnEnemies, maxSpawnEnemies));
            yield return new WaitForSeconds(secondsBetweenWaves);
        }
    }

    private void Spawn(int amount)
    {
        for (int i = 0; i < amount; i++) SpawnOne();
    }

    private void SpawnOne()
    {
        if (enemyCount >= maxTotalEnemies) return;
        int sp = Random.Range(0, spawnPoints.Length - 1);
        GameObject go = Instantiate(enemyPrefab, spawnPoints[sp].transform.position, Quaternion.identity);
        IncreaseEnemyCount();
    }

    public void IncreaseEnemyCount() { enemyCount++; }
    public void DecreaseEnemyCount() { enemyCount--; }
}
