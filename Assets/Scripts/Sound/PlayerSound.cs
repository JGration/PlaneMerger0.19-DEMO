using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour {

    public AudioClip atk1;
    public AudioClip atk2;
    public AudioClip atk3;
    public AudioClip dual1;
    public AudioClip dual2;
    public AudioClip dual3;
    public AudioClip great1;
    public AudioClip great2;
    public AudioClip great3;
    public AudioClip hurt;
    public AudioClip jump;
    public AudioClip jumpdown;
    public AudioClip death;
    public AudioClip dash;
    public AudioClip shadowdash;
    public AudioClip footsteps;
    public AudioSource audioS;

    void Atk1Sound()
    {
        audioS.PlayOneShot(atk1);
    }
    void Atk2Sound()
    {
        audioS.PlayOneShot(atk2);
    }
    void Atk3Sound()
    {
        audioS.PlayOneShot(atk3);
    }
    void Dual1Sound()
    {
        audioS.PlayOneShot(dual1);
    }
    void Dual2Sound()
    {
        audioS.PlayOneShot(dual2);
    }
    void Dual3Sound()
    {
        audioS.PlayOneShot(dual3);
    }
    void Great1Sound()
    {
        audioS.PlayOneShot(great1);
    }
    void Great2Sound()
    {
        audioS.PlayOneShot(great2);
    }
    void Great3Sound()
    {
        audioS.PlayOneShot(great3);
    }
    void HurtSound()
    {
        audioS.PlayOneShot(hurt);
    }
    void JumpSound()
    {
        audioS.PlayOneShot(jump);
    }
    void FootstepsSound()
    {
        audioS.PlayOneShot(footsteps);
    }
    void JumpdownSound()
    {
        audioS.PlayOneShot(jumpdown);
    }
    void DeathSound()
    {
        audioS.PlayOneShot(death);
    }
    void DashSound()
    {
        audioS.PlayOneShot(dash);
    }
    void ShadowDashSound()
    {
        audioS.PlayOneShot(shadowdash);
    }
}
