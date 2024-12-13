using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePanelController : MonoBehaviour
{
    public GameObject Panel_esc; // ���� ���� ESC �г�
    public GameObject lobbyPanel; // �κ� �г� ����
    public Text timeText; // �ð��� ǥ���� Text ������Ʈ
    private float elapsedTime; // ��� �ð� ����
    private bool isPanelOpen = false; // �г� ���� Ȯ�� ����

    private void Update()
    {
        if (!isPanelOpen) // �г��� ���� ���� ���� �ð� ������Ʈ
        {
            elapsedTime += Time.unscaledDeltaTime;

            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(elapsedTime / 60);
                int seconds = Mathf.FloorToInt(elapsedTime % 60);
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }
    }

    public void TogglePanel()
    {
        if (Panel_esc != null)
        {
            isPanelOpen = !Panel_esc.activeSelf;

            if (isPanelOpen)
            {
                PanelManager.Instance.OpenPanel(Panel_esc);
                Time.timeScale = 0; // ���� ����
            }
            else
            {
                PanelManager.Instance.ClosePanel();
                Time.timeScale = 1; // ���� �簳
            }
        }
    }

    // B ��ư Ŭ�� �� ȣ��Ǵ� �޼���
    public void OnBButtonClicked()
    {
        if (Panel_esc != null && lobbyPanel != null)
        {
            // ESC �г��� �ݰ� �κ� �г��� ���ϴ�
            PanelManager.Instance.ClosePanel();
            PanelManager.Instance.OpenPanel(lobbyPanel);
            isPanelOpen = false; // �г� ���� ������Ʈ
        }
    }
}
