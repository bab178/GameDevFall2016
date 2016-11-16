using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        private Slider healthBar;
        private PlayerController Player;
        private float maxHealth;

        public Color32 BarColor;

        void Start()
        {
            // NOTE: For this script to work it must be a child of a PlayerController
            healthBar = transform.GetComponentInChildren<Slider>();
            healthBar.fillRect.GetComponent<Image>().color = BarColor;

            Player = transform.GetComponentInParent<PlayerController>();
            maxHealth = Player.PlayerStats.Health;
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        void Update()
        {
            UpdateHealth();
        }

        void UpdateHealth()
        {
            healthBar.value = Player.PlayerStats.Health;
        }
    }
}