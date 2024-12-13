using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public Button skillButton;
    public Image cooldownOverlay;
    public float cooldownTime;
    private float lastSkillTime = -Mathf.Infinity;
    private bool isCooldown = false;
    private bool isCooldownBypassed = false;

    public GameObject cannonPrefab;
    public Vector3 cannonSpawnPosition;

    private void Start()
    {
        skillButton.onClick.AddListener(UseSkill);
        UpdateButtonAndOverlay();
    }

    private void Update()
    {
        // �г��� ���� ���� �� ��ư�� ��Ȱ��ȭ
        if (PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen())
        {
            skillButton.interactable = false;
            cooldownOverlay.fillAmount = 1; // �г��� ������ �� ��ٿ� �������̸� �ִ�ġ�� ����
            skillButton.image.color = Color.gray; // ��Ȱ��ȭ �� ��ư ���� ����
            return;
        }

        // �г��� ���� ���� ���� ��Ÿ�� üũ
        if (isCooldown && !isCooldownBypassed)
        {
            float elapsedCooldown = Time.time - lastSkillTime;
            float cooldownRatio = elapsedCooldown / cooldownTime;
            cooldownOverlay.fillAmount = Mathf.Clamp01(cooldownRatio);

            if (cooldownRatio >= 1f)
            {
                isCooldown = false;
                UpdateButtonAndOverlay();
            }
        }
        else
        {
            UpdateButtonAndOverlay();
        }
    }

    public void UseSkill()
    {
        if (!isCooldown || isCooldownBypassed)
        {
            Debug.Log("��ų ���!");
            lastSkillTime = Time.time;
            isCooldown = true;
            isCooldownBypassed = false;
            UpdateButtonAndOverlay();
            Instantiate(cannonPrefab, cannonSpawnPosition, Quaternion.identity);
        }
    }

    private void UpdateButtonAndOverlay()
    {
        skillButton.interactable = !isCooldown || isCooldownBypassed;
        cooldownOverlay.fillAmount = skillButton.interactable ? 1 : 0;
        skillButton.image.color = skillButton.interactable ? Color.yellow : Color.gray;
    }

    public void BypassCooldown()
    {
        isCooldownBypassed = true;
        UpdateButtonAndOverlay();
        Debug.Log("��Ÿ�� ���� Ȱ��ȭ!");
    }

    // ��Ÿ���� �Ϸ�Ǿ����� Ȯ���ϴ� �޼���
    public bool IsCooldownComplete()
    {
        return !isCooldown || isCooldownBypassed;
    }

    public void SetButtonInteractable(bool isInteractable)
    {
        skillButton.interactable = isInteractable;
        UpdateButtonAndOverlay();
    }
}
