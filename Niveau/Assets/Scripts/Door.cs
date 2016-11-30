using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private void OnTriggerEnter(Collider obj)
    {
        GetComponent<Animation>().Play("open");
    }

    private void OnTriggerExit(Collider obj)
    {
        GetComponent<Animation>().Play("close");
    }
}
