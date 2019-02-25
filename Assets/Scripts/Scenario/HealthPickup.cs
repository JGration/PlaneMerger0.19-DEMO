using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour {

    public SimpleHealthBar healthBar;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            OnePlayerMovement.health += 50;
            healthBar.UpdateBar(OnePlayerMovement.health, OnePlayerMovement.max);
            Destroy(gameObject);
        }
    }
}
