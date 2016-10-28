using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerAttack : MonoBehaviour
    {

        private bool attacking;
        private float attackTimer;
        private Collider2D attackingTrigger;
        private Slider AttackSilder;
        private PlayerController Player;

        public float attackCooldown;
        public Color32 BarColor;

        void Start()
        {
            Player = GetComponentInParent<PlayerController>();

            attacking = false;
            attackTimer = 0;

            attackingTrigger = GetComponent<BoxCollider2D>();
            attackingTrigger.enabled = false;

            AttackSilder = transform.GetComponentInChildren<Slider>();
            AttackSilder.fillRect.GetComponent<Image>().color = BarColor;
            AttackSilder.minValue = 0f;
            AttackSilder.maxValue = attackCooldown;
        }

        void Update()
        {
            // Update slider
            AttackSilder.value = attackTimer;

            if (Input.GetKey(KeyCode.Q) && !attacking)
            {
                attacking = true;
                attackTimer = attackCooldown;

                attackingTrigger.enabled = true;
            }

            if (attacking)
            {
                if (attackTimer > 0f)
                {
                    attackTimer -= Time.deltaTime;
                }
                else
                {
                    attacking = false;
                    attackingTrigger.enabled = false;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isTrigger && other.CompareTag("Enemy"))
            {
                other.SendMessageUpwards("TakeDamage", Player.PlayerStats.AttackDamage);
            }
        }
    }
}