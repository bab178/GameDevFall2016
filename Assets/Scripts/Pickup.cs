using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public enum PickupType { Health, Mana, Gold };

    public PickupType pickupType;

    public int Magnitude;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

            if (pickupType == PickupType.Health)
            {
                playerController.PlayerStats.Health += Magnitude;
                Destroy(gameObject);
            }
            else if (pickupType == PickupType.Gold)
            {

            }
            else if (pickupType == PickupType.Mana)
            {

            }
        }
    }
}
