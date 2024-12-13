using UnityEngine;

public class EndGamePanelController : MonoBehaviour
{
    public GameObject endGamePanelA; // �¸� �г�
    public GameObject endGamePanelB; // �й� �г�
    private bool gameEnded = false;
    private PanelManager panelManager;

    void Start()
    {
        panelManager = PanelManager.Instance;
    }

    // ���� ���� �޼���
    public void EndGame(bool isPlayerBaseDestroyed)
    {
        if (!gameEnded)
        {
            gameEnded = true;
            GameObject panelToOpen = isPlayerBaseDestroyed ? endGamePanelB : endGamePanelA;

            // �г��� ���� ������ �Ͻ����� (PanelManager���� ����)
            if (panelToOpen != null && panelManager != null)
            {
                panelManager.OpenPanel(panelToOpen);
            }
        }
    }

    // ���� ����� �޼���
    public void RestartGame()
    {
        gameEnded = false;

        // ��� �г� �ݱ� (PanelManager���� ����)
        if (panelManager != null)
        {
            PanelManager.Instance.ClosePanel();
        }
    }
}
