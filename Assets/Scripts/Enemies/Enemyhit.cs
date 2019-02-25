using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyhit : MonoBehaviour {

    public float playerDamage = 20f;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().RemoveHealth(playerDamage);
        }
    }
}
