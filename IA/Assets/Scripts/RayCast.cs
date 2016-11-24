using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RayCast : MonoBehaviour {

    public List<Vector3> lightIntersection;
    Light spotlight;

	// Use this for initialization
	void Start () {
        spotlight = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        lightIntersection.Clear();
        if (!spotlight.enabled)
            return;
        for(float i = -1f; i < 1f; i += 0.1f)
            for (float j = -0.5f; j < 0.5; j += 0.1f)
            {
                Ray ray = new Ray(transform.position, transform.forward + transform.up * i + transform.right * j);
                RaycastHit hitInfo;
                Physics.Raycast(ray, out hitInfo);
                lightIntersection.Add(hitInfo.point);
            }
	}
}
