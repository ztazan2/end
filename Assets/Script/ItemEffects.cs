using UnityEngine;
using UnityEngine.UI;

public class ItemEffects : MonoBehaviour
{
    public GameObject lightningPrefab;
    public GameObject typhoonPrefab;
    public float disableDuration;
    public float bypassCooldownDuration;
    public PlayerSkill playerSkill;
    public UnitSummonManager unitSummonManager;
    public ResourceManager resourceManager;

    public Text costText;

    [Header("Effect Costs")]
    public int lightningCost;
    public int typhoonCost;
    public int disableEnemiesCost;
    public int bypassCannonCost;
    public int bypassSummonCost;

    [Header("Button Cost Texts")]
    public Text lightningCostText;
    public Text typhoonCostText;
    public Text disableEnemiesCostText;
    public Text bypassCannonCostText;
    public Text bypassSummonCostText;

    public Button lightningButton;
    public Button typhoonButton;
    public Button disableEnemiesButton;
    public Button bypassCannonButton;
    public Button bypassSummonButton;

    private void Start()
    {
        resourceManager.OnResourceChanged += UpdateButtonCostTexts;
        UpdateCostUI();
    }

    private void OnDestroy()
    {
        resourceManager.OnResourceChanged -= UpdateButtonCostTexts;
    }

    public void ActivateItemEffect(int effectId)
    {
        CloseOpenPanel();

        if (!HasSufficientResource(effectId))
        {
            Debug.Log("�ڿ��� �����Ͽ� �������� ����� �� �����ϴ�.");
            return;
        }

        DeductResource(effectId);
        UpdateCostUI();

        switch (effectId)
        {
            case 1:
                SummonLightning();
                break;
            case 2:
                SummonTyphoon();
                break;
            case 3:
                DisableAllEnemies(disableDuration);
                break;
            case 4:
                BypassCannonCooldown();
                break;
            case 5:
                StartCoroutine(unitSummonManager.BypassAllCooldowns(bypassCooldownDuration));
                Debug.Log($"{bypassCooldownDuration}�� ���� ���� ��ȯ ��Ÿ�� ���� ȿ�� Ȱ��ȭ!");
                break;
        }
    }

    bool HasSufficientResource(int effectId)
    {
        int requiredResource = GetEffectCost(effectId);
        return resourceManager.currentResource >= requiredResource;
    }

    void DeductResource(int effectId)
    {
        resourceManager.currentResource -= GetEffectCost(effectId);
        resourceManager.UpdateResourceUI();
    }

    int GetEffectCost(int effectId)
    {
        switch (effectId)
        {
            case 1: return lightningCost;
            case 2: return typhoonCost;
            case 3: return disableEnemiesCost;
            case 4: return bypassCannonCost;
            case 5: return bypassSummonCost;
            default: return 0;
        }
    }

    void SummonLightning()
    {
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        Debug.Log("���� ��ȯ!");
    }

    void SummonTyphoon()
    {
        Vector3 startPosition = new Vector3(-6f, -1.4f, 0);
        Vector3 targetPosition = new Vector3(6f, -1.4f, 0);
        GameObject typhoon = Instantiate(typhoonPrefab, startPosition, Quaternion.identity);
        Debug.Log("��ǳ ��ȯ!");

        TyphoonController typhoonController = typhoon.GetComponent<TyphoonController>();
        if (typhoonController != null)
        {
            typhoonController.Initialize(targetPosition);
        }
    }

    void DisableAllEnemies(float duration)
    {
        Debug.Log("��� �� ���� �̵� �� ���� ��Ȱ��ȭ!");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        foreach (GameObject enemy in enemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.DisableEnemy(duration);
            }
        }
    }

    void BypassCannonCooldown()
    {
        Debug.Log("���� ��ų ��Ÿ�� ���� ȿ�� �ߵ�!");
        playerSkill.BypassCooldown();
    }

    void CloseOpenPanel()
    {
        if (PanelManager.Instance != null)
        {
            PanelManager.Instance.ClosePanel();
            Debug.Log("�����ִ� �г��� �������ϴ�.");
        }
    }

    private void UpdateCostUI()
    {
        if (costText != null)
        {
            costText.text = "Resource: " + resourceManager.currentResource;
        }
        UpdateButtonCostTexts();
    }

    private void UpdateButtonCostTexts()
    {
        SetButtonState(lightningButton, lightningCostText, lightningCost);
        SetButtonState(typhoonButton, typhoonCostText, typhoonCost);
        SetButtonState(disableEnemiesButton, disableEnemiesCostText, disableEnemiesCost);

        // BypassCannonCooldown ��ư�� �ڿ� ���� + ��Ÿ�� ���� ���¿����� Ȱ��ȭ
        bool canUseCannonSkill = resourceManager.currentResource >= bypassCannonCost && !playerSkill.IsCooldownComplete();
        bypassCannonButton.interactable = canUseCannonSkill;
        bypassCannonCostText.text = $"{bypassCannonCost} GOLD";
        bypassCannonCostText.color = canUseCannonSkill ? Color.yellow : Color.gray;

        SetButtonState(bypassSummonButton, bypassSummonCostText, bypassSummonCost);
    }

    private void SetButtonState(Button button, Text costText, int cost)
    {
        bool canAfford = resourceManager.currentResource >= cost;
        button.interactable = canAfford;
        costText.text = $"{cost} GOLD";
        costText.color = canAfford ? Color.yellow : Color.gray;
    }
}
