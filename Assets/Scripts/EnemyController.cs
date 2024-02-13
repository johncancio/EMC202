using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;
    public string enemyColor;

    public void SetTarget(Transform target, float moveSpeed)
    {
        this.target = target;
        this.moveSpeed = moveSpeed;

        if (target != null)
        {
            transform.LookAt(target.position);
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
