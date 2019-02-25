using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame()
    {
        Initiate.Fade("Game", Color.black, 2f);
    }
    public void ExitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit();
    }
}
