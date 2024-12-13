using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 

public class PlayerController : MonoBehaviour
{
    public float attackDamage; // ���� ������
    public float attackRange; // ���� ��Ÿ�
    public float moveSpeed; // �̵� �ӵ�
    public float attackCooldown; // ���� ��� �ð�
    public float maxHealth; // �ִ� ü��
    private float currentHealth; // ���� ü��
    public float knockbackForce; // �˹� ��
    public Text healthText; // ü���� ǥ���� Text ������Ʈ

    [Header("Targeting Settings")]
    public List<string> attackPriorityTags; // ���� �켱���� �±� ����Ʈ

    private Transform target; // ���� ���� ����� Transform
    private float lastAttackTime; // ������ ���� �ð��� ����
    private bool knockbackApplied = false; // �˹� �� ���� ����ǵ��� ����

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthText();
    }

    void Update()
    {
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
            ApplyKnockback();
            knockbackApplied = true; // �˹��� �� ���� ����ǵ��� ����
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("�÷��̾� ü��: " + currentHealth);
        UpdateHealthText();
    }

    private void Die()
    {
        Debug.Log("�÷��̾� ���");
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
            Debug.LogWarning("healthText�� ������� �ʾҽ��ϴ�.");
        }
    }

    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // �켱������ ���� ��� Ž��
        foreach (string priorityTag in attackPriorityTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(priorityTag);
            foreach (GameObject enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy.transform;
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
        Debug.Log("�÷��̾ ������ �����մϴ�: " + targetObject.name);

        base_enemy baseEnemy = targetObject.GetComponent<base_enemy>();
        if (baseEnemy != null)
        {
            baseEnemy.TakeDamage((int)attackDamage);
            return;
        }

        EnemyController enemyController = targetObject.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.TakeDamage(attackDamage);
        }
    }

    void ApplyKnockback()
    {
        // �˹� ������ x�� ���� �������� ����
        Vector2 knockbackDirection = Vector2.left;
        transform.position += (Vector3)(knockbackDirection * knockbackForce * Time.deltaTime);
        Debug.Log("���� �������� �˹� �����");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
