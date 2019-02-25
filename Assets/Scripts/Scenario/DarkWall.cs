using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkWall : MonoBehaviour {

    public float playerDamage = 10f;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().GreaterKnockback(playerDamage);
        }
    }
}
