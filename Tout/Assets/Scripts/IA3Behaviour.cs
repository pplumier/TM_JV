﻿using UnityEngine;
using System.Collections;

public class IA3Behaviour : MonoBehaviour {
	private GameObject player;
	
	public float maxDistance = 3; // Distance max from where the IA jumps on the player
	
	private Rigidbody r;
	private bool onCeiling;
	private bool onPlayer;
	private bool done; // has already jumped on the player
	private float distToGround;
	
	private bool isAgentActive;
	
	// Rotation
	private float x, y, z;
	private float sumRotation;
	public float moveLimit = 2; // Amount of mouse movement needed to eject the IA from the player
	
	// Escape
	public Transform escapePosition;
	private GameObject[] escapePoints;
	private Transform target;
	private UnityEngine.AI.NavMeshAgent agent;

	
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		maxDistance = 5f;
		onCeiling = true;
		onPlayer = false;
		isAgentActive = false;
		done = false;
		r = GetComponent<Rigidbody>();
		distToGround = GetComponent<Collider>().bounds.extents.y;

		sumRotation = 0;
		//agent = null;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		escapePoints = GameObject.FindGameObjectsWithTag("FleePosition");
		target = null;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
		
		// If the AI is waiting on the ceiling
		if (onCeiling) {
			Vector3 direction = Camera.main.transform.position - transform.position;
			
			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));

			
			if (hit.collider != null && hit.collider.CompareTag("Player") && !player.GetComponent<StatePlayer>().IsAttacked()) {
				if (Vector2.Distance(pos, playerPos) < maxDistance) {
					r.useGravity = true;
					r.AddForce((player.transform.position - transform.position)*100);
					onCeiling = false;
				}
			}	
			
		}
		// If the AI is falling, or on the player
		else if (!IsGrounded()) {
			if (onPlayer) {
				//Debug.Log("OnPLAYER");
				transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
				transform.rotation = Camera.main.transform.rotation;
				
				if (sumRotation < moveLimit) {
					//Debug.Log("Move needed");
					sumRotation += Mathf.Abs(transform.rotation.x - x);
					sumRotation += Mathf.Abs(transform.rotation.y - y);
					sumRotation += Mathf.Abs(transform.rotation.z - z);
				
					x = transform.rotation.x;
					y = transform.rotation.y;
					z = transform.rotation.z;
				}
				else {
					//Debug.Log("Enough movement");
					r.useGravity = true;
					r.freezeRotation = false;
					onPlayer = false;
					player.GetComponent<StatePlayer>().SetIsAttacked(false);
				}
			}
		}
		// If the AI is on the ground
		else {
			if (!isAgentActive) {
				isAgentActive = true;
				agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent.enabled = true;
				agent.speed = 20;
			}
			
			Vector3 direction = Camera.main.transform.position - transform.position;
			
			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));
			
			// If the player can't see it, destroy the object
			if (hit.collider != null && !hit.collider.CompareTag("Player")) {
				Destroy(gameObject);
			}
			// Else, run away to the escape point
			else {
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
			
		}
		
	}
	
	void OnCollisionEnter(Collision col) 
	{
		if (!done && col.gameObject.CompareTag("Player")) 
		{
			//Debug.Log("Hit Player");
			onPlayer = true;
			done = true;
			player.GetComponent<StatePlayer>().SetIsAttacked(true);
			r.velocity = Vector3.zero;
			r.useGravity = false;
			r.freezeRotation = true;
			
			sumRotation = 0;
			x = transform.rotation.x;
			y = transform.rotation.y;
			z = transform.rotation.z;
			
			transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
			transform.rotation = Camera.main.transform.rotation;
		}
    }
	
	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}

