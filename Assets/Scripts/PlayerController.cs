using Assets.Scripts.Models;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public Stats PlayerStats;
        [HideInInspector]
        public Inventory PlayerInventory;

        public bool AutoEquipIfEmpty;

        private Rigidbody2D rb;
        private int originalHealth;
        private float originalSpeed;

        private Canvas inventoryWindow;
        private GridLayoutGroup inventoryGridLayout;
        private bool inventoryWindowActive;
        private Canvas equipWindow;
        private bool equipWindowActive;
        private GameObject buttonPrefab;
        private float dmgCooldown = 0.15f;
        private float noDieTimer;

        private DamageNumbers dmgNumbers;

        void Start()
        {
            PlayerStats = ScriptableObject.CreateInstance<Stats>();
            PlayerStats.SetStats(100, 5f, 5, 0.5f, 1.5f);
            gameObject.transform.localScale = new Vector3(PlayerStats.Scale, PlayerStats.Scale, 1f);

            originalHealth = PlayerStats.Health;
            originalSpeed = PlayerStats.Speed;

            PlayerInventory = new Inventory();
            rb = gameObject.GetComponent<Rigidbody2D>();

            noDieTimer = dmgCooldown;

            inventoryWindow = transform.FindChild("InventoryWindow").GetComponent<Canvas>();
            inventoryGridLayout = inventoryWindow.GetComponentInChildren<GridLayoutGroup>();
            inventoryWindowActive = false;
            inventoryWindow.gameObject.SetActive(inventoryWindowActive);

            equipWindow = transform.FindChild("EquipmentWindow").GetComponent<Canvas>();
            equipWindowActive = false;
            equipWindow.gameObject.SetActive(equipWindowActive);

            buttonPrefab = Resources.Load<GameObject>("InventoryItemButton");

            AutoEquipIfEmpty = true;
            dmgNumbers = new DamageNumbers();
        }

        public void HealPlayer(int healthRestored)
        {
            if (PlayerStats.Health + healthRestored >= originalHealth)
            {
                PlayerStats.Health = originalHealth;
            }
            else
            {
                PlayerStats.Health += healthRestored;
            }
        }

        public void TakeDamage(int damageTaken)
        {
            noDieTimer -= Time.deltaTime;

            // TODO: Flicker for no die?

            if (noDieTimer <= 0f)
            {
                PlayerStats.Health -= damageTaken;
                noDieTimer = dmgCooldown;

                dmgNumbers.DrawDamage(damageTaken);

                if (PlayerStats.Health <= 0)
                {
                    // TODO: Die, Respawn, Game Over...?

                    gameObject.SetActive(false);
                    Debug.Log("Player Died!");
                }
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
                PlayerStats.Speed = originalSpeed * 2;

            // Stop sprinting
            if (Input.GetButtonUp("Sprint"))
                PlayerStats.Speed = originalSpeed;

            // Move player
            Vector2 movement = new Vector2(moveHorizontal * PlayerStats.Speed, moveVertical * PlayerStats.Speed);
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
            if (Input.GetKeyDown(KeyCode.G))
            {
                TakeDamage(1);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), PlayerStats.PickupRadius);
                foreach (Collider2D item in hitColliders.Where(i => i.tag != "Player")) // all game objects besides player
                {
                    if (item.tag == "InventoryItem")
                    {
                        ManageInventoryItems(item);   
                    }
                    else if (item.tag == "EquipmentItem")
                    {
                        ManageEquipmentItems(item);
                    }
                    else if (item.tag == "LockedDoor")
                    {
                        ManageLockedDoors(item);
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
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventoryWindowActive = !inventoryWindowActive;
                inventoryWindow.gameObject.SetActive(inventoryWindowActive);
            }

            // Toggle Equipment Window
            if (Input.GetKeyDown(KeyCode.P))
            {
                equipWindowActive = !equipWindowActive;
                equipWindow.gameObject.SetActive(equipWindowActive);
            }
        }

        void ManageInventoryItems(Collider2D item, bool shouldTryStack = true)
        {
            InventoryItem invItem = item.GetComponent<InventoryItem>();
            if (invItem.Id == 1) invItem.Quantity = Random.Range(1, 10); // Sets random quantity in range

            bool containsId = PlayerInventory.ContainsItemWithId(invItem.Id);

            if (!PlayerInventory.IsFull || containsId)   
            {
                bool wasStacked = PlayerInventory.AddItemToInventory(invItem, shouldTryStack);

                if (!wasStacked)
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
                Debug.Log("My pockets are too full... I only have " + PlayerInventory.MaxItemCount + " spots.");
            }
        }

        void ManageEquipmentItems(Collider2D item)
        {
            var itemInfo = item.GetComponent<InventoryItem>();

            if (itemInfo == null)
            {
                Debug.LogError("An EquipmentItem could not be parsed out of this item: " + item.name);
                return;
            }

            if (itemInfo.EquipType == EquipType.None)
            {
                Debug.LogError("An EquipmentItem has a type of None, cannot be equipped");
                return;
            }

            if(PlayerInventory.IsEquipmentSlotEmpty(itemInfo.EquipType) && AutoEquipIfEmpty)
            {
                // Auto equip item to slot
                PlayerInventory.EquipItemAtSlot(PlayerInventory, itemInfo);

                var parentPanel = equipWindow.transform.FindChild("Panel");

                GameObject child = null;

                // Update equipment GUI
                switch(itemInfo.EquipType)
                {
                    case EquipType.Head:
                        child = parentPanel.FindChild("Head").gameObject;
                        break;
                    case EquipType.Top:
                        child = parentPanel.FindChild("Top").gameObject;
                        break;
                    case EquipType.Bottom:
                        child = parentPanel.FindChild("Bottom").gameObject;
                        break;
                    case EquipType.Hands:
                        child = parentPanel.FindChild("Hands").gameObject;
                        break;
                    case EquipType.Feet:
                        child = parentPanel.FindChild("Feet").gameObject;
                        break;
                }

                // set image
                var img = child.transform.FindChild("Button").gameObject.GetComponent<Image>();
                img.sprite = itemInfo.Sprite;

                item.GetComponent<SpriteRenderer>().sprite = null;
                Destroy(item);
            }
            else
            {
                // Send to Inventory
                ManageInventoryItems(item, false);
            }
        }

        void ManageLockedDoors(Collider2D item)
        {
            var keyId = 2;
            PlayerInventory.RemoveItem(keyId, 1);

            // Get inventoryItem on button
            InventoryItem btnItemScript = inventoryGridLayout
                .gameObject.transform.GetComponentsInChildren<InventoryItem>()
                .FirstOrDefault(i => i.Id == keyId);

            // destroy key
            Destroy(btnItemScript.gameObject);

            // remove sprite
            item.GetComponent<SpriteRenderer>().sprite = null;

            // destroy door
            Destroy(item);

            Debug.Log("Door unlocked!!!");
        }
    }
}