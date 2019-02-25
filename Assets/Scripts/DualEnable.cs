using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualEnable : MonoBehaviour {

    public GameObject SecondSword;
	void OnEnable () {
        SecondSword.SetActive(true);
	}
    void OnDisable() {
        SecondSword.SetActive(false);
    }
}
