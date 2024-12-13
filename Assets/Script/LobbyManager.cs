using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public GameObject lobbyPanel;
    private PanelManager panelManager; 
    public static bool isRestarting = false; // �� ����� ���θ� ��Ÿ���� ����

    void Start()
    {
        panelManager = PanelManager.Instance;

        // ���� ���� �� ó���� �κ� �г��� Ȱ��ȭ
        if (!isRestarting)
        {
            ShowLobby();
        }
        else
        {
            isRestarting = false; // ����� �Ŀ��� �κ� �г��� �ٽ� ������ �ʵ��� ����
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
        // �κ� �ݰ� ����� ���¸� �����Ͽ� �κ� �ٽ� ������ �ʵ��� ����
        HideLobby();
        isRestarting = true; // ����� ���·� ����
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� ���� �ٽ� �ε�
    }

    // ���ο� ExitGame �޼��� �߰�
    public void ExitGame()
    {
        Application.Quit();
    }
}
