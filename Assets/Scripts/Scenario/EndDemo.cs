using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDemo : MonoBehaviour {

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Initiate.Fade("End", Color.black, 2f);
        }
    }
}
