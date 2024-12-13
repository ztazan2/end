using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelController : MonoBehaviour
{
    public GameObject Panel_item; // ���� ���� �г�

    void Start()
    {
        // Start �Լ� ������ �� ���·� �Ӵϴ�.
    }

    public void TogglePanel()
    {
        if (Panel_item != null)
        {
            bool isActive = !Panel_item.activeSelf;

            if (isActive)
            {
                // PanelManager�� ���� �г��� ����, �ٸ� �г��� �ݽ��ϴ�.
                PanelManager.Instance.OpenPanel(Panel_item);
                Time.timeScale = 0; // ���� ����
            }
            else
            {
                // PanelManager�� ���� �г��� �ݽ��ϴ�.
                PanelManager.Instance.ClosePanel();
                Time.timeScale = 1; // ���� �簳
            }
        }
    }
}
