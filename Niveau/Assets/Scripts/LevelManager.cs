using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 20;
    public int rows = 10;
    public GameObject[] floorTiles;
    public GameObject[] walls;

    private Transform levelHolder;
    private int tileSize = 10;

    public void LevelSetup()
    {
        levelHolder = new GameObject("Level").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiateTile = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instanceTile = Instantiate(toInstantiateTile, new Vector3(x* tileSize, 0f, y* tileSize), Quaternion.identity) as GameObject;
                instanceTile.transform.SetParent(levelHolder);

                if (x == 0 || x == columns - 1)
                {
                    for (int k = (int)(-tileSize * 0.5f); k < (int)(tileSize * 0.5f) + 1; k++)
                    {
                        GameObject instanceWall = Instantiate(walls[0], new Vector3((x + ((x == 0) ? -0.5f : 0.5f)) * tileSize, 0f, y * tileSize + k), Quaternion.identity) as GameObject;
                        instanceWall.transform.SetParent(levelHolder);
                    }
                }

                if (y == 0 || y == rows - 1)
                {
                    for (int k = (int)(-tileSize * 0.5f); k < (int)(tileSize * 0.5f) + 1; k++)
                    {
                        GameObject instanceWall = Instantiate(walls[1], new Vector3(x * tileSize + k, 0f, (y + ((y == 0) ? -0.5f : 0.5f)) * tileSize), Quaternion.identity) as GameObject;
                        instanceWall.transform.SetParent(levelHolder);
                    }
                }
            }
        }
    }
}
