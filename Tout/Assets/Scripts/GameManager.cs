using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public LevelManager levelScript;

    private int nbLevel = 5;
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
    private int currentLevel = 0;
    private Text finishText; 

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
        finishText = GameObject.Find("FinishText").GetComponent<Text>() as Text;
        finishText.text = "";

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
        tabLevelNbWalls[0] = 700;
        tabLevelNbTypeTiles[0] = 6;
        tabLevelMaxNbTilesByRoom[0] = 9;
        tabLevelTileSize[0] = 10;
        tabLevelRoomSize[0] = 4;
        tabLevelNbGoals[0] = 20;
        tabLevelNbLamps[0] = 20;
        tabLevelNbBarricades[0] = 50;

        tabLevelColums[1] = 3;
        tabLevelRows[1] = 10;
        tabLevelNbWalls[1] = 300;
        tabLevelNbTypeTiles[1] = 6;
        tabLevelMaxNbTilesByRoom[1] = 9;
        tabLevelTileSize[1] = 10;
        tabLevelRoomSize[1] = 2;
        tabLevelNbGoals[1] = 2;
        tabLevelNbLamps[1] = 4;
        tabLevelNbBarricades[1] = 20;

        tabLevelColums[2] = 8;
        tabLevelRows[2] = 8;
        tabLevelNbWalls[2] = 900;
        tabLevelNbTypeTiles[2] = 6;
        tabLevelMaxNbTilesByRoom[2] = 9;
        tabLevelTileSize[2] = 10;
        tabLevelRoomSize[2] = 2;
        tabLevelNbGoals[2] = 20;
        tabLevelNbLamps[2] = 40;
        tabLevelNbBarricades[2] = 20;

        tabLevelColums[3] = 2;
        tabLevelRows[3] = 5;
        tabLevelNbWalls[3] = 150;
        tabLevelNbTypeTiles[3] = 6;
        tabLevelMaxNbTilesByRoom[3] = 9;
        tabLevelTileSize[3] = 10;
        tabLevelRoomSize[3] = 3;
        tabLevelNbGoals[3] = 2;
        tabLevelNbLamps[3] = 4;
        tabLevelNbBarricades[3] = 75;

        tabLevelColums[4] = 8;
        tabLevelRows[4] = 8;
        tabLevelNbWalls[4] = 1200;
        tabLevelNbTypeTiles[4] = 6;
        tabLevelMaxNbTilesByRoom[4] = 9;
        tabLevelTileSize[4] = 10;
        tabLevelRoomSize[4] = 3;
        tabLevelNbGoals[4] = 20;
        tabLevelNbLamps[4] = 50;
        tabLevelNbBarricades[4] = 75;

        levelScript.LevelSetup(0, tabLevelColums[0], tabLevelRows[0], tabLevelNbWalls[0], tabLevelNbTypeTiles[0], tabLevelMaxNbTilesByRoom[0],
            tabLevelTileSize[0], tabLevelRoomSize[0], tabLevelNbGoals[0], tabLevelNbLamps[0], tabLevelNbBarricades[0], 0f);
    }
	
	public void DestroyPreviousLevel()
    {
        if (currentLevel + 1 < nbLevel)
        {
            GameObject player = GameObject.Find("Player");
            float oldPlayerPosX = (player.transform.localPosition.x % tabLevelTileSize[currentLevel]) - 0.5f * tabLevelTileSize[currentLevel];
            // Il faut changer la position du joueur avant de générer à nouveau le niveau pour qu'il ne déclenche pas le trigger généré pour le nouveau niveau 
            player.transform.localPosition = new Vector3(-50f, player.transform.localPosition.y, -50f);

            levelScript.DestroyLevel(currentLevel);
            ++currentLevel;
            levelScript.LevelSetup(currentLevel, tabLevelColums[currentLevel], tabLevelRows[currentLevel], tabLevelNbWalls[currentLevel],
                tabLevelNbTypeTiles[currentLevel], tabLevelMaxNbTilesByRoom[currentLevel], tabLevelTileSize[currentLevel], tabLevelRoomSize[currentLevel],
                tabLevelNbGoals[currentLevel], tabLevelNbLamps[currentLevel], tabLevelNbBarricades[currentLevel], oldPlayerPosX);
        }
        else
        {
            finishText.text = "The game is finished!";
        }
    }
}
