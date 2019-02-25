using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float playerDamage = 999f;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().RemoveHealth(playerDamage);
        }
    }
}
