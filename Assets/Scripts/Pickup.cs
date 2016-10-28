using UnityEngine;

namespace Assets.Scripts
{
    public class Pickup : MonoBehaviour
    {
        public enum PickupType { Health, Mana, Gold };

        public PickupType pickupType;

        public int Magnitude;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();

                if (pickupType == PickupType.Health)
                {
                    playerController.HealPlayer(Magnitude);
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
}