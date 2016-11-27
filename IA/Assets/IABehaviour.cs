using UnityEngine;
using System.Collections;

public class IABehaviour : MonoBehaviour {
	public GameObject player;
	
	//public Transform escapePoint;
	//NavMeshAgent agent;
	
	public float maxDistance;
	
	private Rigidbody r;
	private bool onCeiling;
	private bool onPlayer;
	private float distToGround;
	public float timeOnPlayer;
	
	// Use this for initialization
	void Start () {
		//agent = GetComponent<NavMeshAgent>();
		maxDistance = 5f;
		timeOnPlayer = 5f;
		onCeiling = true;
		onPlayer = false;
		r = GetComponent<Rigidbody>();
		distToGround = GetComponent<Collider>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 pos = new Vector2(transform.position.x, transform.position.z);
		Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
		
		if (onCeiling) {
			Vector3 direction = Camera.main.transform.position - transform.position;
			
			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));
			
			if (hit.collider.CompareTag("Player")) {
				if (Vector2.Distance(pos, playerPos) < maxDistance) {
					r.useGravity = true;
					r.AddForce(direction*100);
					onCeiling = false;
				}
			}	
		}
		else if (!IsGrounded()) {
			if (onPlayer) {
				transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1;
				transform.rotation = Camera.main.transform.rotation;
				
				if (timeOnPlayer > 0) {
						timeOnPlayer -= Time.deltaTime;
				}
				else {
						r.useGravity = true;
						r.freezeRotation = false;
						onPlayer = false;
				}
			}
		}
		else {
			Vector3 direction = Camera.main.transform.position - transform.position;
			
			RaycastHit hit;	
			Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, player.transform.position));
			
			if (!hit.collider.CompareTag("Player")) {
				Destroy(gameObject);
			}
			else {
				//agent.SetDestination(escapePoint.position);
			}
		}
	}
	
	void OnCollisionEnter(Collision col) 
	{
		if (col.gameObject.CompareTag("Player")) 
		{
			onPlayer = true;
			r.velocity = Vector3.zero;
			r.useGravity = false;
			r.freezeRotation = true;
		}
    }
	
	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
