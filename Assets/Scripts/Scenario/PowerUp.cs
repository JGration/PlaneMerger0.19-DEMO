using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    public float PowerNumber;
    public static int powerupScore = 0;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().powerupReceiver = PowerNumber;
            powerupScore = 1;
            Destroy(gameObject);
        }
    }
}
