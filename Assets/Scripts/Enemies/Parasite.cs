using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Parasite : MonoBehaviour {

    public float lookRadius = 10f;
    public float health = 100f;
    public bool damage = false;
    public bool death = false;
    public bool invincible = false;
    public float zombiehit = 20f;
    public float zombiespeed = 2f;
    public float atkrange = 2.4f;
    public float speedmax = 2f;
    public float speedmin = 0.05f;

    private Transform player;
    NavMeshAgent agent;
    private Animator anim;

	void Start () {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        
        gameObject.SetActive(false);
    }

    public void RemoveHealth(float amount)
    {
        if (!invincible)
        {
            health -= amount;
            if (health <= 0)
            {
                death = true;
            }
            if(health == 100)
            {
                anim.SetTrigger("Hurt");
                zombiespeed = 0;
            }
            Invoke("resetInvulnerability", 2f);
        }
    }
    void resetInvulnerability()
    {
        invincible = false;
    }
    public void OnTriggerEnter(Collider col)
    {
        if(!death)
        {
            if (col.tag == "Player")
            {
                col.GetComponent<OnePlayerMovement>().RemoveHealth(zombiehit);
            }
        }
    }



    void FixedUpdate () {

        if (!death)
        {
            if (OnePlayerMovement.health >= 0)
            {
                Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
                this.transform.LookAt(targetPosition);

                float distance = Vector3.Distance(transform.position, player.position);

                if (distance <= lookRadius)
                {
                    anim.SetTrigger("Walk");

                    if (distance <= atkrange)
                    {
                        int randomNumber = Random.Range(1, 4);
                        anim.SetTrigger("Attack"+randomNumber);
                        zombiespeed = speedmin;
                        if (OnePlayerMovement.health <= 0)
                        {
                            anim.SetTrigger("Eating");
                        }
                    }
                    if (distance > atkrange)
                    {
                        zombiespeed = speedmax;
                        anim.SetTrigger("Walk");
                        transform.position = Vector3.MoveTowards(transform.position, player.position, zombiespeed * Time.deltaTime);
                    }
                }
                if (distance > lookRadius)
                {
                    anim.SetTrigger("Idle");
                }
            }
        }

        if (death)
        {
            zombiespeed = 0;
            anim.SetBool("Death", true);
            anim.Play("Death");
            Destroy(gameObject, 5f);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void Spawn(bool yes)
    {
        if (death)
            return;
        if (!death)
        {
            if (yes)
                Invoke("spawning", 2);
                gameObject.SetActive(true);
        }  
    }

    public void spawning()
    {
        gameObject.SetActive(true);
    }



}
