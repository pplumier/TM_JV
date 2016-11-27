using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private void OnTriggerEnter(Collider obj)
    {
        var thedoor = GameObject.FindWithTag("SF_Door");
        thedoor.GetComponent<Animation>().Play("open");
    }

    private void OnTriggerExit(Collider obj)
    {
        var thedoor = GameObject.FindWithTag("SF_Door");
        thedoor.GetComponent<Animation>().Play("close");
    }
}
