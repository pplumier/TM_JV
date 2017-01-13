using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectLight : MonoBehaviour {

    GameObject plight;
    List<Vector3> lightIntersection;

	// Use this for initialization
	void Start () {
        plight = GameObject.FindGameObjectWithTag("PlayerLight");
        lightIntersection = plight.GetComponent<RayCast>().lightIntersection;
	}
	
	// Update is called once per frame
	void Update () {
        int l = lightIntersection.Count;
        Vector4[] inter = new Vector4[1000];
        int count = 0;
        for (int i = 0; i < l; ++i)
        {
            inter[count] = lightIntersection[i];
            count += 1;
        }
        GameObject[] olights = GameObject.FindGameObjectsWithTag("LightStick");
        for (int k = 0; k < olights.Length; ++k)
        {
            lightIntersection = olights[k].GetComponent<RayCast>().lightIntersection;
            l = lightIntersection.Count;
            for (int i = 0; i < l && count < 999; ++i)
            {
                inter[count] = lightIntersection[i];
                count += 1;
            }
        }
        //this.GetComponent<Renderer>().material.SetVector("_Center", lightIntersection[0]);
        this.GetComponent<Renderer>().material.SetVectorArray("_Center", inter);
        this.GetComponent<Renderer>().material.SetFloat("_Size", l);
	}
}
