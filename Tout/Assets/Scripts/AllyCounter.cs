using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCounter : MonoBehaviour {

    public int girlSaved = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ally")
        {
            Destroy(other.gameObject);
            girlSaved += 1;
            Debug.Log("Girl Saved " + girlSaved);
        }
    }
}
