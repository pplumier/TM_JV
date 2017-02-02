using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerDeath : MonoBehaviour {

    bool dead;
    float ToD;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (dead && Time.realtimeSinceStartup - ToD > 5f)
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }   
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hi");
        if(collision.gameObject.tag == "IA" && !dead)
        {
            dead = true;
            ToD = Time.realtimeSinceStartup;
            GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            Debug.Log("Player Died");
        }     
    }
}
