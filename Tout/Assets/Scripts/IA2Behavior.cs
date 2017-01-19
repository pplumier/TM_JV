using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

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

	private bool escapePointsFound;

	private Animator anim;

	// Use this for initialization
	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isSearching = true;
		isAttacking = false;
		isRunningAway = false;
		time = timeBeforeSearching;
		player = GameObject.FindGameObjectWithTag("Player");

		plight = GameObject.FindGameObjectWithTag("PlayerLight");

		escapePointsFound = false;
		target = null;

		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (!escapePointsFound) {
			escapePointsFound = true;
			escapePoints = GameObject.FindGameObjectsWithTag("FleePosition");
			anim.SetBool ("walking", true);
		}

		if (isSearching) {
			Vector3 direction = Camera.main.transform.position - transform.position;

			RaycastHit hit;
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));

			// If it sees the player
			if (hit.collider != null && hit.collider.CompareTag("Player") && Vector3.Dot(transform.forward, (player.transform.position - transform.position)) > 0) {
				isSearching = false;
				isAttacking = true;
				anim.SetBool ("walking", true);
			}
			else {
				// Moves randomly
				Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

				randomDirection += transform.position;
				UnityEngine.AI.NavMeshHit nmHit;
				UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out nmHit, walkRadius, 1);

				agent.SetDestination(nmHit.position);
				anim.SetBool ("walking", true);

			}
		}
		else if (isAttacking) {
			// If the player is visible, his light is on, and points the monster
			bool isLightOn = plight.GetComponent<LampControl>().IsOn();

			Vector3 direction = Camera.main.transform.position - (transform.position + transform.up*2);
			RaycastHit hit;
			Physics.Raycast(transform.position + transform.up*2, direction, out hit, Vector3.Distance(transform.position, player.transform.position));

			//Debug.Log("Angle: " + Vector3.Angle(plight.transform.forward, -direction));
			if (isLightOn && hit.collider != null && hit.collider.CompareTag("Player") && Vector3.Angle(plight.transform.forward, -direction) < 50) {
				isAttacking = false;
				isRunningAway = true;
				anim.SetBool ("walking", true);
			}
			// Else
			else {
				agent.SetDestination(player.transform.position);
			}
		}
		else if (isRunningAway) {
			if (target != null) {
				agent.SetDestination(target.position);
				agent.speed = 10;
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
				if (Vector3.Distance(transform.position, target.transform.position) < 5) {
					target = null;
					isRunningAway = false;
				}
		}
		else {
			//Debug.Log("EscapePoint: Time: " + time);
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
