using UnityEngine;
using System.Collections.Generic;

public class TyphoonController : MonoBehaviour
{
    public float speed; // �����տ��� ������ �ӵ� ���
    public float knockbackForce; // �����տ��� ������ �˹� �� ���
    public float knockbackRadius; // �˹� �ݰ�
    public string enemyTag = "enemy"; // �� �±�
    private Vector3 targetPosition; // ��ǥ ��ġ

    private HashSet<GameObject> knockedBackEnemies = new HashSet<GameObject>(); // �̹� �˹�� ���� �����ϴ� Set

    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;

        // ��ǳ�� Z ��ġ�� �÷��̾� ���ֺ��� �ڷ� ����
        transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            Destroy(gameObject);
        }

        ApplyKnockbackInRadius();
    }

    private void ApplyKnockbackInRadius()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemyObject in enemies)
        {
            if (knockedBackEnemies.Contains(enemyObject)) continue;

            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= knockbackRadius)
            {
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    Vector2 knockbackDirection = new Vector2(1, 0).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce);
                    knockedBackEnemies.Add(enemyObject);
                    Debug.Log($"{enemyObject.name}�� �������� �˹� ȿ�� ����");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
