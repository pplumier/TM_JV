using UnityEngine;
using System.Collections;

public class Poursuite : MonoBehaviour {

    GameObject joueur;
    UnityEngine.AI.NavMeshAgent nm;
    SeeLight sl;
    Vector3 lastPosition;
    Vector3 randomPosition;
    float lastObserved;

	// Use this for initialization
	void Start () {
        joueur = GameObject.FindGameObjectWithTag("Player");
        lastPosition = transform.position;
        nm = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sl = GetComponent<SeeLight>();
        lastObserved = Time.fixedTime;
        randomPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if(sl.seeLight){
            nm.SetDestination(joueur.transform.position);
            lastPosition = joueur.transform.position;
            lastObserved = Time.fixedTime;
        }  
        else
        {
            if(Time.fixedTime - lastObserved < 5f)
                nm.SetDestination(lastPosition);
            else if(randomPosition == Vector3.zero)
            {
                Vector3 randomDirection = Random.insideUnitSphere * 300;
                UnityEngine.AI.NavMeshHit hit;
                randomDirection.y = 0;
                UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, int.MaxValue, 1);
                randomPosition = hit.position;
                nm.SetDestination(randomPosition);
            }
            else
            {
                nm.SetDestination(randomPosition);
                if (Vector3.Distance(transform.position, randomPosition) < 3f)
                    randomPosition = Vector3.zero;
            }
                
        }
            
	}
}
