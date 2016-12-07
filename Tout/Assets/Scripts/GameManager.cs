using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public LevelManager levelScript;

    private int nbLevel = 3;
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
        tabLevelMaxNbTilesByRoom[0] = 4;
        tabLevelTileSize[0] = 10;
        tabLevelRoomSize[0] = 4;
        tabLevelNbGoals[0] = 20;
        tabLevelNbLamps[0] = 20;
        tabLevelNbBarricades[0] = 50;

        tabLevelColums[1] = 10;
        tabLevelRows[1] = 10;
        tabLevelNbWalls[1] = 1000;
        tabLevelNbTypeTiles[1] = 6;
        tabLevelMaxNbTilesByRoom[1] = 4;
        tabLevelTileSize[1] = 10;
        tabLevelRoomSize[1] = 2;
        tabLevelNbGoals[1] = 20;
        tabLevelNbLamps[1] = 40;
        tabLevelNbBarricades[1] = 20;

        tabLevelColums[2] = 10;
        tabLevelRows[2] = 10;
        tabLevelNbWalls[2] = 1400;
        tabLevelNbTypeTiles[2] = 6;
        tabLevelMaxNbTilesByRoom[2] = 4;
        tabLevelTileSize[2] = 10;
        tabLevelRoomSize[2] = 3;
        tabLevelNbGoals[2] = 20;
        tabLevelNbLamps[2] = 50;
        tabLevelNbBarricades[2] = 75;

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
