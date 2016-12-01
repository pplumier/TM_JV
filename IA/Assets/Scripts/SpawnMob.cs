using UnityEngine;
using System.Collections;

public class SpawnMob : MonoBehaviour {

    float lastTime = 0;
    public float timeInBetween = 5f;
    public int maxSpawn = 1000;
    public GameObject toSpawn;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.fixedTime - lastTime > timeInBetween && transform.childCount < maxSpawn)
        {
            GameObject go = Instantiate(toSpawn);
            go.transform.parent = transform;
            lastTime = Time.fixedTime;
        }       
	}
}
