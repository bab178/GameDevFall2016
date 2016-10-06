using GameDevFall2016.Scripts.InventoryManagement;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Stats PlayerStats;
    public Inventory PlayerInventory;

    private Player player;
    private GameObject playerGO;

    [System.Serializable]
    public class Player
    {
        public Stats stats;
        public Inventory inventory;

        public Player(Stats setStats, Inventory setInventory)
        {
            // Set stats to default values if they go below 0
            if (setStats.Health <= 0)
            {
                setStats.Health = 10;
            }

            if (setStats.Speed <= 0)
            {
                setStats.Speed = 5;
            }

            inventory = setInventory;
            stats = setStats;
        }
    }

    [System.Serializable]
    public class Stats
    {
        public float PickupRadius
        {
            get { return 0.5f; } // returns one value
            protected set { } // prevents PickupRadius from being overwritten
        }

        public int Health;
        public int Speed;
    }

    // Use this for initialization
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag("Player");
        player = new Player(PlayerStats, PlayerInventory);
        player.inventory.AddItemToInventory("Apple", 1);
    }

    public void TakeDamage(int damageTaken)
    {
        player.stats.Health = -damageTaken;
        if(player.stats.Health <= 0)
        {
            // Die, Respawn, Game Over...? You choose
            Debug.Log("Player Died!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        InteractWithWorld();
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Move player
        Vector2 movement = new Vector2(moveHorizontal * player.stats.Speed, moveVertical * player.stats.Speed);
        rb.velocity = movement;

        // Rotate player in direction they're moving
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void InteractWithWorld()
    {
        if(Input.GetKey(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(playerGO.transform.position.x, playerGO.transform.position.y), player.stats.PickupRadius);
            foreach(var item in hitColliders.Where(i => i.name != "Player")) // all game objects besides player
            {
                if(item.tag == "PickupItem")
                {
                    player.inventory.AddItemToInventory(item.name, 1);
                    Destroy(item.gameObject);

                    Debug.Log("I picked up one: " + item.name);
                }
                else
                {
                    Debug.Log("I found an interesting " + item.name + ".");
                }
            }
        }
    }
}
