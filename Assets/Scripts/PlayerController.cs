using GameDevFall2016.Scripts.InventoryManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Stats PlayerStats;
    public Inventory PlayerInventory;
    public Player player;

    private Rigidbody2D rb;
    private float originalSpeed;
    private float playerForward = 0.0f; // used to find the direction the player is facing in.

    private Canvas inventoryWindow;
    private GridLayoutGroup inventoryGridLayout;
    private bool inventoryWindowActive;
    private GameObject buttonPrefab;

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
        public float Speed;
        public float AttackRadius
        {
            get { return 1.0f; } // returns one value
            protected set { } // prevents AttackRadius from being overwritten
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        player = new Player(PlayerStats, PlayerInventory);
        originalSpeed = player.stats.Speed;

        inventoryWindow = transform.FindChild("InventoryWindow").GetComponent<Canvas>();
        inventoryGridLayout = inventoryWindow.GetComponentInChildren<GridLayoutGroup>();
        inventoryWindowActive = false;
        inventoryWindow.gameObject.SetActive(inventoryWindowActive);
        buttonPrefab = Resources.Load<GameObject>("InventoryItemButton");

    }

    public void TakeDamage(int damageTaken)
    {
        player.stats.Health = -damageTaken;

        // TODO: Damage numbers

        if (player.stats.Health <= 0)
        {
            // TODO: Die, Respawn, Game Over...?

            gameObject.SetActive(false);
            Debug.Log("Player Died!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();
        InteractWithWorld();

        //InteractWithMenu();

    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Sprint!
        if (Input.GetButtonDown("Sprint"))
            player.stats.Speed = originalSpeed * 2;

        // Stop sprinting
        if (Input.GetButtonUp("Sprint"))
            player.stats.Speed = originalSpeed;

        // Move player
        Vector2 movement = new Vector2(moveHorizontal * player.stats.Speed, moveVertical * player.stats.Speed);
        rb.velocity = movement;

        // Rotate player in direction they're moving
        if (movement != Vector2.zero)
        {
            playerForward = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(playerForward, Vector3.forward);
        }
    }


    // Pickup items, pull levers, press buttons, etc
    void InteractWithWorld()
    {

        if (Input.GetKey(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), player.stats.PickupRadius);
            foreach (var item in hitColliders.Where(i => i.tag != "Player")) // all game objects besides player
            {
                if (item.tag == "InventoryItem")
                {
                    // Create InventoryItem from GameObject
                    InventoryItem newItem = item.GetComponent<InventoryItem>();
                    newItem.Sprite = item.GetComponent<SpriteRenderer>().sprite;
                    newItem.Name = item.name;
                    newItem.Quantity = 1;
                    newItem.FlavorText = "Some kind of " + item.name;

                    if (player.inventory.AddItemToInventory(newItem))
                    {
                        // Add to / display in Inventory
                        GameObject newItemBtn = (GameObject)Instantiate(buttonPrefab, inventoryGridLayout.transform, false);

                        var btnItem = newItemBtn.GetComponent<InventoryItem>();
                        var btnSprite = newItemBtn.GetComponent<Image>();
                        btnSprite.sprite = newItem.Sprite;
                        Text btnText = newItemBtn.transform.GetChild(0).GetComponent<Text>();
                        btnText.text = newItem.Quantity.ToString();

                        // Remove item from scene
                        Destroy(item.gameObject);
                        Debug.Log("+" + newItem.Quantity + " " + newItem.Name);
                    }
                    else
                    {
                        // Inventory is full
                        Debug.Log("My pockets are too full... I only have " + player.inventory.MaxItemCount + " spots.");
                    }
                }
                else
                {
                    // Cannot pick this thing up
                    Debug.Log("There's an interesting " + item.name + " next to me. I don't think this will fit in my pockets.");
                }
            }
        }
    }


    public float getPlayerForward() // returns the player forward
    {
        return playerForward;
    }


    // Open/close menus
    void InteractWithMenu()
    {
        // Toggle Inventory Window
        
        {
            inventoryWindowActive = !inventoryWindowActive;
            inventoryWindow.gameObject.SetActive(inventoryWindowActive);
        }
    }

}
