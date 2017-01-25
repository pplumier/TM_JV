using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walking : MonoBehaviour {

    private Vector3 LastPosition = new Vector3(0,0,0);
    private Animator anim;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        anim.SetFloat("Speed", Vector3.Magnitude(transform.position - LastPosition) * 10);
        LastPosition = transform.position;
	}
}
