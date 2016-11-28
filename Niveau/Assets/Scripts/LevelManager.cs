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
    public int nbBarricades = 50;
    public GameObject roof;
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
    public GameObject[] AllBarricades;

    private Transform levelHolder;
    private Transform goalHolder;
    private GameObject[,] floorTiles;
    private int[] sizeFloorRoomTiles;
    private NavMeshPath path;
    private List<GameObject> insideHorizontalWallList;
    private List<GameObject> insideVerticalWallList;
    private List<GameObject> removeHorizontalWallList;
    private List<GameObject> removeVerticalWallList;
    private bool[,] barricadePositionList;
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

        roof.transform.localScale = new Vector3((float)tileSize / (float)10 * roomSize, -1f, (float)tileSize / (float)10 * roomSize);

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject instanceRoof = Instantiate(roof, new Vector3(((x + 0.5f) * roomSize - 0.5f) * tileSize, outsideWalls[0].transform.localScale.y, ((y + 0.5f) * roomSize - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                instanceRoof.transform.SetParent(levelHolder);

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
        debug2 = nbVerticalWalls;

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
            int posX = Random.Range(0, columns * roomSize);
            int posY = Random.Range(0, rows * roomSize);

            while (posX < columns / 2 * roomSize && posY < rows / 2 * roomSize)
            {
                posX = Random.Range(0, columns * roomSize);
                posY = Random.Range(0, rows * roomSize);
            }

            //Debug.Log(posX * tileSize + " || " + posY * tileSize + " || " + NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.GetAreaFromName("walkable"), path) + " || " + NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.AllAreas, path));

            if (NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), NavMesh.GetAreaFromName("walkable"), path))//NavMesh.AllAreas
            {
                GameObject instanceGoal = Instantiate(goal, new Vector3(posX * tileSize, goal.transform.localScale.y * 3f / 4f, posY * tileSize), Quaternion.identity) as GameObject;
                instanceGoal.transform.SetParent(levelHolder);
                barricadePositionList[posX, posY] = false;
            }
        }
    }


    void OneWall()
    {
        for (int j = 0; j < insideHorizontalWallList.Count; j++)
        {
            GameObject wall = insideHorizontalWallList[j];
            if (isInInsideHorizontalWallList[j])
            {
                bool newChange = true;
                float posX = wall.transform.localPosition.x;
                float posY = wall.transform.localPosition.z;
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
                                // vérification que le deuxième mur ne soit pas inclus dans le premier
                                if (otherPosY - otherScaleY < posY - scaleY || otherPosY + otherScaleY > posY + scaleY)
                                {
                                    newChange = true;

                                    float posMin = (posY < otherPosY) ? posY : otherPosY;
                                    float scaleMin = (posY < otherPosY) ? scaleY : otherScaleY;
                                    float scaleMax = (posY < otherPosY) ? otherScaleY : scaleY;

                                    posY = posMin + (scaleMax / 2f - chevauchement) / ((scaleMax / 2f - chevauchement) + (scaleMin / 2f - chevauchement)) * Mathf.Abs(posY - otherPosY);
                                    scaleY = (scaleY + otherScaleY) / 2f + Mathf.Abs(posY - otherPosY);
                                }
                                isInInsideHorizontalWallList[k] = false;
                                removeHorizontalWallList.Add(otherWall);
                                Destroy(otherWall);
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




        for (int j = 0; j < insideVerticalWallList.Count; j++)
        {
            GameObject wall = insideVerticalWallList[j];
            if (isInInsideVerticalWallList[j])
            {
                bool newChange = true;
                float posX = wall.transform.localPosition.x;
                float posY = wall.transform.localPosition.z;
                float scaleX = wall.transform.localScale.x;

                while (newChange)
                {
                    newChange = false;
                    for (int k = 0; k < insideVerticalWallList.Count; k++)
                    {
                        GameObject otherWall = insideVerticalWallList[k];
                        if (isInInsideVerticalWallList[k] && wall != otherWall)
                        {
                            float otherPosX = otherWall.transform.localPosition.x;
                            float otherPosY = otherWall.transform.localPosition.z;
                            float otherscaleX = otherWall.transform.localScale.x;

                            float chevauchement = ((scaleX + otherscaleX) / 2f - Mathf.Abs(posX - otherPosX)) / 2f;

                            if (otherPosY == posY && chevauchement >= 0)
                            {
                                // vérification que le deuxième mur ne soit pas inclus dans le premier
                                if (otherPosX - otherscaleX < posX - scaleX || otherPosX + otherscaleX > posX + scaleX)
                                {
                                    newChange = true;

                                    float posMin = (posX < otherPosX) ? posX : otherPosX;
                                    float scaleMin = (posX < otherPosX) ? scaleX : otherscaleX;
                                    float scaleMax = (posX < otherPosX) ? otherscaleX : scaleX;

                                    posX = posMin + (scaleMax / 2f - chevauchement) / ((scaleMax / 2f - chevauchement) + (scaleMin / 2f - chevauchement)) * Mathf.Abs(posX - otherPosX);
                                    scaleX = (scaleX + otherscaleX) / 2f + Mathf.Abs(posX - otherPosX);
                                }
                                isInInsideVerticalWallList[k] = false;
                                removeVerticalWallList.Add(otherWall);
                                Destroy(otherWall);
                            }
                        }
                    }
                }
                wall.transform.localPosition = new Vector3(posX, wall.transform.localPosition.y, posY);
                wall.transform.localScale = new Vector3(scaleX, wall.transform.localScale.y, wall.transform.localScale.z);
            }
        }

        for (int j = 0; j < removeVerticalWallList.Count; j++)
        {
            insideVerticalWallList.Remove(removeVerticalWallList[j]);
        }

        removeVerticalWallList.Clear();
    }


    void BarricadeSetup()
    {
        for (int k = 0; k < nbBarricades && k < columns * roomSize * rows * roomSize - nbGoals - 1; k++)
        {
            int posX = Random.Range(0, columns * roomSize);
            int posY = Random.Range(0, rows * roomSize);

            while ((posX == 0 && posY == 0) || !barricadePositionList[posX, posY])
            {
                posX = Random.Range(0, columns * roomSize);
                posY = Random.Range(0, rows * roomSize);
            }

            barricadePositionList[posX, posY] = false;

            int numBarricade = Random.Range(0, AllBarricades.Length);
            GameObject instanceBarricade = Instantiate(AllBarricades[numBarricade], new Vector3(posX * tileSize, AllBarricades[numBarricade].transform.localScale.y / 2f, posY * tileSize), Quaternion.identity) as GameObject;
            instanceBarricade.transform.SetParent(levelHolder);
        }
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
        barricadePositionList = new bool[columns * roomSize, rows * roomSize];

        for (int x = 0; x < columns * roomSize; x++)
        {
            for (int y = 0; y < rows * roomSize; y++)
            {
                barricadePositionList[x, y] = true;
            }
        }

        FloorSetup();
        InsideWallSetup();
        OneWall();
        GoalSetup();
        BarricadeSetup();
        Debug.Log("Horizontal avant : " + debug + ", après : " + insideHorizontalWallList.Count + " test " + removeHorizontalWallList.Count);
        Debug.Log("Vertical avant : " + debug2 + ", après : " + insideVerticalWallList.Count + " test " + removeVerticalWallList.Count);
    }
}
