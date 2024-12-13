using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyPanel;
    private PanelManager panelManager; 
    public static bool isRestarting = false; // 씬 재시작 여부를 나타내는 변수

    void Start()
    {
        panelManager = PanelManager.Instance;

        // 게임 시작 시 처음만 로비 패널을 활성화
        if (!isRestarting)
        {
            ShowLobby();
        }
        else
        {
            isRestarting = false; // 재시작 후에는 로비 패널이 다시 열리지 않도록 설정
        }
    }

    public void ShowLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            if (panelManager.OpenPanel(lobbyPanel))
            {

            }
        }
    }

    public void HideLobby()
    {
        if (lobbyPanel != null && panelManager != null)
        {
            PanelManager.Instance.ClosePanel();
        }
    }

    public void StartGame()
    {
        HideLobby();
    }

    public void RestartGame()
    {
        // 로비를 닫고 재시작 상태를 설정하여 로비가 다시 열리지 않도록 설정
        HideLobby();
        isRestarting = true; // 재시작 상태로 설정
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬을 다시 로드
    }

    // 새로운 ExitGame 메서드 추가
    public void ExitGame()
    {
        Application.Quit();
    }
}
