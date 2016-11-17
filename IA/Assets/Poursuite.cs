using UnityEngine;
using System.Collections;

public class Poursuite : MonoBehaviour {

    public GameObject joueur;
    NavMeshAgent nm;

	// Use this for initialization
	void Start () {
        nm = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        nm.SetDestination(joueur.transform.position);
	}
}
