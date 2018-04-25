using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParasiteLevelController : MonoBehaviour {

    [SerializeField] private int maxEnemies;
    private int enemiesSpawned;
    [SerializeField] private GameObject spawnEffect;
    private GameObject spawn;
    [SerializeField] private GameObject enemyPrefab;
    private GameObject enemy;
    [SerializeField] private float spawnDelay;
    private float spawnDelayTimer = 0f;
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    private float spawnTime;
    private float spawnTimer = 0f;
    [SerializeField] private float spawnDestroyTime;
    private float spawnDestroyTimer = 0f;

    private void Start()
    {
        spawnTime = GetSpawnTime();
    }

    private void Update()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemiesSpawned >= maxEnemies) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer < spawnTime) return;

        bool validPos = false;
        Vector3 spawnPosition = Vector3.zero;

        while (!validPos)
        {
            spawnPosition = new Vector3(Random.Range(0, 30), 0, Random.Range(0, 30));
            if (Physics.CheckSphere(spawnPosition, 2, LayerMask.NameToLayer("Unwalkable")))
            {
                validPos = true;
            }
        }

        if (spawn == null)
        {
            spawn = Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
        }

        spawnDelayTimer += Time.deltaTime;
        if (spawnDelayTimer < spawnDelay) return;
        
        if (enemy == null)
        {
            enemy = Instantiate(enemyPrefab, spawn.transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
        }

        spawnDestroyTimer += Time.deltaTime;
        if (spawnDestroyTimer < spawnDestroyTime) return;

        spawnDelayTimer = 0f;
        spawnTimer = 0f;
        spawnDestroyTimer = 0f;

        Destroy(spawn);
        enemy = null;

        enemiesSpawned++;
        spawnTime = GetSpawnTime();
    }

    private float GetSpawnTime()
    {
        return Random.Range(minSpawnTime, maxSpawnTime);
    }
}
