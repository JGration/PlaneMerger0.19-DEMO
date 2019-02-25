using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFloor : MonoBehaviour {

    private Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            anim.SetBool("Fade", true);
        }
    }
}
