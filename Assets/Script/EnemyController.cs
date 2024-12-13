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
    public List<string> attackPriorityTags; // �켱 Ÿ�� �±� ����Ʈ

    private Transform target;
    private float lastAttackTime;
    private bool knockbackApplied = false;
    private bool isDisabled = false; // �̵� �� ���� ��Ȱ��ȭ ����

    private ResourceManager resourceManager;

    void Start()
    {
        currentHealth = maxHealth;
        resourceManager = FindObjectOfType<ResourceManager>();
        UpdateHealthText();
    }

    void Update()
    {
        if (isDisabled) return; // ��Ȱ��ȭ ���¿����� Update ������ �������� ����

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
            // �˹� ������ x�� ���� �������θ� ����
            ApplyKnockback(new Vector2(1, 0), knockbackForce);
            knockbackApplied = true;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        Debug.Log("�� ü��: " + currentHealth);
        UpdateHealthText();
    }

    private void Die()
    {
        Debug.Log("�� ���");
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
            Debug.LogWarning("healthText�� ������� �ʾҽ��ϴ�.");
        }
    }

    void FindClosestTarget()
    {
        target = null;
        float closestDistance = Mathf.Infinity;

        // �켱���� Ÿ�� Ž��
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
        Debug.Log("���� ������ �����մϴ�: " + targetObject.name);

        PlayerController playerController = targetObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            playerController.TakeDamage(attackDamage);
        }

        // ���� ������Ʈ�� ������ ��
        base_player playerBase = targetObject.GetComponent<base_player>();
        if (playerBase != null)
        {
            playerBase.TakeDamage((int)attackDamage);
        }
    }

    // �˹��� x�� ���� �������θ� ����
    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackForce)
    {
        Vector2 fixedKnockbackDirection = new Vector2(1, 0).normalized; // x�� ���� �������� ����
        transform.position += (Vector3)(fixedKnockbackDirection * knockbackForce * Time.deltaTime);
    }

    // ���� �ð� ���� �̵��� ������ ��Ȱ��ȭ�ϴ� �޼���
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
        isDisabled = false; // ��Ȱ��ȭ ���� ����
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
