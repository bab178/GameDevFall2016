using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    public float maxHealth;
    public PlayerController Player;

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
