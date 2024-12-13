using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSummonManager : MonoBehaviour
{
    public ResourceManager resourceManager; // ResourceManager 참조
    public Text resourceText; // 현재 자원 상태 텍스트

    [System.Serializable]
    public class UnitButton
    {
        public Button summonButton; // 유닛 소환 버튼
        public Text costText; // 버튼에 표시될 자원 비용 텍스트
        public int summonCost; // 유닛 소환 비용
        public GameObject unitPrefab; // 소환할 유닛 프리팹
        public float cooldownTime; // 유닛 소환 쿨타임 (초 단위)
        public Image cooldownOverlay; // 쿨타임 표시용 오버레이 이미지
        [HideInInspector] public float lastSummonTime; // 마지막 소환 시간
        [HideInInspector] public bool isFirstSummon = true; // 첫 소환 여부
    }

    public List<UnitButton> unitButtons = new List<UnitButton>(5); // 5개의 유닛 소환 버튼 설정
    private Vector3 spawnPosition = new Vector3(-6f, -1.4f, 0f); // 유닛 소환 위치 고정
    private bool isCooldownBypassed = false; // 쿨타임 무시 상태 변수

    private void Start()
    {
        foreach (UnitButton unitButton in unitButtons)
        {
            unitButton.cooldownOverlay.fillAmount = 1; // 초기 상태에서 쿨타임 오버레이가 가득 차 있음
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
                Debug.Log($"{unitButton.summonButton.name} 유닛 소환!");
                UpdateUI();

                if (unitButton.unitPrefab != null)
                {
                    Instantiate(unitButton.unitPrefab, spawnPosition, Quaternion.identity);
                }
            }
        }
    }

    // 쿨타임을 일정 시간 동안 무시하도록 설정하는 메서드
    public IEnumerator BypassAllCooldowns(float duration)
    {
        isCooldownBypassed = true; // 쿨타임 무시 활성화
        yield return new WaitForSeconds(duration); // duration 동안 쿨타임 무시 유지
        isCooldownBypassed = false; // 쿨타임 무시 비활성화
        Debug.Log("유닛 소환 쿨타임 무시 효과 종료.");
    }
}
