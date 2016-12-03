using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    public GameObject roof;
    public GameObject transitionLevelFloor;
    public GameObject door;
    public GameObject closeDoor;
    public GameObject lamp;
    public GameObject restartLevel;
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
    private UnityEngine.AI.NavMeshPath path;
    private List<GameObject> insideHorizontalWallList;
    private List<GameObject> insideVerticalWallList;
    private List<GameObject> removeHorizontalWallList;
    private List<GameObject> removeVerticalWallList;
    private bool[,] barricadePositionList;
    private bool[,] lampPositionList;
    private bool[] isInInsideHorizontalWallList;
    private bool[] isInInsideVerticalWallList;
    private int debug;
    private int debug2;
    private const int planSize = 10;
    private const float sizeDoor = 7f;
    private const int transitionSize = 3;

    private int numLevel = 0;
    private int columns = 5;
    private int rows = 5;
    private int nbWalls = 400;
    private int nbTypeTiles = 6;
    private int maxNbTilesByRoom = 3;
    private int tileSize = 10;
    private int roomSize = 4;
    private int nbGoals = 20;
    private int nbLamps = 20;
    private int nbBarricades = 50;

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


    void FloorSetup(int nextLevelDoor, int previousLevelDoor)
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

        roof.transform.localScale = new Vector3((float)tileSize / (float)planSize * roomSize, -1f, (float)tileSize / (float)planSize * roomSize);

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
                        toInstantiateTile.transform.localScale = new Vector3((float)tileSize / (float)planSize, toInstantiateTile.transform.localScale.y, (float)tileSize / (float)planSize);
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
                    if ((y == rows - 1 && x == nextLevelDoor) || (y == 0 && x == previousLevelDoor))
                    {
                        float epsilonSecuDoor = 2f;
                        float epsilonDeepthDoor = 0.5f;

                        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor)/2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
                        GameObject instanceWall = Instantiate(outsideWalls[1], new Vector3(((x + 0.5f) * roomSize - 0.5f) * tileSize - (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                        instanceWall.transform.SetParent(levelHolder);

                        door.transform.localRotation = new Quaternion(0f, -90f, 0f, 0f);
                        GameObject instanceDoor = Instantiate(door, new Vector3(((x + 0.5f) * roomSize) * tileSize - sizeDoor / 2f, 0f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize - epsilonDeepthDoor), Quaternion.identity) as GameObject;
                        instanceDoor.transform.SetParent(levelHolder);

                        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor) / 2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
                        GameObject instanceOtherWall = Instantiate(outsideWalls[1], new Vector3(((x + 0.5f) * roomSize - 0.5f) * tileSize + (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                        instanceOtherWall.transform.SetParent(levelHolder);
                    }
                    else
                    {
                        outsideWalls[1].transform.localScale = new Vector3(tileSize * roomSize, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
                        GameObject instanceWall = Instantiate(outsideWalls[1], new Vector3(((x + 0.5f) * roomSize - 0.5f) * tileSize, outsideWalls[1].transform.localScale.y / 2f, (y * roomSize + ((y == 0) ? 0 : roomSize) - 0.5f) * tileSize), Quaternion.identity) as GameObject;
                        instanceWall.transform.SetParent(levelHolder);
                    }
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
        /*path = new UnityEngine.AI.NavMeshPath();*/

        for (int k = 0; k < nbGoals; k++)
        {
            int posX = Random.Range(0, columns * roomSize);
            int posY = Random.Range(0, rows * roomSize);

            while (posY < rows / 2 * roomSize || !barricadePositionList[posX, posY])
            {
                posX = Random.Range(0, columns * roomSize);
                posY = Random.Range(0, rows * roomSize);
            }
            /*
            Debug.Log(posX * tileSize + " || " + posY * tileSize + " || " + UnityEngine.AI.NavMesh.CalculatePath(new Vector3(-20f, 0f, -20f), new Vector3(posX * tileSize, 0f, posY * tileSize), UnityEngine.AI.NavMesh.GetAreaFromName("walkable"), path) + " || " + UnityEngine.AI.NavMesh.CalculatePath(new Vector3(-20f, 0f, -20f), new Vector3(posX * tileSize, 0f, posY * tileSize), UnityEngine.AI.NavMesh.AllAreas, path));

            if (UnityEngine.AI.NavMesh.CalculatePath(new Vector3(0f, 0f, 0f), new Vector3(posX * tileSize, 0f, posY * tileSize), UnityEngine.AI.NavMesh.GetAreaFromName("walkable"), path))//NavMesh.AllAreas
            {*/
                GameObject instanceGoal = Instantiate(goal, new Vector3(posX * tileSize, goal.transform.localScale.y * 3f / 4f, posY * tileSize), Quaternion.identity) as GameObject;
                instanceGoal.transform.SetParent(levelHolder);
                barricadePositionList[posX, posY] = false;
            /*}*/
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

                                    scaleY = (scaleY + otherScaleY) / 2f + Mathf.Abs(posY - otherPosY);
                                    posY = posMin + (scaleY - scaleMin) / 2f;
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

                                    scaleX = (scaleX + otherscaleX) / 2f + Mathf.Abs(posX - otherPosX);
                                    posX = posMin + (scaleX - scaleMin) / 2f;
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


    void LampSetup()
    {
        for (int k = 0; k < nbLamps; k++)
        {
            int posX = Random.Range(0, columns * roomSize);
            int posY = Random.Range(0, rows * roomSize);

            while (!lampPositionList[posX, posY])
            {
                posX = Random.Range(0, columns * roomSize);
                posY = Random.Range(0, rows * roomSize);
            }

            GameObject instanceLamp = Instantiate(lamp, new Vector3(posX * tileSize, outsideWalls[0].transform.localScale.y + lamp.transform.localScale.y /2f, posY * tileSize), Quaternion.identity) as GameObject;
            instanceLamp.transform.SetParent(levelHolder);
            lampPositionList[posX, posY] = false;
        }
    }


    void TransitionLevelSetup(int nextLevelDoor, int previousLevelDoor, float oldPlayerPosX)
    {
        GameObject toInstantiateTile = transitionLevelFloor;
        GameObject instanceWall, instanceOtherWall, instanceCloseDoor;

        // génération de la transition pour le prochain level
        for (int l = 0; l < transitionSize + 1; l++)
        {
            toInstantiateTile.transform.localScale = new Vector3((float)tileSize / (float)planSize, toInstantiateTile.transform.localScale.y, (float)tileSize / (float)planSize);
            GameObject instanceTile = Instantiate(toInstantiateTile,
                new Vector3(((nextLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize, 0f, (rows * roomSize + l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceTile.transform.SetParent(levelHolder);

            GameObject instanceRoof = Instantiate(roof,
                new Vector3(((nextLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize, outsideWalls[0].transform.localScale.y, (rows * roomSize + l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceRoof.transform.SetParent(levelHolder);

            outsideWalls[0].transform.localScale = new Vector3(outsideWalls[0].transform.localScale.x, outsideWalls[0].transform.localScale.y, tileSize);
            instanceWall = Instantiate(outsideWalls[0],
                new Vector3(((nextLevelDoor + 0.5f) * roomSize - 1f) * tileSize, outsideWalls[0].transform.localScale.y / 2f, (rows * roomSize + l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceWall.transform.SetParent(levelHolder);

            instanceOtherWall = Instantiate(outsideWalls[0],
                new Vector3((nextLevelDoor + 0.5f) * roomSize * tileSize, outsideWalls[0].transform.localScale.y / 2f, (rows * roomSize + l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceOtherWall.transform.SetParent(levelHolder);
        }

        float epsilonSecuDoor = 2f;
        float epsilonDeepthDoor = 0.5f;

        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor) / 2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
        instanceWall = Instantiate(outsideWalls[1],
            new Vector3(((nextLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize - (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (rows * roomSize - 0.5f + transitionSize) * tileSize),
            Quaternion.identity) as GameObject;
        instanceWall.transform.SetParent(levelHolder);

        closeDoor.transform.localRotation = new Quaternion(0f, -90f, 0f, 0f);
        instanceCloseDoor = Instantiate(closeDoor,
            new Vector3(((nextLevelDoor + 0.5f) * roomSize) * tileSize - sizeDoor / 2f, 0f, (rows * roomSize - 0.5f + transitionSize) * tileSize - epsilonDeepthDoor),
            Quaternion.identity) as GameObject;
        instanceCloseDoor.transform.SetParent(levelHolder);

        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor) / 2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
        instanceOtherWall = Instantiate(outsideWalls[1],
            new Vector3(((nextLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize + (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (rows * roomSize - 0.5f + transitionSize) * tileSize),
            Quaternion.identity) as GameObject;
        instanceOtherWall.transform.SetParent(levelHolder);

        restartLevel.transform.localScale = new Vector3(tileSize * roomSize, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
        GameObject instanceRestartLevel = Instantiate(restartLevel,
            new Vector3(((nextLevelDoor + 0.5f) * roomSize) * tileSize - sizeDoor / 2f, restartLevel.transform.localScale.y / 2f, (rows * roomSize + 0.5f) * tileSize),
            Quaternion.identity) as GameObject;
        instanceRestartLevel.transform.SetParent(levelHolder);

        // génération de la transition pour le début du level
        for (int l = 0; l < transitionSize + 1; l++)
        {
            toInstantiateTile.transform.localScale = new Vector3((float)tileSize / (float)planSize, toInstantiateTile.transform.localScale.y, (float)tileSize / (float)planSize);
            GameObject instanceTile = Instantiate(toInstantiateTile,
                new Vector3(((previousLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize, 0f, (-1 - l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceTile.transform.SetParent(levelHolder);

            GameObject instanceRoof = Instantiate(roof,
                new Vector3(((previousLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize, outsideWalls[0].transform.localScale.y, (-1 - l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceRoof.transform.SetParent(levelHolder);

            outsideWalls[0].transform.localScale = new Vector3(outsideWalls[0].transform.localScale.x, outsideWalls[0].transform.localScale.y, tileSize);
            instanceWall = Instantiate(outsideWalls[0],
                new Vector3(((previousLevelDoor + 0.5f) * roomSize - 1f) * tileSize, outsideWalls[0].transform.localScale.y / 2f, (-1 - l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceWall.transform.SetParent(levelHolder);

            instanceOtherWall = Instantiate(outsideWalls[0],
                new Vector3((previousLevelDoor + 0.5f) * roomSize * tileSize, outsideWalls[0].transform.localScale.y / 2f, (-1 - l) * tileSize),
                Quaternion.identity) as GameObject;
            instanceOtherWall.transform.SetParent(levelHolder);
        }

        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor) / 2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
        instanceWall = Instantiate(outsideWalls[1],
            new Vector3(((previousLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize - (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (-transitionSize - 0.5f) * tileSize),
            Quaternion.identity) as GameObject;
        instanceWall.transform.SetParent(levelHolder);

        closeDoor.transform.localRotation = new Quaternion(0f, -90f, 0f, 0f);
        instanceCloseDoor = Instantiate(closeDoor,
            new Vector3(((previousLevelDoor + 0.5f) * roomSize) * tileSize - sizeDoor / 2f, 0f, (-transitionSize - 0.5f) * tileSize - epsilonDeepthDoor),
            Quaternion.identity) as GameObject;
        instanceCloseDoor.transform.SetParent(levelHolder);

        outsideWalls[1].transform.localScale = new Vector3((tileSize * roomSize - sizeDoor) / 2f + epsilonSecuDoor, outsideWalls[1].transform.localScale.y, outsideWalls[1].transform.localScale.z);
        instanceOtherWall = Instantiate(outsideWalls[1],
            new Vector3(((previousLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize + (outsideWalls[1].transform.localScale.x - epsilonSecuDoor + sizeDoor) / 2f, outsideWalls[1].transform.localScale.y / 2f, (-transitionSize - 0.5f) * tileSize),
            Quaternion.identity) as GameObject;
        instanceOtherWall.transform.SetParent(levelHolder);

        GameObject player = GameObject.Find("Player");
        GameObject camera = GameObject.Find("Main Camera");
        float deltaCameraY = -player.transform.localPosition.z;

        float newPlayerPosX = ((previousLevelDoor + 0.5f) * roomSize - 0.5f) * tileSize + oldPlayerPosX;
        player.transform.localPosition = new Vector3(newPlayerPosX, player.transform.localPosition.y, (-transitionSize + 0.5f) * tileSize);

        //MODIFICATIONS A PREVOIR POUR LE CHANGEMENT DE CAMERA EN VUE FPS
        float deltaCameraX = player.transform.localPosition.x;
        deltaCameraY += player.transform.localPosition.z;
        camera.transform.localPosition = new Vector3(deltaCameraX, camera.transform.localPosition.y, camera.transform.localPosition.z + deltaCameraY);
    }


    public void LevelSetup(int levelNumLevel, int levelColums, int levelRows, int levelNbWalls, int levelNbTypeTiles, int levelMaxNbTilesByRoom, int levelTileSize,
        int levelRoomSize, int levelNbGoals, int levelNbLamps, int levelNbBarricades, float levelOldPlayerPosX)
    {
        numLevel = levelNumLevel;
        columns = levelColums;
        rows = levelRows;
        nbWalls = levelNbWalls;
        nbTypeTiles = levelNbTypeTiles;
        maxNbTilesByRoom = levelMaxNbTilesByRoom;
        tileSize = levelTileSize;
        roomSize = levelRoomSize;
        nbGoals = levelNbGoals;
        nbLamps = levelNbLamps;
        nbBarricades = levelNbBarricades;

        levelHolder = new GameObject("Level").transform;
        goalHolder = new GameObject("Goals").transform;
        goalHolder.transform.SetParent(GameObject.Find("Canvas").transform);
        goalHolder.transform.localPosition = new Vector3(0f, 0f, 0f);
        insideHorizontalWallList = new List<GameObject>();
        insideVerticalWallList = new List<GameObject>();
        removeHorizontalWallList = new List<GameObject>();
        removeVerticalWallList = new List<GameObject>();
        barricadePositionList = new bool[columns * roomSize, rows * roomSize];
        lampPositionList = new bool[columns * roomSize, rows * roomSize];

        for (int x = 0; x < columns * roomSize; x++)
        {
            for (int y = 0; y < rows * roomSize; y++)
            {
                barricadePositionList[x, y] = true;
                lampPositionList[x, y] = true;
            }
        }

        int nextLevelDoor = Random.Range(0, columns);
        int previousLevelDoor = Random.Range(0, columns);

        FloorSetup(nextLevelDoor, previousLevelDoor);
        InsideWallSetup();
        OneWall();
        GoalSetup();
        BarricadeSetup();
        LampSetup();
        TransitionLevelSetup(nextLevelDoor, previousLevelDoor, levelOldPlayerPosX);

        Debug.Log("Horizontal avant : " + debug + ", après : " + insideHorizontalWallList.Count + " test " + removeHorizontalWallList.Count);
        Debug.Log("Vertical avant : " + debug2 + ", après : " + insideVerticalWallList.Count + " test " + removeVerticalWallList.Count);
    }


    public void DestroyLevel(int levelNumLevel)
    {
        Destroy(GameObject.Find("Level"));
        Destroy(GameObject.Find("Goals"));
        
    }
}
