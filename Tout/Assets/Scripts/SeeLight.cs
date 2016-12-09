using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeeLight : MonoBehaviour {

    GameObject light;
    List<Vector3> lightIntersection;
    public bool seeLight;

	// Use this for initialization
	void Start () {
        light = GameObject.FindGameObjectWithTag("PlayerLight"); 
        lightIntersection = light.GetComponent<RayCast>().lightIntersection;
	}
	
	// Update is called once per frame
	void Update () {
        int l = lightIntersection.Count;
        seeLight = false;
        for (int i = 0; i < l; ++i)
        {
            Vector3 dir = lightIntersection[i] - transform.position;
            float dist = dir.magnitude - 0.1f;
            dir = dir.normalized;
            if (Vector3.Dot(transform.forward, dir) < 0) // If light is behind
                continue;
            if (!Physics.Raycast(transform.position, dir, dist))
            {
                seeLight = true;
                break;
            }
        }
        //Debug.Log(seeLight);
	}
}
