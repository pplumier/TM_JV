using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private void OnTriggerEnter(Collider obj)
    {
        var theDoor = GameObject.FindWithTag("SF_Door");
        theDoor.GetComponent<Animation>().Play("open");
    }

    private void OnTriggerExit(Collider obj)
    {
        var theDoor = GameObject.FindWithTag("SF_Door");
        theDoor.GetComponent<Animation>().Play("close");
    }
}
