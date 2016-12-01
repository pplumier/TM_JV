using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectLight : MonoBehaviour {

    GameObject light;
    List<Vector3> lightIntersection;

	// Use this for initialization
	void Start () {
        light = GameObject.FindGameObjectWithTag("PlayerLight");
        lightIntersection = light.GetComponent<RayCast>().lightIntersection;
	}
	
	// Update is called once per frame
	void Update () {
        int l = lightIntersection.Count;
        Vector4[] inter = new Vector4[1000];
        l = Mathf.Min(l, 999);
        for (int i = 0; i < l; ++i)
        {
            inter[i] = lightIntersection[i];
        }
        //this.GetComponent<Renderer>().material.SetVector("_Center", lightIntersection[0]);
        this.GetComponent<Renderer>().material.SetVectorArray("_Center", inter);
        this.GetComponent<Renderer>().material.SetFloat("_Size", l);
	}
}
