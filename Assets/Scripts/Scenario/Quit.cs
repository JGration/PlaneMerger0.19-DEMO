using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

	// Use this for initialization
	void Update () {
        StartCoroutine(Ending());
	}

    IEnumerator Ending()
    {

        yield return new WaitForSeconds(1.0f);
        if(Input.anyKey)
        {
            Application.Quit();
            Debug.Log("Quitting...");
        }
    }
}
