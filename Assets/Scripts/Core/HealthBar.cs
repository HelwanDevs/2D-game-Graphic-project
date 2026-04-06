using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError($"[HealthBar] Slider component missing on {gameObject.name}", this);
            enabled = false;
            return;
        }
        slider.interactable = false;
        slider.wholeNumbers = false;
        slider.minValue = 0f;
    }

    public void SetMaxHealth(float maxHealth)
    {
        if (maxHealth <= 0f)
        {
            Debug.LogError($"[HealthBar] Invalid maxHealth ({maxHealth}) on {gameObject.name}", this);
            return;
        }
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(float currentHealth)
    {
        if (slider.maxValue <= 0f)
        {
            Debug.LogWarning($"[HealthBar] SetHealth called before SetMaxHealth on {gameObject.name}", this);
            return;
        }
        slider.value = Mathf.Clamp(currentHealth, 0f, slider.maxValue);
    }
}