using UnityEngine;
using System.Collections;

public class Poursuite : MonoBehaviour {

    public GameObject joueur;
    NavMeshAgent nm;
    SeeLight sl;

	// Use this for initialization
	void Start () {
        nm = GetComponent<NavMeshAgent>();
        sl = GetComponent<SeeLight>();
	}
	
	// Update is called once per frame
	void Update () {
        if(sl.seeLight)
            nm.SetDestination(joueur.transform.position);
        else
            nm.SetDestination(transform.position);
	}
}
