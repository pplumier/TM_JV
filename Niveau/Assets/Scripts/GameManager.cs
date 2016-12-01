using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public LevelManager levelScript;

    private int nbLevel = 1;
    private int[] tabLevelColums;
    private int[] tabLevelRows;
    private int[] tabLevelNbWalls;
    private int[] tabLevelNbTypeTiles;
    private int[] tabLevelMaxNbTilesByRoom;
    private int[] tabLevelTileSize;
    private int[] tabLevelRoomSize;
    private int[] tabLevelNbGoals;
    private int[] tabLevelNbLamps;
    private int[] tabLevelNbBarricades;

	void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        levelScript = GetComponent<LevelManager>();
        InitGame();
	}

    void InitGame()
    {
        tabLevelColums = new int[nbLevel];
        tabLevelRows = new int[nbLevel];
        tabLevelNbWalls = new int[nbLevel];
        tabLevelNbTypeTiles = new int[nbLevel];
        tabLevelMaxNbTilesByRoom = new int[nbLevel];
        tabLevelTileSize = new int[nbLevel];
        tabLevelRoomSize = new int[nbLevel];
        tabLevelNbGoals = new int[nbLevel];
        tabLevelNbLamps = new int[nbLevel];
        tabLevelNbBarricades = new int[nbLevel];

        tabLevelColums[0] = 5;
        tabLevelRows[0] = 5;
        tabLevelNbWalls[0] = 400;
        tabLevelNbTypeTiles[0] = 6;
        tabLevelMaxNbTilesByRoom[0] = 3;
        tabLevelTileSize[0] = 10;
        tabLevelRoomSize[0] = 4;
        tabLevelNbGoals[0] = 20;
        tabLevelNbLamps[0] = 20;
        tabLevelNbBarricades[0] = 50;

        levelScript.LevelSetup(0, tabLevelColums[0], tabLevelRows[0], tabLevelNbWalls[0], tabLevelNbTypeTiles[0], tabLevelMaxNbTilesByRoom[0],
            tabLevelTileSize[0], tabLevelRoomSize[0], tabLevelNbGoals[0], tabLevelNbLamps[0], tabLevelNbBarricades[0]);
    }
	
	// Update is called once per frame
	void Update () {
	}
}
