using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour {

    public List<Vector3> lightIntersection;
    Light spotlight;
    public bool randomDirection = false;

	// Use this for initialization
	void Start () {
        if(GetComponent<Light>() != null)
            spotlight = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update () {
        lightIntersection.Clear();
        if (spotlight!= null && !spotlight.enabled)
            return;
        if(!randomDirection)
            for(float i = -0.4f; i < 0.6f; i += 0.2f)
                for (float j = -0.4f; j < 0.6f; j += 0.2f)
                {
                    Ray ray = new Ray(transform.position, transform.forward + transform.up * i + transform.right * j);
                    RaycastHit hitInfo;
                    Physics.Raycast(ray, out hitInfo);
                    lightIntersection.Add(hitInfo.point);
                }
        else
        for(int i = 0; i < 50; ++i)
            {
                Ray ray = new Ray(transform.position, Random.insideUnitSphere);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo);
                lightIntersection.Add(hitInfo.point);
                
            }
	}
}
