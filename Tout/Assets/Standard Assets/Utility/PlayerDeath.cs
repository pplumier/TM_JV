using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerDeath : MonoBehaviour {

	private bool dead;
	private float ToD;
	private Text finishText;

    // Use this for initialization
    void Start () {
		finishText = GameObject.Find("FinishText").GetComponent<Text>() as Text;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (dead) 
		{
			finishText.text = "You died!";

			if (Time.realtimeSinceStartup - ToD > 5f) {
				int scene = SceneManager.GetActiveScene ().buildIndex;
				SceneManager.LoadScene (scene, LoadSceneMode.Single);
				finishText.text = "";
			}   
		}
	}

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "IA" && !dead)
        {
            dead = true;
            ToD = Time.realtimeSinceStartup;
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Debug.Log("Player Died");
        }     
    }
}
