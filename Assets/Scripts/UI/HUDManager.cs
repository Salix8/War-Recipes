using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("Barra de vida")]
    public Slider healthBar;

    [Header("Cooldowns")]
    public Image rollCooldownImage;
    public Image attack1CooldownImage;
    public Image attack2CooldownImage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateHealth(float current, int max)
    {
        if (healthBar != null)
            healthBar.value = (float)current / max;
    }

    public void UpdateRollCooldown(float cooldownPercent)
    {
        if (rollCooldownImage != null)
            rollCooldownImage.fillAmount = cooldownPercent;
    }

    public void UpdateAttack1Cooldown(float cooldownPercent)
    {
        if (attack1CooldownImage != null)
            attack1CooldownImage.fillAmount = cooldownPercent;
    }

    public void UpdateAttack2Cooldown(float cooldownPercent)
    {
        if (attack2CooldownImage != null)
            attack2CooldownImage.fillAmount = cooldownPercent;
    }
}
