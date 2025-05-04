using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float regenPerSecond = 5f;

    [Header("UI References")]
    public Image healthFillImage; // Asigna aquí el Image con FillAmount
    public Animator damageAnimator; // Asigna aquí el Animator (opcional)

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void Update()
    {
        RegenerateHealth();
    }

    void RegenerateHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += regenPerSecond * Time.deltaTime;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateHealthUI();
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);
        UpdateHealthUI();

        if (damageAnimator != null)
        {
            damageAnimator.SetTrigger("DamageFlash"); // Este trigger debe existir en el Animator
        }
    }

    void UpdateHealthUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}
