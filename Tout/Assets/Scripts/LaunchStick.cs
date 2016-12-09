using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchStick : MonoBehaviour {

    public GameObject stick;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
        {
            GameObject go = Instantiate(stick);
            go.transform.position = transform.position + transform.forward * 2;
            go.GetComponent<Rigidbody>().AddForce((transform.forward + 0.1f * transform.up) * 1000f);
            go.GetComponent<Rigidbody>().AddTorque(transform.right * 1000f);
        }
	}
}
