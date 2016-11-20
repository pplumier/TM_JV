using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public LevelManager levelScript;

	void Awake ()
    {
        levelScript = GetComponent<LevelManager>();
        InitGame();
	}

    void InitGame()
    {
        levelScript.LevelSetup();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
