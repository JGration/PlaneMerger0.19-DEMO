using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsCanvas : MonoBehaviour {

    private Image controls;
    private void Start()
    {
        controls = GetComponent<Image>();
        controls.enabled = false;
    }
    void Update () {
		if(ShowControls.showButtons)
            controls.enabled = true;
        else
            controls.enabled = false;
    }
}
