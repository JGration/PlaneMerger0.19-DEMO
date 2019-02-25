using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairsgrounded : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().isGrounded = true;
        }
    }
}
