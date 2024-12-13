using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public GameObject[] panels; // ������ �г� �迭�� ����
    private Button resumeButton; // Resume ��ư�� Button ������Ʈ ����

    void Start()
    {
        resumeButton = GetComponent<Button>();
        UpdateResumeButtonState(); // �ʱ� ���� ����
    }

    void Update()
    {
        UpdateResumeButtonState(); // �� �����Ӹ��� ���� ������Ʈ
    }

    private void UpdateResumeButtonState()
    {
        // panels �迭�� ���Ե� �г� �� �ϳ��� ���� ������ Resume ��ư Ȱ��ȭ
        bool shouldEnableButton = false;

        foreach (GameObject panel in panels)
        {
            if (PanelManager.Instance != null && PanelManager.Instance.GetCurrentOpenPanel() == panel)
            {
                shouldEnableButton = true;
                break;
            }
        }

        // Resume ��ư�� Ȱ��ȭ ���� ������Ʈ
        if (resumeButton != null)
        {
            resumeButton.interactable = shouldEnableButton;
        }
    }

    public void PauseGame()
    {
        // ������ �Ͻ� ������Ű��, �г��� ���� ����
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        // ���� ���� �ִ� �г��� panels �迭�� ���Ե� ��쿡�� ���� �簳 �� �г� �ݱ�
        bool hasAssignedPanelOpen = false;

        if (PanelManager.Instance != null)
        {
            GameObject currentOpenPanel = PanelManager.Instance.GetCurrentOpenPanel();

            foreach (GameObject panel in panels)
            {
                if (panel == currentOpenPanel)
                {
                    hasAssignedPanelOpen = true;
                    break;
                }
            }
        }

        if (hasAssignedPanelOpen)
        {
            // �Ҵ�� �г��� ���� �ִ� ��쿡�� ��� �Ҵ�� �г� �ݱ� �� ���� �簳
            foreach (GameObject panel in panels)
            {
                PanelManager.Instance.ClosePanel();
            }
            Time.timeScale = 1; // ������ �ٽ� ����
        }
        else
        {
            Debug.Log("�Ҵ���� ���� �г��� ���� �־� ������ �簳���� �ʽ��ϴ�.");
        }
    }

    public void OnResumeButtonClick()
    {
        // Resume ��ư Ŭ�� �� �Ҵ�� �г��� ��� �ݰ� ������ �簳
        ResumeGame();
    }
}
