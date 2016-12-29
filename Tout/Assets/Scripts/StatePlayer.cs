using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayer : MonoBehaviour {

	// Is a IA 3 currently on the player?
	private bool isAttacked;
	
	// Use this for initialization
	void Start () {
		isAttacked = false;
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log(isAttacked);
	}
	
	public bool IsAttacked() {
		return isAttacked;
	}
	
	public void SetIsAttacked(bool value) {
		isAttacked = value;
	}
}
