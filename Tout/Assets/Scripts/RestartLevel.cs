using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {

    public GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager(Clone)").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.DestroyPreviousLevel();
        }
    }
}
