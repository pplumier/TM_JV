using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA2Behavior : MonoBehaviour {

	private bool isSearching;
	private bool isAttacking;
	private bool isRunningAway;
	
	public GameObject player;
	public Transform escapePoint;
	private UnityEngine.AI.NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isSearching = true;
		isAttacking = false;
		isRunningAway = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isSearching) {
			Vector3 direction = Camera.main.transform.position - transform.position;

			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));
			
			if (hit.collider != null && hit.collider.CompareTag("Player")) {
				isSearching = false;
				isAttacking = true;
			}
			else {
				// Moving randomly
			}
		}
		else if (isAttacking) {
			// If the light isn't on him
			agent.SetDestination(player.transform.position);
			// Else
			// isAttacking = false;
			// isRunningAway = true;
		}
		else if (isRunningAway) {
			agent.SetDestination(escapePoint.position);
		}
		else {
			// Waiting x seconds before searching again
		}
	}
}
