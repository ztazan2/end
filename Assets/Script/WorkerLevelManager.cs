using UnityEngine;
using UnityEngine.UI;

public class WorkerLevelManager : MonoBehaviour
{
    public int currentLevel = 1; // ���� ����
    public int maxLevel = 8; // �ִ� ����

    // ResourceManager�� ����
    private ResourceManager resourceManager;

    // UI ���
    public Text levelText; // ���� ���� �� ���� ǥ�ÿ� �ؽ�Ʈ
    public Text resourceText; // ���� �ڿ� ���� �� ������ �ʿ� �ڿ��� ǥ�ÿ� �ؽ�Ʈ
    public Button levelUpButton; // ������ ��ư
    private Color defaultButtonColor = Color.gray; // �⺻ ��ư ������ ȸ������ ����

    private readonly int[] levelUpRequirements = { 180, 360, 540, 720, 900, 1080, 1260 };

    private void Start()
    {
        // ResourceManager ������Ʈ�� ã�� ���� ����
        resourceManager = FindObjectOfType<ResourceManager>();

        // ��ư�� �ʱ� ���� ����
        levelUpButton.image.color = defaultButtonColor;

        UpdateUI(); // �ʱ� UI ������Ʈ
    }

    private void Update()
    {
        UpdateUI(); // UI ������Ʈ
    }

    public void LevelUp()
    {
        if (CanLevelUp())
        {
            // �ڿ� ����
            resourceManager.currentResource -= levelUpRequirements[currentLevel - 1];
            currentLevel++;

            // ���������� ���� �ڿ� ȸ�� �ӵ��� �ִ� �ڿ��� ����
            resourceManager.resourceRecoveryRate += 20;
            resourceManager.maxResource += 500;

            // UI ������Ʈ ȣ��
            UpdateUI();
            Debug.Log($"���� {currentLevel}�� ���! ȸ�� �ӵ�: {resourceManager.resourceRecoveryRate}, �ִ� �ڿ���: {resourceManager.maxResource}");
        }
    }

    private bool CanLevelUp()
    {
        return currentLevel < maxLevel && resourceManager.currentResource >= levelUpRequirements[currentLevel - 1];
    }

    private void UpdateUI()
    {
        // ���� ������ ������ ���� ǥ��
        if (currentLevel < maxLevel)
        {
            levelText.text = $"Lv. {currentLevel + 1}\nLEVEL\nUP!\n{levelUpRequirements[currentLevel - 1]} GOLD";
        }
        else
        {
            levelText.text = "MAX LEVEL";
        }

        // ���� �ڿ� ���� ǥ��
        resourceText.text = $"{resourceManager.currentResource} / {resourceManager.maxResource}";

        // �г��� ���� �ִ� ��� ��ư ��Ȱ��ȭ
        if (PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen())
        {
            levelUpButton.interactable = false;
            levelUpButton.image.color = defaultButtonColor;
        }
        else
        {
            // �г��� ���� ���� ���� ������ ���� ���ο� ���� ��ư ����� Ȱ��ȭ ���� ������Ʈ
            if (CanLevelUp())
            {
                levelUpButton.interactable = true;
                levelUpButton.image.color = Color.yellow;
            }
            else
            {
                levelUpButton.interactable = false;
                levelUpButton.image.color = defaultButtonColor; // �⺻ ���� ȸ������ ����
            }
        }
    }
}
