using UnityEngine;

public class LightningController : MonoBehaviour
{
    public float damageRadius = 5f; // �������� �� �ݰ�
    public int damageAmount = 50; // ������ �� ������ ��
    public float duration = 0.5f; // ���� ȿ���� ������ �ð�
    public string enemyTag = "enemy"; // �� ������ �±�

    private void Start()
    {
        // ������ ��ȯ�Ǹ� ��� ���� �� ���鿡�� ������ ����
        ApplyDamageToEnemies();
        // ���� �ð��� ���� �� ���� ������Ʈ �ı�
        Destroy(gameObject, duration);
    }

    private void ApplyDamageToEnemies()
    {
        // ��� �� ������ ã��
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);

            // ���� ���� ���� �ִ� �� ���ָ� ������ ����
            if (distance <= damageRadius)
            {
                EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(damageAmount);
                    Debug.Log($"{enemyObject.name}�� {damageAmount}�� ���� �������� ��");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �����Ϳ��� ���� ���� �ð�ȭ�� ���� Gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
