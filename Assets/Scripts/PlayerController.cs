using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Player player;
    public Rigidbody2D rb;
    public int CustomHealth;
    public int CustomSpeed;


    [System.Serializable]
    public class Player
    {
        public Stats stats;

        // Default Contructor
        public Player(int hp, int spd)
        {
            stats = new Stats(hp, spd);
        }
    }

    [System.Serializable]
    public class Stats
    {
        public int Health;
        public int Speed;

        // Default Contructor
        public Stats()
        {
            Health = 10;
            Speed = 5;
        }

        public Stats(int hp, int spd)
        {
            // Validate hp and spd
            if(hp > 0 && spd > 0)
            {
                Health = hp;
                Speed = spd;
            }
            else
            {
                Health = 10;
                Speed = 5;
            }
        }
    }

	// Use this for initialization
	void Start () {
        player = new Player(CustomHealth, CustomSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorzontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorzontal * player.stats.Speed, moveVertical * player.stats.Speed);
        rb.velocity = movement;
    }
}

