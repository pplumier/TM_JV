using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA2Behavior : MonoBehaviour {

	private bool isSearching;
	private bool isAttacking;
	private bool isRunningAway;
	
	public float timeBeforeSearching = 10f;
	private float time;
	
	private int walkRadius = 10;
	
	public GameObject player;
	public Transform escapePoint;
	private UnityEngine.AI.NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isSearching = true;
		isAttacking = false;
		isRunningAway = false;
		time = timeBeforeSearching;
	}
	
	// Update is called once per frame
	void Update () {
		if (isSearching) {
			Vector3 direction = Camera.main.transform.position - transform.position;

			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));
			
			// If it sees the player
			if (hit.collider != null && hit.collider.CompareTag("Player") && Vector3.Dot(transform.forward, (player.transform.position - transform.position)) > 0) {
				isSearching = false;
				isAttacking = true;
			}
			else {
				// Moves randomly
				Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
				 
				randomDirection += transform.position;
				UnityEngine.AI.NavMeshHit nmHit;
				UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out nmHit, walkRadius, 1);
				
				agent.SetDestination(nmHit.position);
				
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
			if (time > 0)
				time -= Time.deltaTime;
			else {
				isRunningAway = false;
				isSearching = true;
				time = timeBeforeSearching;
			}
		}
	}
}
