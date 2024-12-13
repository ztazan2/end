using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public GameObject[] panels; // 제어할 패널 배열로 참조
    private Button resumeButton; // Resume 버튼의 Button 컴포넌트 참조

    void Start()
    {
        resumeButton = GetComponent<Button>();
        UpdateResumeButtonState(); // 초기 상태 설정
    }

    void Update()
    {
        UpdateResumeButtonState(); // 매 프레임마다 상태 업데이트
    }

    private void UpdateResumeButtonState()
    {
        // panels 배열에 포함된 패널 중 하나가 열려 있으면 Resume 버튼 활성화
        bool shouldEnableButton = false;

        foreach (GameObject panel in panels)
        {
            if (PanelManager.Instance != null && PanelManager.Instance.GetCurrentOpenPanel() == panel)
            {
                shouldEnableButton = true;
                break;
            }
        }

        // Resume 버튼의 활성화 상태 업데이트
        if (resumeButton != null)
        {
            resumeButton.interactable = shouldEnableButton;
        }
    }

    public void PauseGame()
    {
        // 게임을 일시 정지시키고, 패널을 열지 않음
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        // 현재 열려 있는 패널이 panels 배열에 포함된 경우에만 게임 재개 및 패널 닫기
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
            // 할당된 패널이 열려 있는 경우에만 모든 할당된 패널 닫기 및 게임 재개
            foreach (GameObject panel in panels)
            {
                PanelManager.Instance.ClosePanel();
            }
            Time.timeScale = 1; // 게임을 다시 시작
        }
        else
        {
            Debug.Log("할당되지 않은 패널이 열려 있어 게임이 재개되지 않습니다.");
        }
    }

    public void OnResumeButtonClick()
    {
        // Resume 버튼 클릭 시 할당된 패널을 모두 닫고 게임을 재개
        ResumeGame();
    }
}
