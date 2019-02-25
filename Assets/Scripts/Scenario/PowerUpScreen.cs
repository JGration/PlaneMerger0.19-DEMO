using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScreen : MonoBehaviour {

    public GameObject ShadowDash;
    public GameObject RangedSlash;
    public GameObject BlackPanel;

    void Update () {
		if(GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().powerupReceiver == 1)
        {
            Time.timeScale = 0f;
            ShadowDash.SetActive(true);
            BlackPanel.SetActive(true);
            StartCoroutine(CloseDash());
        }
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().powerupReceiver == 2)
        {
            Time.timeScale = 0f;
            RangedSlash.SetActive(true);
            BlackPanel.SetActive(true);
            StartCoroutine(CloseSlash());
        }
    }
    IEnumerator CloseDash()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        if(Input.anyKeyDown)
        {
            ShadowDash.SetActive(false);
            BlackPanel.SetActive(false);
            Time.timeScale = 1.0f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().powerupReceiver = 0;
            yield return 0;
        }        
    }
    IEnumerator CloseSlash()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        if (Input.anyKeyDown)
        {
            RangedSlash.SetActive(false);
            BlackPanel.SetActive(false);
            Time.timeScale = 1.0f;
            GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().powerupReceiver = 0;
            yield return 0;
        }
    }
}
