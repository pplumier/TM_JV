using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SeeLight : MonoBehaviour {

    public GameObject plight;
    public bool seePLight;
    public GameObject olight;
    public bool seeOLight;
    public GameObject[] olights;
    

	// Use this for initialization
	void Start () {
        plight = GameObject.FindGameObjectWithTag("PlayerLight");
        olights = GameObject.FindGameObjectsWithTag("LightStick");
	}
	
	// Update is called once per frame
	void Update () {
        List<Vector3> lightIntersection = plight.GetComponent<RayCast>().lightIntersection;
        int l = lightIntersection.Count;
        seePLight = false;
        for (int i = 0; i < l; ++i)
        {
            Vector3 dir = lightIntersection[i] - transform.position;
            float dist = dir.magnitude - 0.1f;
            dir = dir.normalized;
            if (Vector3.Dot(transform.forward, dir) < 0) // If light is behind
                continue;
            if (!Physics.Raycast(transform.position, dir, dist))
            {
                seePLight = true;
                break;
            }
        }
        olights = GameObject.FindGameObjectsWithTag("LightStick");
        seeOLight = false;
        for(int k = 0; k < olights.Length; ++k)
        {
            lightIntersection = olights[k].GetComponent<RayCast>().lightIntersection;
            l = lightIntersection.Count;
            for (int i = 0; i < l; ++i)
            {
                Vector3 dir = lightIntersection[i] - transform.position;
                float dist = dir.magnitude - 0.1f;
                dir = dir.normalized;
                if (Vector3.Dot(transform.forward, dir) < 0) // If light is behind
                    continue;
                if (!Physics.Raycast(transform.position, dir, dist))
                {
                    seeOLight = true;
                    olight = olights[k];
                    break;
                }
            }
        }
        //Debug.Log(seeLight);
    }
}
