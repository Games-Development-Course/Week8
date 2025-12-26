using UnityEngine;
using UnityEngine.UI;

public class HealthHUD : MonoBehaviour
{
    public Slider healthSlider;
    public PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealth;
            UpdateHealth(playerHealth.currentHealth, playerHealth.maxHealth);
        }
    }

    void UpdateHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
        if (healthSlider.value < healthSlider.maxValue * 0.3f)
            healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        else
            healthSlider.fillRect.GetComponent<Image>().color = Color.green;
    }

    void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateHealth;
    }
}
