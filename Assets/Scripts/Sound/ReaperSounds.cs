using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperSounds : MonoBehaviour {

    public AudioClip death1;
    public AudioClip death2;
    public AudioClip death3;
    public AudioClip scythe;
    public AudioSource audioS;

    void IntroDeathSound()
    {
        audioS.PlayOneShot(death1);
    }
    void DeathDeathSound()
    {
        audioS.PlayOneShot(death2);
    }
    void HurtDeathSound()
    {
        audioS.PlayOneShot(death3);
    }
    void ScytheSound()
    {
        audioS.PlayOneShot(scythe);
    }
}
