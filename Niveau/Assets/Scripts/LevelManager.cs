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
    public int nbWalls = 200;
    public int nbTypeTiles;
    public int maxNbTilesByRoom;
    public int roomSize = 2;
    public GameObject[] floorRoom1Tiles = null;
    public GameObject[] floorRoom2Tiles = null;
    public GameObject[] floorRoom3Tiles = null;
    public GameObject[] floorRoom4Tiles = null;
    public GameObject[] floorRoom5Tiles = null;
    public GameObject[] floorRoom6Tiles = null;
    public GameObject[] floorRoom7Tiles = null;
    public GameObject[] floorRoom8Tiles = null;
    public GameObject[] floorRoom9Tiles = null;
    public GameObject[] floorRoom10Tiles = null;
    public GameObject[] floorRoom11Tiles = null;
    public GameObject[] walls;

    private Transform levelHolder;
    private int tileSize = 10;
    private GameObject[,] floorTiles;
    private int[] sizeFloorRoomTiles;

    void InitialiseFloorTiles(GameObject[] floorRoomTiles, int number)
    {
        if (floorRoomTiles != null)
        {
            for (int i = 0; i < floorRoomTiles.Length; i++)
            {
                floorTiles[number - 1, i] = floorRoomTiles[i];
                sizeFloorRoomTiles[number - 1] = floorRoomTiles.Length;
            }
        }
    }

    public void LevelSetup()
    {
        floorTiles = new GameObject[nbTypeTiles, maxNbTilesByRoom];
        sizeFloorRoomTiles = new int[nbTypeTiles];

        InitialiseFloorTiles(floorRoom1Tiles, 1);
        InitialiseFloorTiles(floorRoom2Tiles, 2);
        InitialiseFloorTiles(floorRoom3Tiles, 3);
        InitialiseFloorTiles(floorRoom4Tiles, 4);
        InitialiseFloorTiles(floorRoom5Tiles, 5);
        InitialiseFloorTiles(floorRoom6Tiles, 6);
        InitialiseFloorTiles(floorRoom7Tiles, 7);
        InitialiseFloorTiles(floorRoom8Tiles, 8);
        InitialiseFloorTiles(floorRoom9Tiles, 9);
        InitialiseFloorTiles(floorRoom10Tiles, 10);
        InitialiseFloorTiles(floorRoom11Tiles, 11);

        levelHolder = new GameObject("Level").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                int Room = Random.Range(0, nbTypeTiles);

                for (int m = 0; m < roomSize; m++)
                {
                    for (int l = 0; l < roomSize; l++)
                    {
                        GameObject toInstantiateTile = floorTiles[Room, Random.Range(0, sizeFloorRoomTiles[Room])];
                        GameObject instanceTile = Instantiate(toInstantiateTile, new Vector3((x * roomSize + m) * tileSize, 0f, (y * roomSize + l) * tileSize), Quaternion.identity) as GameObject;
                        instanceTile.transform.SetParent(levelHolder);

                        if ((x == 0 && m == 0) || (x == columns - 1 && m == roomSize - 1))
                        {
                            for (int k = (int)(-tileSize * 0.5f); k < (int)(tileSize * (roomSize - 0.5f)) + 1; k++)
                            {
                                GameObject instanceWall = Instantiate(walls[0], new Vector3((x * roomSize + ((x == 0) ? 0 : roomSize) - 0.5f) * tileSize, 0f, y * tileSize * roomSize + k), Quaternion.identity) as GameObject;
                                instanceWall.transform.SetParent(levelHolder);
                            }
                        }

                        if ((y == 0 && l == 0) || (y == rows - 1 && l == roomSize - 1))
                        {
                            for (int k = (int)(-tileSize * 0.5f); k < (int)(tileSize * (roomSize - 0.5f)) + 1; k++)
                            {
                                GameObject instanceWall = Instantiate(walls[1], new Vector3(x * tileSize * roomSize + k, 0f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                                instanceWall.transform.SetParent(levelHolder);
                            }
                        }
                    }
                }
            }
        }

        int nbHorizontalWalls = Random.Range(1, nbWalls - 1);
        int nbVerticalWalls = nbWalls - nbHorizontalWalls;

        for (int k = 0; k < nbHorizontalWalls; k++)
        {
            int posX = Random.Range(1, columns - 1);
            int posY = Random.Range(1, (rows - 1) * tileSize);
            GameObject instanceInsideWall = Instantiate(walls[0], new Vector3((posX - 0.5f) * tileSize, 0f, posY), Quaternion.identity) as GameObject;
            instanceInsideWall.transform.SetParent(levelHolder);
        }

        for (int k = 0; k < nbVerticalWalls; k++)
        {
            int posX = Random.Range(1, (columns - 1) * tileSize);
            int posY = Random.Range(1, rows - 1);
            GameObject instanceInsideWall = Instantiate(walls[1], new Vector3(posX, 0f, (posY - 0.5f) * tileSize), Quaternion.identity) as GameObject;
            instanceInsideWall.transform.SetParent(levelHolder);
        }
    }
}
