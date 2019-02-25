using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour {

    public static bool bossappear = false;
    public static bool opendoor = false;
    public Animator anim;

    private void Start()
    {
        opendoor = false;
        bossappear = false;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            bossappear = true;
            anim.SetTrigger("Close");
        }
    }
    private void Update()
    {
        if (opendoor)
            anim.SetTrigger("Open");
    }
}
