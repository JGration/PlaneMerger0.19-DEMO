using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowControls : MonoBehaviour {

    public static bool showButtons = false;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            showButtons = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        showButtons = false;
    }
}
