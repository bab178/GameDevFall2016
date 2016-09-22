using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public GameObject Spawner;

    // Tile to be spawned
    public GameObject PrimaryTile;
    public GameObject SecondaryTile;

    [Range(0,100)]
    public float PercentPrimary;

    private int worldWidth  = 21;
    private int worldHeight = 21;

    // Use this for initialization
    void Start()
    {
        
        CreateWorld();
    }

    private void CreateWorld()
    {
        for (int x = 0; x < worldWidth; x++)
        {
            for (int z = 0; z < worldHeight; z++)
            {
                var selectedTile = (Random.Range(0.0f, 1.0f) < PercentPrimary / 100.0f) ? PrimaryTile : SecondaryTile;
                // Create your tile object.
                GameObject tileInstance = Instantiate(selectedTile);

                // Set spawn as parent
                tileInstance.transform.parent = Spawner.transform;

                // Set scale and position
                selectedTile.transform.localScale = new Vector3(1.6f, 1.6f, 1.0f);
                tileInstance.transform.position = new Vector3(x - worldWidth/2, z - worldHeight/2, 0);
            }
        }
    }
}
