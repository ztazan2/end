using UnityEngine;

public class LightningController : MonoBehaviour
{
    public float damageRadius = 5f; // 데미지를 줄 반경
    public int damageAmount = 50; // 적에게 줄 데미지 양
    public float duration = 0.5f; // 번개 효과가 유지될 시간
    public string enemyTag = "enemy"; // 적 유닛의 태그

    private void Start()
    {
        // 번개가 소환되면 즉시 범위 내 적들에게 데미지 적용
        ApplyDamageToEnemies();
        // 일정 시간이 지난 후 번개 오브젝트 파괴
        Destroy(gameObject, duration);
    }

    private void ApplyDamageToEnemies()
    {
        // 모든 적 유닛을 찾기
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemyObject.transform.position);

            // 일정 범위 내에 있는 적 유닛만 데미지 적용
            if (distance <= damageRadius)
            {
                EnemyController enemyController = enemyObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(damageAmount);
                    Debug.Log($"{enemyObject.name}에 {damageAmount}의 번개 데미지를 줌");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 에디터에서 번개 범위 시각화를 위한 Gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
