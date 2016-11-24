﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RewardRotation : MonoBehaviour {

    public Text message;
    private Text nouveau;

    void Start()
    {
        message.text = "";
        nouveau = Instantiate(message) as Text;
        nouveau.transform.SetParent(GameObject.Find("Goals").transform);
        //nouveau.transform.SetParent(GameObject.Find("Canvas").transform);
        nouveau.rectTransform.localPosition = new Vector3(0f, 0f, 0f);
    }

    void Update()
    {
        transform.Rotate(new Vector3(60, 60, 60) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            message.text = ":-D\nYou WIN!!!";
            nouveau.text = ":-D\nYou WIN!!!";
            gameObject.SetActive(false);
        }
    }
}
