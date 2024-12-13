using UnityEngine;

public class CannonPrefab : MonoBehaviour
{
    public float damageRadius; // ���� �� �������� �� �ݰ�
    public int damageAmount; // ������ �� ������ ��
    public float knockbackForce; // �˹� �� ����
    public string enemyTag; // �� �±�
    public float explodeDelay; // ���� ���� �ð�

    private void Start()
    {
        Invoke(nameof(Explode), explodeDelay); // explodeDelay �� �� ����
    }

    public void Explode()
    {
        // ���� �±׸� ���� ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // ������ ��ȸ�ϸ� �������� �˹� ����
        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= damageRadius) // ���� �ݰ� ���� ��
            {
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount); // ������ ����
                    Debug.Log($"{enemyObject.name}�� {damageAmount}�� �������� ��"); // �������� �־��� �� �޽��� ���

                    // �˹� ������ x�� ���� �������θ� ����
                    Vector2 knockbackDirection = new Vector2(1, 0).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce); // �˹� ����
                    Debug.Log($"{enemyObject.name}�� x�� ���� �������� {knockbackForce}�� �˹��� ����");
                }
            }
        }

        Destroy(gameObject); // ���� ������Ʈ �ı�
    }

    private void OnDrawGizmosSelected()
    {
        // �����Ϳ��� Gizmo�� ���� ���� �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
