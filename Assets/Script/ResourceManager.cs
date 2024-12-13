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

    public event Action OnResourceChanged; // 자원 변경 이벤트

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
        OnResourceChanged?.Invoke(); // 자원 변경 시 이벤트 호출
    }

    public void OnEnemyDefeated(GameObject enemy)
    {
        foreach (var unitReward in unitResourceRewards)
        {
            string enemyName = enemy.name.Replace("(Clone)", "").Trim();

            if (enemy.CompareTag("enemy") && enemyName == unitReward.unitPrefab.name)
            {
                AddResource(unitReward.resourceReward);
                Debug.Log($"{enemy.name} 처치로 {unitReward.resourceReward} 자원 획득!");
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
