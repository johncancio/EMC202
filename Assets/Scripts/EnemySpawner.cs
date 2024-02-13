using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public float spawnInterval = 5f;
    public float spawnRadius = 100f;
    public float minSpawnDistance = 50f;
    public float enemySpeed = 5f;

    public Material[] enemyMaterials;

    private float nextSpawnTime;

    private void Start()
    {
        nextSpawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition;
        float angle;
        float distanceToPlayer;
        do
        {
            angle = Random.Range(0f, Mathf.PI * 2f);
            Vector3 spawnOffset = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * Random.Range(0f, spawnRadius);
            spawnPosition = playerTransform.position + spawnOffset;

            distanceToPlayer = Vector3.Distance(spawnPosition, playerTransform.position);
        } while (distanceToPlayer < minSpawnDistance);

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        Material randomMaterial = enemyMaterials[Random.Range(0, enemyMaterials.Length)];

        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            enemyRenderer.material = randomMaterial;
        }

        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetTarget(playerTransform, enemySpeed);

            Color enemyColor = randomMaterial.color;
            string colorName = enemyColor.ToString();
            enemyController.enemyColor = colorName;
        }
    }
}
