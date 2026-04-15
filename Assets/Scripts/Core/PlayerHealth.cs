using UnityEngine;
using static GameManager;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;
    public NetworkVariable<float> currentHealth = new NetworkVariable<float>(100f);

    [Header("Player Identity")]
    [SerializeField] private PlayerID playerID;

    [Header("UI")]
    [SerializeField] private HealthBar healthBar;

    void Start()
    {
        if (!GameSettings.isNetworkMultiplayer)
        {
            currentHealth.Value = maxHealth;
        }

        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
        else
            Debug.LogWarning($"{name}: HealthBar not assigned");
    }


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            currentHealth.Value = maxHealth;
        }
        currentHealth.OnValueChanged += (oldValue, newValue) =>
        {
            if (healthBar != null)
                healthBar.SetHealth(newValue);
        };
    }

    void IDamageable.TakeDamage(float damage)
    {

        if (GameSettings.isNetworkMultiplayer && !IsServer) return;
        ApplyDamage(damage);
    }

    private void ApplyDamage(float damage)
    {
        if (damage <= 0 || currentHealth.Value <= 0) return;

        currentHealth.Value -= damage;
        currentHealth.Value = Mathf.Clamp(currentHealth.Value, 0, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth.Value);

        if (currentHealth.Value <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{playerID} died");

        GameManager.Instance.OnPlayerDied(playerID);
    }
}