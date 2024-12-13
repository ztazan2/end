using UnityEngine;

public class CannonPrefab : MonoBehaviour
{
    public float damageRadius; // 폭발 시 데미지를 줄 반경
    public int damageAmount; // 적에게 줄 데미지 양
    public float knockbackForce; // 넉백 힘 설정
    public string enemyTag; // 적 태그
    public float explodeDelay; // 폭발 지연 시간

    private void Start()
    {
        Invoke(nameof(Explode), explodeDelay); // explodeDelay 초 후 폭발
    }

    public void Explode()
    {
        // 적을 태그를 통해 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // 적들을 순회하며 데미지와 넉백 적용
        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);
            if (distance <= damageRadius) // 폭발 반경 내의 적
            {
                EnemyController enemy = enemyObject.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damageAmount); // 데미지 적용
                    Debug.Log($"{enemyObject.name}에 {damageAmount}의 데미지를 줌"); // 데미지를 주었을 때 메시지 출력

                    // 넉백 방향을 x축 우측 방향으로만 설정
                    Vector2 knockbackDirection = new Vector2(1, 0).normalized;
                    enemy.ApplyKnockback(knockbackDirection, knockbackForce); // 넉백 적용
                    Debug.Log($"{enemyObject.name}에 x축 우측 방향으로 {knockbackForce}의 넉백을 적용");
                }
            }
        }

        Destroy(gameObject); // 대포 오브젝트 파괴
    }

    private void OnDrawGizmosSelected()
    {
        // 에디터에서 Gizmo로 폭발 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
