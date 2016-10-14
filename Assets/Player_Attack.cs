using UnityEngine;
using System.Collections;

public class Player_Attack : MonoBehaviour {


    Vector2 pointB;
    Vector2 pointA;

    // Use this for initialization
    void Start ()
    {
        Vector2 playerPosition = GameObject.Find("player").transform.position;
    }

    
	
	// Update is called once per frame
	void Update ()
    {
        playerAttack();
	}

    void playerAttack()
    {

        pointA.x = GameObject.Find("player").transform.position.x + 0.55f;
        pointA.y = GameObject.Find("player").transform.position.y - 0.16f;

        pointB.x= GameObject.Find("player").transform.position.x + 0.2369485f;
        pointB.y= GameObject.Find("player").transform.position.y + 0.16f;

        Collider2D[] hitObjects = Physics2D.OverlapAreaAll(pointA, pointB);
        foreach(var objects in hitObjects.Where(i=>i.tag !="Player"))
        {
            if(objects.tag=="Enemy")
            {
                if(Input.GetKey(KeyCode.Q))
                {
                    Debug.Log("Gotcha!!!");

                    objects.GetComponent.Enemy_dmg();
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