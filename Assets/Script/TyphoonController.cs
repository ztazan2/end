using UnityEngine;
using System.Collections.Generic;

public class TyphoonController : MonoBehaviour
{
    public float speed; // 프리팹에서 설정된 속도 사용
    public float knockbackForce; // 프리팹에서 설정된 넉백 힘 사용
    public float knockbackRadius; // 넉백 반경
    public string enemyTag = "enemy"; // 적 태그
    private Vector3 targetPosition; // 목표 위치

    private HashSet<GameObject> knockedBackEnemies = new HashSet<GameObject>(); // 이미 넉백된 적을 추적하는 Set

    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;

        // 태풍의 Z 위치를 플레이어 유닛보다 뒤로 설정
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
                    Debug.Log($"{enemyObject.name}에 우측으로 넉백 효과 적용");
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
