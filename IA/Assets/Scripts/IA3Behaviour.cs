using UnityEngine;
using System.Collections;

public class IA3Behaviour : MonoBehaviour {
	public GameObject player;
	
	//public Transform escapePoint;
	
	public float maxDistance;
	
	private Rigidbody r;
	private bool onCeiling;
	private bool onPlayer;
	private float distToGround;
	
	// Rotation
	private float x, y, z;
	private float sumRotation;
	public float moveLimit;
	
	// Escape
	public Transform escapePosition;
	private UnityEngine.AI.NavMeshAgent agent;

	
	// Use this for initialization
	void Start () {
		maxDistance = 5f;
		onCeiling = true;
		onPlayer = false;
		r = GetComponent<Rigidbody>();
		distToGround = GetComponent<Collider>().bounds.extents.y;
		sumRotation = 0;
		agent = null;
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
				transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
				transform.rotation = Camera.main.transform.rotation;
				
				if (sumRotation < moveLimit) {
					sumRotation += Mathf.Abs(transform.rotation.x - x);
					sumRotation += Mathf.Abs(transform.rotation.y - y);
					sumRotation += Mathf.Abs(transform.rotation.z - z);
				
					x = transform.rotation.x;
					y = transform.rotation.y;
					z = transform.rotation.z;
				}
				else {
					r.useGravity = true;
					r.freezeRotation = false;
					onPlayer = false;
					player.GetComponent<StatePlayer>().SetIsAttacked(false);
				}
			}
		}
		// If the AI is on the ground
		else {
			if (agent == null) {
				agent = UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent( gameObject, "Assets/Scripts/IABehaviour.cs (30,9)", "NavMeshAgent" ) as UnityEngine.AI.NavMeshAgent;
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
				agent.SetDestination(escapePosition.position);
			}
			
		}
		
	}
	
	void OnCollisionEnter(Collision col) 
	{
		if (col.gameObject.CompareTag("Player")) 
		{
			onPlayer = true;
			player.GetComponent<StatePlayer>().SetIsAttacked(true);
			r.velocity = Vector3.zero;
			r.useGravity = false;
			r.freezeRotation = true;
			
			sumRotation = 0;
			x = transform.rotation.x;
			y = transform.rotation.y;
			z = transform.rotation.z;
		}
    }
	
	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}

