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

    public void LevelSetup()
    {
        levelHolder = new GameObject("Level").transform;

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x*10f, 0f, y*10f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(levelHolder);
            }
        }
    }
}
