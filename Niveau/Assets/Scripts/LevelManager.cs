using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    public int columns = 20;
    public int rows = 10;
    public int nbWalls = 200;
    public int nbTypeTiles;
    public int maxNbTilesByRoom;
    public int tileSize = 10;
    public int roomSize = 3;
    public int nbGoals = 20;
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
    public GameObject[] outsideWalls;
    public GameObject goal;

    private Transform levelHolder;
    private Transform goalHolder;
    private GameObject[,] floorTiles;
    private int[] sizeFloorRoomTiles;
    private NavMeshPath path;
    private List<GameObject> insideHorizontalWallList;
    private List<GameObject> insideVerticalWallList;
    private List<GameObject> removeHorizontalWallList;
    private List<GameObject> removeVerticalWallList;
    private bool[] isInInsideHorizontalWallList;
    private bool[] isInInsideVerticalWallList;
    private int debug;
    private int debug2;

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


    void FloorSetup()
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
                        toInstantiateTile.transform.localScale = new Vector3((float)tileSize / (float)10, toInstantiateTile.transform.localScale.y, (float)tileSize / (float)10);
                        GameObject instanceTile = Instantiate(toInstantiateTile, new Vector3((x * roomSize + m) * tileSize, 0f, (y * roomSize + l) * tileSize), Quaternion.identity) as GameObject;
                        instanceTile.transform.SetParent(levelHolder);
                    }
                }
            }
        }

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0 || x == columns - 1)
                {
                    outsideWalls[0].transform.localScale = new Vector3(outsideWalls[0].transform.localScale.x, outsideWalls[0].transform.localScale.y, tileSize * roomSize);
                    GameObject instanceWall = Instantiate(outsideWalls[0], new Vector3((x * roomSize + ((x == 0) ? 0 : roomSize) - 0.5f) * tileSize, outsideWalls[0].transform.localScale.y / 2f, ((y + 0.5f) * roomSize - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                    instanceWall.transform.SetParent(levelHolder);
                }

                if (y == 0 || y == rows - 1)
                {
                    outsideWalls[1].transform.localScale = new Vector3(tileSize * roomSize, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
                    GameObject instanceWall = Instantiate(outsideWalls[1], new Vector3(((x + 0.5f) * roomSize - 0.5f) * tileSize, outsideWalls[1].transform.localScale.y / 2f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                    instanceWall.transform.SetParent(levelHolder);
                }
            }
        }
    }


    void InsideWallSetup()
    {
        int nbHorizontalWalls = Random.Range(nbWalls / 4, 3 * nbWalls / 4);
        int nbVerticalWalls = nbWalls - nbHorizontalWalls;
        isInInsideHorizontalWallList = new bool[nbHorizontalWalls];
        isInInsideVerticalWallList = new bool[nbVerticalWalls];

        for (int k = 0; k < nbHorizontalWalls; k++)
        {
            isInInsideHorizontalWallList[k] = true;
        }

        for (int k = 0; k < nbVerticalWalls; k++)
        {
            isInInsideVerticalWallList[k] = true;
        }

        debug = nbHorizontalWalls;
        debug2 = nbHorizontalWalls;

        for (int k = 0; k < nbHorizontalWalls; k++)
        {
            int posX = Random.Range(1, columns);
            int posY = Random.Range((int)(-tileSize * 0.5f), (int)((rows * roomSize - 0.5f) * tileSize + 1));
            GameObject instanceInsideWall = Instantiate(walls[0], new Vector3((posX * roomSize - 0.5f) * tileSize, walls[0].transform.localScale.y / 2f, posY), Quaternion.identity) as GameObject;
            instanceInsideWall.transform.SetParent(levelHolder);
            insideHorizontalWallList.Add(instanceInsideWall);
        }

        for (int k = 0; k < nbVerticalWalls; k++)
        {
            int posX = Random.Range((int)(-tileSize * 0.5f), (int)((columns * roomSize - 0.5f) * tileSize + 1));
            int posY = Random.Range(1, rows);
            GameObject instanceInsideWall = Instantiate(walls[1], new Vector3(posX, walls[1].transform.localScale.y / 2f, (posY * roomSize - 0.5f) * tileSize), Quaternion.identity) as GameObject;
            instanceInsideWall.transform.SetParent(levelHolder);
            insideVerticalWallList.Add(instanceInsideWall);
        }
    }


    void GoalSetup()
    {
        NavMeshBuilder.BuildNavMesh();
        path = new NavMeshPath();

        for (int k = 0; k < nbGoals; k++)
        {
            int posX = Random.Range(1, columns * roomSize);
            int posY = Random.Range(1, rows * roomSize);

            //Debug.Log(posX * tileSize + " || " + posY * tileSize + " || " + NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.GetAreaFromName("walkable"), path) + " || " + NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.AllAreas, path));

            if (NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.GetAreaFromName("walkable"), path))//NavMesh.AllAreas
            {
                GameObject instanceGoal = Instantiate(goal, new Vector3(posX * tileSize, goal.transform.localScale.y * 3f / 4f, posY * tileSize), Quaternion.identity) as GameObject;
                instanceGoal.transform.SetParent(levelHolder);
            }
        }
    }


    void OneWall()
    {
        int nbDebug = 0;
        for (int j = 0; j < insideHorizontalWallList.Count; j++)
        {
            GameObject wall = insideHorizontalWallList[j];
            if (isInInsideHorizontalWallList[j])
            {
                bool newChange = true;
                float posX = wall.transform.localPosition.x;
                float posY = wall.transform.localPosition.z;
                //float scaleX = wall.transform.localScale.x;
                float scaleY = wall.transform.localScale.z;

                while (newChange)
                {
                    newChange = false;
                    for (int k = 0; k < insideHorizontalWallList.Count; k++)
                    {
                        GameObject otherWall = insideHorizontalWallList[k];
                        if (isInInsideHorizontalWallList[k] && wall != otherWall)
                        {
                            float otherPosX = otherWall.transform.localPosition.x;
                            float otherPosY = otherWall.transform.localPosition.z;
                            float otherScaleY = otherWall.transform.localScale.z;

                            float chevauchement = ((scaleY + otherScaleY) / 2f - Mathf.Abs(posY - otherPosY)) / 2f;

                            if (otherPosX == posX && chevauchement >= 0)
                            {
                                newChange = true;
                            
                                float posMin = (posY < otherPosY) ? posY : otherPosY;
                                float scaleMin = (posY < otherPosY) ? scaleY : otherScaleY;
                                float scaleMax = (posY < otherPosY) ? otherScaleY : scaleY;
                                if (nbDebug < 100)
                                {
                                    float debug3 = (scaleY + otherScaleY) / 2f;
                                    float debug4 = Mathf.Abs(posY - otherPosY);
                                    float debug5 = posY - otherPosY;
                                    Debug.Log("Pour 1er mur : posX " + posX + " posY " + posY + " scaleY " + scaleY + " || " + "2eme mur : otherPosX " + otherPosX + " otherPosY " + otherScaleY + " otherScaleY " + otherScaleY +
                                        "\net chevauchement " + chevauchement + " (scaleY + otherScaleY) / 2f " + debug3 + " Mathf.Abs(posY - otherPosY) " + debug4 + " posY - otherPosY " + debug5);
                                }
                                posY = posMin + (scaleMax / 2f - chevauchement) / ((scaleMax / 2f - chevauchement) + (scaleMin / 2f - chevauchement)) * Mathf.Abs(posY - otherPosY);
                                scaleY = (scaleY + otherScaleY) / 2f + Mathf.Abs(posY - otherPosY);
                                isInInsideHorizontalWallList[k] = false;
                                if (nbDebug < 100)
                                {
                                    Debug.Log("nouveau posY " + posY + "  et nouveau scaleY " + scaleY + "  si NAN " + (scaleMax / 2f - chevauchement) + (scaleMin / 2f - chevauchement));
                                    nbDebug++;
                                }
                                removeHorizontalWallList.Add(otherWall);
                                Destroy(otherWall);
                                --debug2;
                            }
                        }
                    }
                }
                wall.transform.localPosition = new Vector3(posX, wall.transform.localPosition.y, posY);
                wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, scaleY);
            }
        }

        for (int j = 0; j < removeHorizontalWallList.Count; j++)
        {
            insideHorizontalWallList.Remove(removeHorizontalWallList[j]);
        }

        removeHorizontalWallList.Clear();
    }


    public void LevelSetup()
    {
        levelHolder = new GameObject("Level").transform;
        goalHolder = new GameObject("Goals").transform;
        goalHolder.transform.SetParent(GameObject.Find("Canvas").transform);
        goalHolder.transform.localPosition = new Vector3(0f, 0f, 0f);
        insideHorizontalWallList = new List<GameObject>();
        insideVerticalWallList = new List<GameObject>();
        removeHorizontalWallList = new List<GameObject>();
        removeVerticalWallList = new List<GameObject>();

        FloorSetup();
        InsideWallSetup();
        OneWall();
        //GoalSetup();
        Debug.Log("avant : " + debug + ", après : " + insideHorizontalWallList.Count + " || " + debug2 + " test " + removeHorizontalWallList.Count);
    }
}
