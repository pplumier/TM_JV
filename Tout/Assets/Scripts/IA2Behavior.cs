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
	
	private GameObject plight;
	
	public GameObject player;
	private UnityEngine.AI.NavMeshAgent agent;
	
	private GameObject[] escapePoints;
	private Transform target;
	
	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isSearching = true;
		isAttacking = false;
		isRunningAway = false;
		time = timeBeforeSearching;
		player = GameObject.FindGameObjectWithTag("Player");
		
		plight = GameObject.FindGameObjectWithTag("PlayerLight");
		
		escapePoints = GameObject.FindGameObjectsWithTag("FleePosition");
		target = null;
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
			// If the player is visible, his light is on, and points the monster	
			bool isLightOn = plight.GetComponent<LampControl>().IsOn();

			Vector3 direction = Camera.main.transform.position - transform.position;
			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));

			if (isLightOn && hit.collider != null && hit.collider.CompareTag("Player") && Vector3.Angle(plight.transform.forward, -direction) < 50) {
				isAttacking = false;
				isRunningAway = true;
			}
			// Else
			else {
				agent.SetDestination(player.transform.position);
			}
		}
		else if (isRunningAway) {
			if (target != null) {
				agent.SetDestination(target.position);
			}
			else {
				// Flee to the furthest escape point	
				target = escapePoints[0].transform;
				if (Vector3.Distance(target.position, transform.position) < Vector3.Distance(escapePoints[1].transform.position, transform.position))
					target = escapePoints[1].transform;
				if (Vector3.Distance(target.position, transform.position) < Vector3.Distance(escapePoints[2].transform.position, transform.position))
					target = escapePoints[2].transform;
				if (Vector3.Distance(target.position, transform.position) < Vector3.Distance(escapePoints[3].transform.position, transform.position))
					target = escapePoints[3].transform;
				agent.SetDestination(target.position);		
			}	
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
