using UnityEngine;
using System.Collections;

public class AddAlly : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Fire3"))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo);
            GameObject go = hitInfo.transform.gameObject;
            if (go.tag == "Ally")
            {
                Follower f = go.GetComponent<Follower>();
                f.activate = !f.activate;
            }
        }
    }
}
