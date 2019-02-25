using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {

    public GameObject gameoverMenuUI;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (OnePlayerMovement.playerdeath)
        {
            ShowScreen();
        }
    }
    public void ShowScreen()
    {
        gameoverMenuUI.SetActive(true);
    }
    public void Restart()
    {
        OnePlayerMovement.playerdeath = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting...");
    }
}
