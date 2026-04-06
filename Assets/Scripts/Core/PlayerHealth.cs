using UnityEngine;
using static GameManager;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [Header("Player Identity")]
    [SerializeField] private PlayerID playerID;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
        else
            Debug.LogWarning($"{name}: HealthBar not assigned");
    }

    void IDamageable.TakeDamage(float damage)
    {
        ApplyDamage(damage);
    }

    private void ApplyDamage(float damage)
    {
        if (damage <= 0 || currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{playerID} died");

        GameManager.Instance.OnPlayerDied(playerID);
    }
}