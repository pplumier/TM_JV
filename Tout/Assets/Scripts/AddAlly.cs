using UnityEngine;
using System.Collections;

public class AddAlly : MonoBehaviour {

    public float minDistance = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("TractAndAlly"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            GameObject go = hitInfo.transform.gameObject;
            if (go.tag == "Ally" && Vector3.Magnitude(transform.position - go.transform.position) < minDistance)
            {
                Follower f = go.GetComponent<Follower>();
                f.activate = !f.activate;
            }
        }
    }
}
