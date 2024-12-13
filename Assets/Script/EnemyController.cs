using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    public float attackDamage;
    public float attackRange;
    public float moveSpeed;
    public float attackCooldown;
    public float maxHealth;
    private float currentHealth;
    public float knockbackForce;
    public Text healthText;

    [Header("Targeting Settings")]
    public List<string> attackPriorityTags; // 우선 타겟 태그 리스트

    private Transform target;
    private float lastAttackTime;
    private bool knockbackApplied = false;
    private bool isDisabled = false; // 이동 및 공격 비활성화 상태

    private ResourceManager resourceManager;

    void Start()
    {
        currentHealth = maxHealth;
        resourceManager = FindObjectOfType<ResourceManager>();
        UpdateHealthText();
    }

    void Update()
    {
        if (isDisabled) return; // 비활성화 상태에서는 Update 로직을 실행하지 않음

        FindClosestTarget();

        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            if (distanceToTarget > attackRange)
            {
                MoveTowardsTarget();
            }
            else if (Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;
                Attack(target.gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= maxHealth * 0.3f && !knockbackApplied)
        {
            // 넉백 방향을 x축 우측 방향으로만 설정
            ApplyKnockback(new Vector2(1, 0), knockbackForce);
            knockbackApplied = true;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("적 체력: " + currentHealth);
        UpdateHealthText();
    }

    private void Die()
    {
        Debug.Log("적 사망");
        if (resourceManager != null)
        {
            resourceManager.OnEnemyDefeated(gameObject);
        }
        Destroy(gameObject);
    }

    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth.ToString("F0");
        }
        else
        {
            Debug.LogWarning("healthText가 연결되지 않았습니다.");
        }
    }

    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // 우선순위 타겟 탐색
        foreach (string priorityTag in attackPriorityTags)
        {
            GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag(priorityTag);
            foreach (GameObject potentialTarget in potentialTargets)
            {
                float distance = Vector2.Distance(transform.position, potentialTarget.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = potentialTarget.transform;
                }
            }
            if (target != null)
            {
                return;
            }
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void Attack(GameObject targetObject)
    {
        Debug.Log("적이 공격을 수행합니다: " + targetObject.name);

        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }

        // 기지 오브젝트를 공격할 때
        base_player playerBase = targetObject.GetComponent<base_player>();
        if (playerBase != null)
        {
            playerBase.TakeDamage((int)attackDamage);
        }
    }

    // 넉백을 x축 우측 방향으로만 적용
    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        Vector2 fixedKnockbackDirection = new Vector2(1, 0).normalized; // x축 우측 방향으로 고정
        transform.position += (Vector3)(fixedKnockbackDirection * knockbackForce * Time.deltaTime);
    }

    // 일정 시간 동안 이동과 공격을 비활성화하는 메서드
    public void DisableEnemy(float duration)
    {
        if (!isDisabled)
        {
            isDisabled = true;
            StartCoroutine(EnableEnemyAfterDelay(duration));
        }
    }

    private IEnumerator EnableEnemyAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDisabled = false; // 비활성화 상태 해제
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
