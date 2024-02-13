using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Material[] playerColors;
    public Renderer playerRenderer;
    public Renderer bulletRenderer;
    public Renderer childRenderer;
    public TextMeshProUGUI gameOverText;
    public GameObject[] enemies;

    [Header("PlayerStats")]
    public float fireRate = 1f;
    public float bulletSpeed = 1f;
    public float detectionDistance = 30f;
    public float rotationSpeed = 2.5f;
    private float nextFireTime;

    private int currentColorIndex = 0;
    private bool isGameOver = false;

    private void Start()
    {
        nextFireTime = Time.time;

        SetColor(currentColorIndex);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (Time.time > nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }

        if (enemies.Length > 0)
        {
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                Vector3 directionToEnemy = nearestEnemy.transform.position - transform.position;
                directionToEnemy.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGameObject = collision.gameObject;
        if ((!isGameOver) && (otherGameObject.CompareTag("Enemy")))
        {
            Destroy(gameObject);
            Destroy(otherGameObject);

            isGameOver = true;
            gameOverText.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= detectionDistance && distanceToEnemy < nearestDistance)
            {
                nearestEnemy = enemy;
                nearestDistance = distanceToEnemy;
            }
        }

        return nearestEnemy;
    }

    private void OnMouseDown()
    {
        currentColorIndex = (currentColorIndex + 1) % playerColors.Length;
        SetColor(currentColorIndex);
    }

    void SetColor(int colorIndex)
    {
        playerRenderer.material = playerColors[colorIndex];
        if (bulletRenderer != null)
        {
            bulletRenderer.material = playerColors[colorIndex];
        }

        SyncMaterial();
    }

    void SyncMaterial()
    {
        Material playerMaterial = playerRenderer.material;

        childRenderer.material = playerMaterial;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletrb = bullet.GetComponent<Rigidbody>();
        bulletrb.velocity = firePoint.forward * bulletSpeed;

        Renderer bulletRenderer = bullet.GetComponent<Renderer>();
        if (bulletRenderer != null)
        {
            Color bulletColor = bulletRenderer.material.color;
            string colorName = bulletColor.ToString();
            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.bulletColor = colorName;
            }
        }
    }
}
