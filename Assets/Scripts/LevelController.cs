﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class LevelController : MonoBehaviour
    {

        [System.Serializable]
        public class ColorToPrefab
        {
            public Color32 Color;
            public GameObject Prefab;
        }

        public string levelFileName;
        public ColorToPrefab[] colorToPrefabs;

        Color32 transparent = new Color32(0, 0, 0, 0);
        Color32 white = new Color32(255, 255, 255, 255);

        void Start()
        {
            LoadMap();
        }

        void Update()
        {
            if (Input.GetAxis("ReloadScene") > 0f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
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
                    SpawnTileAt(allPixels[(y * width) + x], x - width / 2, y - height / 2);
                }
            }
        }

        void SpawnTileAt(Color32 c, int x, int y)
        {
            // If this is a white or transparent pixel, then it's meant to just be empty.
            if (c.Equals(white) || c.Equals(transparent))
            {
                return;
            }

            // Find the right color in our map
            foreach (ColorToPrefab ctp in colorToPrefabs)
            {
                // NOTE: This isn't optimized. You should have a dictionary lookup for max speed
                if (c.Equals(ctp.Color))
                {
                    // Spawn the prefab at the right location
                    GameObject go = (GameObject)Instantiate(ctp.Prefab, new Vector3(x, y, 0), Quaternion.identity);
                    if (!go.CompareTag("Player"))
                    {
                        go.transform.SetParent(this.transform);
                        go.name = string.Concat(ctp.Prefab.name, " (", x, ", ", y, ")");
                    }
                    return;
                }
            }

            // If we got to this point, it means we did not find a matching color in our array.
            Debug.LogError(string.Concat("No color to prefab found for: ", c, " at: ", " (", x, ", ", y, ")"));
        }
    }
}