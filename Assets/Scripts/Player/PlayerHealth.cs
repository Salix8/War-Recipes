using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats Reference")]
    public PlayerStats playerStats; // ← ScriptableObject compartido

    [Header("UI References")]
    public Image healthFillImage;
    public Animator damageAnimator;

    private void Start()
    {
        // Asegúrate de que la salud esté bien inicializada (solo si quieres reiniciar)
        if (playerStats.currentHealth <= 0f)
            playerStats.currentHealth = playerStats.maxHealth;

        UpdateHealthUI();
    }

    private void Update()
    {
        RegenerateHealth();
    }

    void RegenerateHealth()
    {
        if (playerStats.currentHealth < playerStats.maxHealth)
        {
            playerStats.currentHealth += playerStats.regenPerSecond * Time.deltaTime;
            playerStats.currentHealth = Mathf.Min(playerStats.currentHealth, playerStats.maxHealth);
            UpdateHealthUI();
        }
    }

    public void TakeDamage(float damage)
    {
        playerStats.currentHealth -= damage;
        playerStats.currentHealth = Mathf.Max(0f, playerStats.currentHealth);
        UpdateHealthUI();

        if (damageAnimator != null)
        {
            damageAnimator.ResetTrigger("DamageFlash"); // ← Permite repetir animación
            damageAnimator.SetTrigger("DamageFlash");
        }
    }

    void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = playerStats.currentHealth / playerStats.maxHealth;
        }
    }
}
