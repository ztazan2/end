using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public int maxResource = 1000;
    public int currentResource = 0;
    public int resourceRecoveryRate = 100;
    public float recoveryInterval = 3f;

    public Text resourceText;

    public event Action OnResourceChanged; // �ڿ� ���� �̺�Ʈ

    [System.Serializable]
    public class UnitResourceReward
    {
        public GameObject unitPrefab;
        public int resourceReward;
    }

    public List<UnitResourceReward> unitResourceRewards = new List<UnitResourceReward>(5);

    private void Start()
    {
        InvokeRepeating(nameof(RecoverResource), recoveryInterval, recoveryInterval);
        UpdateResourceUI();
    }

    private void RecoverResource()
    {
        currentResource = Mathf.Min(currentResource + resourceRecoveryRate, maxResource);
        UpdateResourceUI();
    }

    public void UpdateResourceUI()
    {
        resourceText.text = $"{currentResource} / {maxResource}";
        OnResourceChanged?.Invoke(); // �ڿ� ���� �� �̺�Ʈ ȣ��
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        foreach (var unitReward in unitResourceRewards)
        {
            string enemyName = enemy.name.Replace("(Clone)", "").Trim();

            if (enemy.CompareTag("enemy") && enemyName == unitReward.unitPrefab.name)
            {
                AddResource(unitReward.resourceReward);
                Debug.Log($"{enemy.name} óġ�� {unitReward.resourceReward} �ڿ� ȹ��!");
                break;
            }
        }
    }

    private void AddResource(int amount)
    {
        currentResource = Mathf.Min(currentResource + amount, maxResource);
        UpdateResourceUI();
    }
}
