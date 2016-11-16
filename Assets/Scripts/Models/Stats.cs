using UnityEngine;

namespace Assets.Scripts.Models
{
    [System.Serializable]
    public class Stats : ScriptableObject
    {
        public int Health;
        public float Speed;
        public int AttackDamage;
        public float PickupRadius;
        public float Scale;

        public void SetStats(int hp, float spd, int atk, float pickup, float scale)
        {
            Health = hp;
            Speed = spd;
            AttackDamage = atk;
            PickupRadius = pickup;
            Scale = scale;
        }
    }
}