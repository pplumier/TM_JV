using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(transform.gameObject, 15f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
