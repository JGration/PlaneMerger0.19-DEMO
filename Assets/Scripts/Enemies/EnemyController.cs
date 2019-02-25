using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    public float health = 100f;
    public bool damage = false;
    private bool death = false;
    private bool invincible = false;
    public float HitDamage = 20f;
    public static int zombiescore = 0;
    [SerializeField] private float MoveSpeed = 1.7f;
    [SerializeField] private float speedmax = 1.7f;
    public float atkrange = 1.2f;
    private Transform player;
    public Transform knockbackPos;
    NavMeshAgent agent;
    private Animator anim;

	void Start () {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        //gameObject.SetActive(false);
        health = 100f;
    }

    public void RemoveHealth(float amount)
    {
        if (!invincible)
        {
            health -= amount;
            if (health <= 0)
                death = true;
            StartCoroutine(HurtFrames());
        }
    }
    IEnumerator HurtFrames()
    {
        invincible = true;
        anim.Play("Hurt");
        transform.position = Vector3.MoveTowards(transform.position, knockbackPos.position, 8 * Time.deltaTime);
        yield return 0;
        transform.position = Vector3.MoveTowards(transform.position, knockbackPos.position, 8 * Time.deltaTime);
        yield return 0;
        transform.position = Vector3.MoveTowards(transform.position, knockbackPos.position, 8 * Time.deltaTime);
        yield return 0;
        invincible = false;
        transform.position = Vector3.MoveTowards(transform.position, knockbackPos.position, 8 * Time.deltaTime);
        yield return 0;
        transform.position = Vector3.MoveTowards(transform.position, knockbackPos.position, 8 * Time.deltaTime);
        yield return 0;
        
    }
    public void OnTriggerEnter(Collider col)
    {
        if(!death)
            if (col.tag == "Player")
                col.GetComponent<OnePlayerMovement>().RemoveHealth(HitDamage);
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

                    if (distance <= atkrange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        anim.SetBool("Attack", true);
                        speedmax = 0f;
                        MoveSpeed = speedmax;
                        if (OnePlayerMovement.health <= 0)
                            anim.SetTrigger("Eating");
                    }
                    else if (distance > atkrange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        anim.SetBool("Attack", false);
                        speedmax = 2f;
                        MoveSpeed = speedmax;
                        anim.SetTrigger("Walk");
                        transform.position = Vector3.MoveTowards(transform.position, player.position, MoveSpeed * Time.deltaTime);
                    }
                    else if (distance > atkrange && anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                    {
                        anim.SetBool("Attack", false);
                        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                        {
                            speedmax = 2f;
                            MoveSpeed = speedmax;
                            anim.SetTrigger("Walk");
                            transform.position = Vector3.MoveTowards(transform.position, player.position, MoveSpeed * Time.deltaTime);
                        }
                    }
                }
                if (distance > lookRadius)
                    anim.SetTrigger("Idle");
            }
        }

        if (death)
        {
            MoveSpeed = 0;
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                anim.SetBool("Death", true);
            Destroy(gameObject, 2.5f);
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
            if (yes)
                gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        zombiescore = 1;
    }

}
