using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Rigidbody2D rb;
    public Stats PlayerStats;

    private Player player;

    [System.Serializable]
    public class Player
    {
        public Stats stats;

        public Player(Stats setStats)
        {
            // Set stats to default values if they go below 0
            if(setStats.Health <= 0)
            {
                setStats.Health = 10;
            }

            if(setStats.Speed <= 0)
            {
                setStats.Speed = 5;
            }

            stats = setStats;
        }
    }

    [System.Serializable]
    public class Stats
    {
        public int Health;
        public int Speed;
    }

	// Use this for initialization
	void Start () {
        player = new Player(PlayerStats);
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

