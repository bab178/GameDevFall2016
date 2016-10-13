using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public UnityEngine.UI.Slider healthBar;
    public PlayerController Player;

    float maxHealth;

	// Use this for initialization
	void Start ()
    {
        maxHealth = Player.PlayerStats.Health;
        healthBar.value = maxHealth;
        healthBar.minValue = 0;
        healthBar.maxValue = maxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateHealth();
	}

    void UpdateHealth ()
    {
        healthBar.value = Player.PlayerStats.Health;
    }
}
