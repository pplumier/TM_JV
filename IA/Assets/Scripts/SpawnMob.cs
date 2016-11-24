using UnityEngine;
using System.Collections;

public class SpawnMob : MonoBehaviour {

    float lastTime = 0;
    public GameObject toSpawn;
    public GameObject light;
    public GameObject player;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.fixedTime - lastTime > 1)
        {
            GameObject go = Instantiate(toSpawn);
            go.GetComponent<Poursuite>().joueur = player;
            go.GetComponent<SeeLight>().light = light;
            lastTime = Time.fixedTime;
        }       
	}
}
