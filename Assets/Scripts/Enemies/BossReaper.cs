using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReaper : MonoBehaviour {

    public static bool defeated = false;
    public float health = 500f;
    public float atkrange = 2f;
    public float max = 500f;
    private bool death = false;
    public float bossContactDamage = 10f;
    public static bool EndMusic;
    public Animator Master_anim;
    private Animator anim;
    private bool triggeredOnce = false;
    private bool deathPlayed;
    private bool invincible;
    public static int bossscore = 0;
    public SimpleHealthBar bossBar;
    public GameObject BossHealthBar;
    private Transform player;
    [SerializeField] private int choosingMeleeAttack, choosingRangedAttack;
    [SerializeField] private float distancePlayer;
    [SerializeField] private bool awake = false;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        anim = GetComponent<Animator>();
        deathPlayed = false;
        defeated = false;
    }

    public void RemoveHealth(float amount)
    {
        if (!invincible)
        {
            health -= amount;
            bossBar.UpdateBar(health, max);            
            StartCoroutine(HurtFrames());
            if (health <= 0)
                death = true;
        }
        
    }

    IEnumerator HurtFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(0.4f);
        invincible = false;
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<OnePlayerMovement>().RemoveHealth(bossContactDamage);
        }
    }

    void Update () {
        if(BossTrigger.bossappear)
        {

            if(!triggeredOnce)
            {
                triggeredOnce = true;
                BossHealthBar.SetActive(true);
                awake = true;
            }
        }
        if (death)
        {
            defeated = true;
            Master_anim.SetTrigger("Death");
            if (!deathPlayed)
                anim.Play("death");
            else if (deathPlayed)
                return;
        }
        else if (!death)
        {
            Vector3 targetPosition = new Vector3(player.position.x, this.transform.position.y, player.position.z);
            this.transform.LookAt(targetPosition);
            if (awake)
                Master_anim.SetTrigger("Awake");
            else
                return;            
        }
        if (defeated)
            BossTrigger.opendoor = true;
	}

    public void deathAnim()
    {
        deathPlayed = true;
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        bossscore = 1;
        Destroy(BossHealthBar);
    }
    public void NormalMusic()
    {
        EndMusic = true;
    }
}
