using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour {

    public float weaponDamage;
    public AudioSource audioClip;
    void OnEnable () {
        if (this.gameObject.CompareTag("SingleSword"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().weaponType = 0;
        if (this.gameObject.CompareTag("DualSword"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().weaponType = 1;
        if (this.gameObject.CompareTag("GreatSword"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().weaponType = 2;   
    }

    private void Start()
    {
        audioClip = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().isAttacking)
            GetComponent<BoxCollider>().enabled = false;
        else if (GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerMovement>().isAttacking)
            return;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            col.GetComponent<EnemyController>().RemoveHealth(weaponDamage);
            audioClip.Play();
        }
        if (col.tag == "Boss")
        {
            col.GetComponent<BossReaper>().RemoveHealth(weaponDamage);
            audioClip.Play();
        }
    }
}
