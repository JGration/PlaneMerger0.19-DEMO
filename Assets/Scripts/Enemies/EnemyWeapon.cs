using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour {

    public float enemyWeaponDamage;
    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().RemoveHealth(enemyWeaponDamage);
        }
    }
}
