using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoves : MonoBehaviour {

    public Animator Boss_animator;
    public float atkrange = 3f;
    public static bool playBossMusic;
    [SerializeField] private Transform playerTarget;
    [SerializeField] private Vector3 playerTargetOldPos, BossSpell, direction;
    public Transform SpawnBalls;
    [SerializeField] private int choosingMeleeAttack, choosingRangedAttack;
    [SerializeField] private float distancePlayer;
    public GameObject Hellflame, Darkskull;

    void Start() {
        playerTarget = GameObject.Find("PlayerTarget").GetComponent<Transform>();
        StartCoroutine(GetPlayerOldPos());
        BossSpell = SpawnBalls.position;
    }

    // Update is called once per frame
    void Update() {
        distancePlayer = Vector3.Distance(transform.position, playerTarget.position);
        if (SpawnBalls != null)
            BossSpell = SpawnBalls.position;
        else
            return;
        direction = (playerTargetOldPos - BossSpell);
    }

    public void MoveAnimation()
    {
        Boss_animator.SetBool("Move", true);
    }
    public void IntroAnimation()
    {
        Boss_animator.SetTrigger("Intro");
    }
    public void AttackAnimation()
    {
        if (distancePlayer < atkrange)
        {
            choosingMeleeAttack = Random.Range(1, 3);
            Boss_animator.SetTrigger("Attack" + choosingMeleeAttack);
            Boss_animator.SetBool("Move", false);
        }
        if (distancePlayer >= atkrange)
        {
            choosingRangedAttack = Random.Range(3, 5);
            if (choosingRangedAttack == 3)
            {
                Boss_animator.SetTrigger("Attack3");
                Boss_animator.SetBool("Move", false);
                Invoke("HellflameSpawn", 1f);
            }
            if (choosingRangedAttack == 4)
            {
                Boss_animator.SetTrigger("Attack4");
                Boss_animator.SetBool("Move", false);
                Invoke("DarkskullSpawn", 1.5f);
            }
        }
    }

    void HellflameSpawn ()
    {
        GameObject.Instantiate(Hellflame, playerTargetOldPos, Hellflame.transform.rotation);
    }
    void DarkskullSpawn()
    {
        GameObject sk = (GameObject)GameObject.Instantiate(Darkskull, BossSpell,
        Darkskull.transform.rotation);
        sk.transform.forward = direction;
    }
    IEnumerator GetPlayerOldPos()
    {
        while(true)
        {
            playerTargetOldPos = playerTarget.position;
            yield return new WaitForSeconds(1.0f);
        }
    }

    void PlayMusic()
    {
        playBossMusic = true;
    }
}
