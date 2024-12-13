using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class base_enemy : MonoBehaviour
{
    public int maxHealth; // ������ ��ü��
    private int currentHealth; // ���� ü��
    public Text healthText; // ü���� ǥ���� �ؽ�Ʈ UI

    private EndGamePanelController gameManager; // GameManager ����

    void Start()
    {
        // ü���� �ʱ�ȭ�ϰ� �ؽ�Ʈ UI�� ������Ʈ�մϴ�.
        currentHealth = maxHealth;
        UpdateHealthText();

        // GameManager ã��
        gameManager = FindObjectOfType<EndGamePanelController>();
    }

    // �÷��̾��� �������� ���� ������ ���ظ� ���� �� ȣ���� �޼���
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // ü���� 0 �̸����� �������� �ʵ��� ����
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            DestroyBase();
        }

        Debug.Log("�� ���� ü�� ����: " + currentHealth);

        // ü�� �ؽ�Ʈ UI ������Ʈ
        UpdateHealthText();
    }

    // ü�� �ؽ�Ʈ UI�� ������Ʈ�ϴ� �޼���
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }

    // ������ �ı��Ǿ��� �� ������ ���� (��: �ı� �ִϸ��̼�, ���� ���� ó�� ��)
    private void DestroyBase()
    {
        Debug.Log("�� ������ �ı��Ǿ����ϴ�!");

        // GameManager�� ���� ���� ���� ó��
        if (gameManager != null)
        {
            gameManager.EndGame(false); // �� ���� �ı� �� false ����
        }

        Destroy(gameObject); // ������ �ı��Ͽ� ���ӿ��� ����
    }
}
