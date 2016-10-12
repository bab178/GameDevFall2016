using UnityEngine;
public class LevelController : MonoBehaviour {

    [System.Serializable]
    public class ColorToPrefab
    {
        public Color32 Color;
        public GameObject Prefab;
    }

    public string levelFileName;
    public ColorToPrefab[] colorToPrefabs;

    Color32 white = new Color32(255, 255, 255, 255);

    void Start()
    {
        LoadMap();
    }

    void EmptyMap()
    {
        while (transform.childCount > 0)
        {
            Transform c = transform.GetChild(0);
            c.SetParent(null); // become Batman
            Destroy(c.gameObject); // become The Joker
        }
    }

    void LoadMap()
    {
        EmptyMap();

        // Read the image data from the file in StreamingAssets
        string filePath = Application.dataPath + "/StreamingAssets/" + levelFileName;
        byte[] bytes = System.IO.File.ReadAllBytes(filePath);
        Texture2D levelMap = new Texture2D(2, 2);
        levelMap.LoadImage(bytes);

        // Get the raw pixels from the level imagemap
        Color32[] allPixels = levelMap.GetPixels32();
        int width = levelMap.width;
        int height = levelMap.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnTileAt(allPixels[(y * width) + x], x-width / 2, y - height / 2);
            }
        }
    }

    void SpawnTileAt(Color32 c, int x, int y)
    {

        // If this is a transparent pixel, then it's meant to just be empty.
        if (c.Equals(white))
        {
            return;
        }

        // Find the right color in our map

        // NOTE: This isn't optimized. You should have a dictionary lookup for max speed
        foreach (ColorToPrefab ctp in colorToPrefabs)
        {
            if (c.Equals(ctp.Color))
            {
                // Spawn the prefab at the right location
                GameObject go = (GameObject)Instantiate(ctp.Prefab, new Vector3(x, y, 0), Quaternion.identity);
                go.transform.SetParent(this.transform);
                go.name = "Tile" + x + "-" + y; 
                // maybe do more stuff to the gameobject here?
                return;
            }
        }

        // If we got to this point, it means we did not find a matching color in our array.

        Debug.LogError("No color to prefab found for: " + c.ToString());

    }
}
