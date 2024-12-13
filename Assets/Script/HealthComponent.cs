using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public float maxHealth = 100f; // �ִ� ü��
    protected float currentHealth; // ���� ü��

    // �ܺο��� currentHealth�� ���� �� �ִ� ������Ƽ
    public float CurrentHealth
    {
        get { return currentHealth; }
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth; // �ʱ� ü���� �ִ� ü������ ����
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " ü��: " + currentHealth); // ü�� ���� �޽��� ���

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " ���"); // ��� �޽��� ���
        Destroy(gameObject);
    }
}
