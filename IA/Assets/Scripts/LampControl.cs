using UnityEngine;
using System.Collections;

public class LampControl : MonoBehaviour {
    Light spotlight;
	// Use this for initialization
	void Start () {
        spotlight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
            spotlight.enabled = !spotlight.enabled;
	}
}
