using UnityEngine;
using System.Collections;

public class SpawnMob : MonoBehaviour {

    float lastTime = 0;
    public float timeInBetween = 5f;
    public int maxSpawn = 1000;
    public GameObject toSpawn;

    private bool parentSet;

	// Use this for initialization
	void Start () {
        parentSet = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (!parentSet)
        {
            transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            parentSet = true;
        }
        if (Time.fixedTime - lastTime > timeInBetween && transform.childCount < maxSpawn)
        {
            GameObject go = Instantiate(toSpawn);
            go.transform.position = transform.position;
            go.transform.parent = GameObject.FindGameObjectWithTag("Level").transform;
            lastTime = Time.fixedTime;
        }       
	}
}
