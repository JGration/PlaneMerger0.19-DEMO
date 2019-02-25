using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChange : MonoBehaviour {

    public AudioClip normalMusic;
    public AudioClip bossMusic;

    private AudioSource aS;

    void Start () {
        aS = GetComponent<AudioSource>();
        aS.loop = true;
        aS.PlayOneShot(normalMusic);
    }

	void Update () {
		if(BossMoves.playBossMusic)
        {
            BossAppears();
            BossMoves.playBossMusic = false;
        }
        if(BossReaper.EndMusic)
        {
            BossDead();
            BossReaper.EndMusic = false;
        }
	}
    public void BossAppears()
    {
        aS.Stop();
        aS.loop = true;
        aS.PlayOneShot(bossMusic);
    }
    public void BossDead()
    {
        aS.Stop();
        aS.loop = true;
        aS.PlayOneShot(normalMusic);
    }

}
