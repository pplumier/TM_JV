using UnityEngine;
using System.Collections;

public class AddAlly : MonoBehaviour {

    public float minDistance = 5f;

    public int nbAlly = 0;

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
            Debug.Log("tag " + go.tag);
            if (go.tag == "Ally" && Vector3.Magnitude(transform.position - go.transform.position) < minDistance)
            {
                Follower f = go.GetComponent<Follower>();
                f.activate = !f.activate;
            }
        }

        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");
        nbAlly = 0;
        for(int i = 0; i < allies.Length; ++i)
        {
            nbAlly += (allies[i].GetComponent<Follower>().activate)?1:0;
        }
    }
}
