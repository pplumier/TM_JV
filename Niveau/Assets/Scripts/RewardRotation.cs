using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardRotation : MonoBehaviour {

    public Text message;

    void Start()
    {
        message.text = "";
    }

    void Update()
    {
        transform.Rotate(new Vector3(60, 60, 60) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log(message.text);
            message.text = ":-D\nYou WIN!!!";
            gameObject.SetActive(false);
            Debug.Log(message.text);
        }
    }
}

