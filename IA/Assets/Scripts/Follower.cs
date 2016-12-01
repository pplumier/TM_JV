using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {

    GameObject joueur;
    public bool activate;
    UnityEngine.AI.NavMeshAgent nm;

    // Use this for initialization
    void Start () {
        activate = false;
        nm = GetComponent<UnityEngine.AI.NavMeshAgent>();
        joueur = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (activate)
            nm.SetDestination(joueur.transform.position);
        else
            nm.SetDestination(transform.position);
    }
}
