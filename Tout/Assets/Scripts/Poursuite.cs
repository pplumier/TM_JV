using UnityEngine;
using System.Collections;

public class Poursuite : MonoBehaviour {
    UnityEngine.AI.NavMeshAgent nm;
    SeeLight sl;
    Vector3 lastPosition;
    public Vector3 randomPosition;
    float lastObserved;
    Vector3 lastUpdatePosition;

	// Use this for initialization
	void Start () {
        lastPosition = transform.position;
        nm = GetComponent<UnityEngine.AI.NavMeshAgent>();
        sl = GetComponent<SeeLight>();
        lastObserved = -100f;
        randomPosition = Vector3.zero;
        lastUpdatePosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if(sl.seePLight){
            nm.SetDestination(sl.plight.transform.position);
            lastPosition = sl.plight.transform.position;
            lastObserved = Time.fixedTime;
        }
        else if (sl.seeOLight && sl.olight != null)
        {
            nm.SetDestination(sl.olight.transform.position);
            lastPosition = sl.olight.transform.position;
            lastObserved = Time.fixedTime;
        }
        else
        {
            if (Time.fixedTime - lastObserved < 5f)
                nm.SetDestination(lastPosition);
            else if (randomPosition == Vector3.zero)
            {
                while (true)
                {
                    Vector3 randomDirection = Random.insideUnitSphere * 100;
                    UnityEngine.AI.NavMeshHit hit;
                    randomDirection.y = 0;
                    UnityEngine.AI.NavMesh.SamplePosition(transform.position + randomDirection, out hit, int.MaxValue, 1);
                    randomPosition = hit.position;
                    if (Vector3.Magnitude(transform.position + randomDirection - randomPosition) < 1f)
                    {
                        nm.SetDestination(randomPosition);
                        break;
                    }
                }

            }
            else if (Vector3.Distance(transform.position, randomPosition) < 3f)
                randomPosition = Vector3.zero;
            else if (lastUpdatePosition == transform.position)
                randomPosition = Vector3.zero;

            lastUpdatePosition = transform.position;
        }
            
	}
}
