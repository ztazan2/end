using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSummonManager : MonoBehaviour
{
    public ResourceManager resourceManager; // ResourceManager ����
    public Text resourceText; // ���� �ڿ� ���� �ؽ�Ʈ

    [System.Serializable]
    public class UnitButton
    {
        public Button summonButton; // ���� ��ȯ ��ư
        public Text costText; // ��ư�� ǥ�õ� �ڿ� ��� �ؽ�Ʈ
        public int summonCost; // ���� ��ȯ ���
        public GameObject unitPrefab; // ��ȯ�� ���� ������
        public float cooldownTime; // ���� ��ȯ ��Ÿ�� (�� ����)
        public Image cooldownOverlay; // ��Ÿ�� ǥ�ÿ� �������� �̹���
        [HideInInspector] public float lastSummonTime; // ������ ��ȯ �ð�
        [HideInInspector] public bool isFirstSummon = true; // ù ��ȯ ����
    }

    public List<UnitButton> unitButtons = new List<UnitButton>(5); // 5���� ���� ��ȯ ��ư ����
    private Vector3 spawnPosition = new Vector3(-6f, -1.4f, 0f); // ���� ��ȯ ��ġ ����
    private bool isCooldownBypassed = false; // ��Ÿ�� ���� ���� ����

    private void Start()
    {
        foreach (UnitButton unitButton in unitButtons)
        {
            unitButton.cooldownOverlay.fillAmount = 1; // �ʱ� ���¿��� ��Ÿ�� �������̰� ���� �� ����
        }
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";
        bool isPanelOpen = PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen();

        foreach (UnitButton unitButton in unitButtons)
        {
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || isCooldownBypassed || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            unitButton.summonButton.interactable = !isPanelOpen && canSummon;
            unitButton.summonButton.image.color = unitButton.summonButton.interactable ? Color.yellow : Color.gray;
            unitButton.costText.text = $"{unitButton.summonCost} GOLD";

            if (!canSummon && !unitButton.isFirstSummon && !isCooldownBypassed)
            {
                float cooldownProgress = (Time.time - unitButton.lastSummonTime) / unitButton.cooldownTime;
                unitButton.cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownProgress);
            }
            else
            {
                unitButton.cooldownOverlay.fillAmount = 1;
            }
        }
    }

    public void SummonUnit(int index)
    {
        if (index >= 0 && index < unitButtons.Count)
        {
            UnitButton unitButton = unitButtons[index];
            bool canSummon = resourceManager.currentResource >= unitButton.summonCost &&
                             (unitButton.isFirstSummon || isCooldownBypassed || Time.time >= unitButton.lastSummonTime + unitButton.cooldownTime);

            if (canSummon)
            {
                resourceManager.currentResource -= unitButton.summonCost;
                unitButton.lastSummonTime = Time.time;
                unitButton.isFirstSummon = false;
                unitButton.cooldownOverlay.fillAmount = 0;
                Debug.Log($"{unitButton.summonButton.name} ���� ��ȯ!");
                UpdateUI();

                if (unitButton.unitPrefab != null)
                {
                    Instantiate(unitButton.unitPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // ��Ÿ���� ���� �ð� ���� �����ϵ��� �����ϴ� �޼���
    public IEnumerator BypassAllCooldowns(float duration)
    {
        isCooldownBypassed = true; // ��Ÿ�� ���� Ȱ��ȭ
        yield return new WaitForSeconds(duration); // duration ���� ��Ÿ�� ���� ����
        isCooldownBypassed = false; // ��Ÿ�� ���� ��Ȱ��ȭ
        Debug.Log("���� ��ȯ ��Ÿ�� ���� ȿ�� ����.");
    }
}
