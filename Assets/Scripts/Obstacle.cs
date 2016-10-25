using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    public enum ObstacleType { DeathTrap, BurnTrap, KnockbackTrap };

    public ObstacleType obstacleType;

    public int Magnitude;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (obstacleType == ObstacleType.DeathTrap)
            {
                playerController.TakeDamage(1000);
            }
            else if (obstacleType == ObstacleType.BurnTrap)
            {
                playerController.PlayerStats.Health -= Magnitude;
            }
            else if (obstacleType == ObstacleType.KnockbackTrap)
            {

            }
        }
    }
}
