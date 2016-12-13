using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractObject : MonoBehaviour {

    public float minDistance = 10f;
    Rigidbody r;
    Vector3 lastVelocity;

    // Use this for initialization
    void Start () {
        r = GetComponentInParent<Rigidbody>();
        lastVelocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("TractAndAlly"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            GameObject go = hitInfo.transform.gameObject;
            Debug.Log(go.tag);
            if (go.tag == "Tractable" && Vector3.Magnitude(transform.position - go.transform.position) < minDistance)
            {
                Debug.Log("tract");
                go.GetComponent<Rigidbody>().AddForce((r.velocity - lastVelocity) * 1000, ForceMode.Impulse);
            }
            lastVelocity = r.velocity;
        }
    }
}
