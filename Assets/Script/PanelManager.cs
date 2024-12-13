using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }
    private GameObject currentOpenPanel;

    [SerializeField] private List<GameObject> players; // 플레이어 오브젝트 리스트 참조
    [SerializeField] private List<GameObject> enemies; // 에너미 오브젝트 리스트 참조

    private PlayerSkill playerSkill; // PlayerSkill 인스턴스 참조

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerSkill = FindObjectOfType<PlayerSkill>(); // PlayerSkill 인스턴스 찾기
    }

    public bool OpenPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            Debug.Log("다른 패널이 이미 열려 있어 이 패널을 열 수 없습니다.");
            return false;
        }

        if (currentOpenPanel == panel)
        {
            Debug.Log("같은 패널이 이미 열려 있습니다.");
            return false;
        }

        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
        Debug.Log($"{panel.name} 패널이 열렸습니다.");

        DisableEntities(); // 패널이 열릴 때 모든 유닛 비활성화
        Time.timeScale = 0f;

        if (playerSkill != null)
        {
            playerSkill.SetButtonInteractable(false); // 패널이 열릴 때 스킬 버튼 비활성화
        }

        return true;
    }

    public void ClosePanel()
    {
        if (currentOpenPanel != null)
        {
            currentOpenPanel.SetActive(false);
            Debug.Log($"{currentOpenPanel.name} 패널이 닫혔습니다.");
            currentOpenPanel = null;

            EnableEntities(); // 패널이 닫힐 때 모든 유닛 활성화
            Time.timeScale = 1f;

            if (playerSkill != null)
            {
                playerSkill.SetButtonInteractable(true); // 패널이 닫힐 때 스킬 버튼 활성화
            }
        }
    }

    public bool IsPanelOpen()
    {
        return currentOpenPanel != null;
    }

    public GameObject GetCurrentOpenPanel()
    {
        return currentOpenPanel;
    }

    private void DisableEntities()
    {
        SetActiveForEntities(players, false); // 모든 플레이어 유닛 비활성화
        SetActiveForEntities(enemies, false); // 모든 적 유닛 비활성화
    }

    private void EnableEntities()
    {
        SetActiveForEntities(players, true); // 모든 플레이어 유닛 활성화
        SetActiveForEntities(enemies, true); // 모든 적 유닛 활성화
    }

    private void SetActiveForEntities(List<GameObject> entities, bool isActive)
    {
        foreach (var entity in entities)
        {
            if (entity != null)
            {
                entity.SetActive(isActive);
            }
        }
    }
}
