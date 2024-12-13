using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }
    private GameObject currentOpenPanel;

    [SerializeField] private List<GameObject> players; // �÷��̾� ������Ʈ ����Ʈ ����
    [SerializeField] private List<GameObject> enemies; // ���ʹ� ������Ʈ ����Ʈ ����

    private PlayerSkill playerSkill; // PlayerSkill �ν��Ͻ� ����

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
        playerSkill = FindObjectOfType<PlayerSkill>(); // PlayerSkill �ν��Ͻ� ã��
    }

    public bool OpenPanel(GameObject panel)
    {
        if (currentOpenPanel != null && currentOpenPanel != panel)
        {
            Debug.Log("�ٸ� �г��� �̹� ���� �־� �� �г��� �� �� �����ϴ�.");
            return false;
        }

        if (currentOpenPanel == panel)
        {
            Debug.Log("���� �г��� �̹� ���� �ֽ��ϴ�.");
            return false;
        }

        currentOpenPanel = panel;
        panel.SetActive(true);
        panel.transform.SetAsLastSibling();
        Debug.Log($"{panel.name} �г��� ���Ƚ��ϴ�.");

        DisableEntities(); // �г��� ���� �� ��� ���� ��Ȱ��ȭ
        Time.timeScale = 0f;

        if (playerSkill != null)
        {
            playerSkill.SetButtonInteractable(false); // �г��� ���� �� ��ų ��ư ��Ȱ��ȭ
        }

        return true;
    }

    public void ClosePanel()
    {
        if (currentOpenPanel != null)
        {
            currentOpenPanel.SetActive(false);
            Debug.Log($"{currentOpenPanel.name} �г��� �������ϴ�.");
            currentOpenPanel = null;

            EnableEntities(); // �г��� ���� �� ��� ���� Ȱ��ȭ
            Time.timeScale = 1f;

            if (playerSkill != null)
            {
                playerSkill.SetButtonInteractable(true); // �г��� ���� �� ��ų ��ư Ȱ��ȭ
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
        SetActiveForEntities(players, false); // ��� �÷��̾� ���� ��Ȱ��ȭ
        SetActiveForEntities(enemies, false); // ��� �� ���� ��Ȱ��ȭ
    }

    private void EnableEntities()
    {
        SetActiveForEntities(players, true); // ��� �÷��̾� ���� Ȱ��ȭ
        SetActiveForEntities(enemies, true); // ��� �� ���� Ȱ��ȭ
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
