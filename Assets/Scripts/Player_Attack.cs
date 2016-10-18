using GameDevFall2016.Scripts.InventoryManagement;
using System.Linq;
using System.Collections;
using UnityEngine;


public class Player_Attack : MonoBehaviour {

    public GameObject Player;
    PlayerController User;
    float playerForward=0.0f;// used to find the direction the player is facing in.



    // Use this for initialization
    void Start ()
    {
        
    }

    
	
	// Update is called once per frame
	void Update ()
    {
        User =Player.GetComponent<PlayerController>();
        playerForward=User.getPlayerForward();
        PlayerAttack();
    }

    void PlayerAttack()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), User.player.stats.AttackRadius);
            foreach (var item in hitColliders.Where(i => i.tag != "Player")) // all game objects besides player
            {
                if (item.tag == "Enemy")
                {
                    Debug.Log("Pressed Q");

                    Vector2 enemyPosition = item.transform.position; // current position of enemy in the circle 
                    Vector2 playerPosition = this.transform.position;
                    float enemyAngle = Mathf.Atan((Mathf.Abs(playerPosition.y - enemyPosition.y))/ Mathf.Abs((playerPosition.x - enemyPosition.y))) * Mathf.Rad2Deg;// The angle of the enemy in reffrence to the player.
                    float attackThreshold = 30.0f;// The size of the player attack cone
                    float leftBound = playerForward + attackThreshold;// The left bound infront of the player
                    float rightBound = playerForward - attackThreshold;// The right bound infront of the player

                    Debug.Log("Enemy Angle: " + enemyAngle + ", Left Bound: " + leftBound + ", Right Bound: " + rightBound);

                    if (enemyAngle <= leftBound && enemyAngle >= rightBound)
                    {
                        Debug.Log("Enemy Here");
                    }
                }
            }

        }
    }




}
      /*      Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), player.stats.PickupRadius);
            foreach(var item in hitColliders.Where(i => i.tag != "Player")) // all game objects besides player
            {
                if(item.tag == "InventoryItem")
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
        */