using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Animation>().Play("open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<Animation>().Play("close");
        }
    }
}
