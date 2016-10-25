using GameDevFall2016.Scripts.InventoryManagement;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Stats PlayerStats;
    public Inventory PlayerInventory;

    private Player player;
    private Rigidbody2D rb;
    private float originalSpeed;

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

        if(player.stats.Health <= 0)
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
        InteractWithMenu();
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
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    // Pickup items, pull levers, press buttons, etc
    void InteractWithWorld()
    {
        if(Input.GetKey(KeyCode.E))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), player.stats.PickupRadius);
            foreach(var item in hitColliders.Where(i => i.tag != "Player")) // all game objects besides player
            {
                if(item.tag == "InventoryItem")
                {
                    InventoryItem invItem = item.GetComponent<InventoryItem>();
                    invItem.Quantity = Random.Range(1, 10); // Sets random quantity in range

                    if (!player.inventory.IsFull)
                    {
                        bool wasStacked = player.inventory.AddItemToInventory(invItem);

                        if(!wasStacked)
                        {
                            // Add to / display in Inventory
                            GameObject newItemBtn = (GameObject)Instantiate(buttonPrefab, inventoryGridLayout.transform, false);

                            // Set inventoryItem of button
                            InventoryItem btnItem = newItemBtn.GetComponent<InventoryItem>();
                            btnItem.Id = invItem.Id;
                            btnItem.Sprite = invItem.Sprite;
                            btnItem.Name = invItem.Name;
                            btnItem.Quantity = invItem.Quantity;
                            btnItem.FlavorText = invItem.FlavorText;

                            // Set sprite in inventory
                            Image btnSprite = newItemBtn.GetComponent<Image>();
                            btnSprite.sprite = invItem.Sprite;

                            // Set quantity text in inventory
                            Text btnText = newItemBtn.transform.GetChild(0).GetComponent<Text>();
                            btnText.text = invItem.Quantity.ToString();
                        }
                        else
                        {
                            // Get inventoryItem on button
                            InventoryItem btnItemScript = inventoryGridLayout
                                .gameObject.transform.GetComponentsInChildren<InventoryItem>()
                                .FirstOrDefault(i => i.Id == invItem.Id);

                            if (btnItemScript == null) return;

                            // Stack items
                            btnItemScript.Quantity += invItem.Quantity;

                            // Set quantity text in inventory to addition of quantities
                            Text btnText = btnItemScript.gameObject.transform.GetChild(0).GetComponent<Text>();
                            btnText.text = (btnItemScript.Quantity).ToString();
                        }

                        // Remove item from scene
                        Destroy(item.gameObject);
                        Debug.Log("+" + invItem.Quantity + " " + invItem.Name);
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


    // Open/close menus
    void InteractWithMenu()
    {
        // Toggle Inventory Window
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryWindowActive = !inventoryWindowActive;
            inventoryWindow.gameObject.SetActive(inventoryWindowActive);
        }
    }
}
