using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateBox : MonoBehaviour {

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            anim.SetTrigger("Activate");
        }
    }
    private void OnTriggerExit(Collider col)
    {
        anim.SetTrigger("Deactivate");
    }
}
