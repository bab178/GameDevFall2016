using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    public int damage;

    public enum DamageType { Burning, KnockBack, DeathTrap }

    public DamageType damageType;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            // Burning damage still needs to take damage OVER TIME
            // Still needs to damage over time precisely
            if ( damageType == DamageType.Burning )
                playerController.PlayerStats.Health -= damage;

            // Knock back damage needs to damage ONCE and knock player back a pixel
            // Still needs a knockback
            if ( damageType == DamageType.KnockBack )
                playerController.PlayerStats.Health -= damage;

            // Death trap immediately kills player
            if (damageType == DamageType.DeathTrap)
                playerController.PlayerStats.Health = 0;
        }
    }
}