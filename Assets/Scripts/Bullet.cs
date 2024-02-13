using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public string bulletColor;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGameObject = collision.gameObject;

        EnemyController enemyController = otherGameObject.GetComponent<EnemyController>();
        
        if (otherGameObject.CompareTag("Enemy"))
        {
            if (bulletColor == enemyController.enemyColor)
            {
                Destroy(gameObject);
                Destroy(otherGameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
