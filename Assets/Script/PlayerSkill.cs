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
        // 패널이 열려 있을 때 버튼을 비활성화
        if (PanelManager.Instance != null && PanelManager.Instance.IsPanelOpen())
        {
            skillButton.interactable = false;
            cooldownOverlay.fillAmount = 1; // 패널이 열렸을 때 쿨다운 오버레이를 최대치로 설정
            skillButton.image.color = Color.gray; // 비활성화 시 버튼 색상 변경
            return;
        }

        // 패널이 닫혀 있을 때만 쿨타임 체크
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
            Debug.Log("스킬 사용!");
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
        Debug.Log("쿨타임 무시 활성화!");
    }

    // 쿨타임이 완료되었는지 확인하는 메서드
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
