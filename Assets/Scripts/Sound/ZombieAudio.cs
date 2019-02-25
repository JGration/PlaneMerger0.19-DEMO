using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour {

    public AudioClip atk;
    public AudioClip death;
    public AudioClip hurt;
    public AudioSource audioS;

    void AtkSound()
    {
        audioS.PlayOneShot(atk);
    }
    void DeathSound()
    {
        audioS.PlayOneShot(death);
    }
    
    void HurtSound()
    {
        audioS.PlayOneShot(hurt);
    }
}

