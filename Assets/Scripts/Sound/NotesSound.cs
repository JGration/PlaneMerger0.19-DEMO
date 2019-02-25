using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotesSound : MonoBehaviour {

    public AudioClip notein;
    public AudioClip noteout;
    public AudioSource audioS;

    void NoteInSound()
    {
        audioS.PlayOneShot(notein);
    }
    void NoteOutSound()
    {
        audioS.PlayOneShot(noteout);
    }
}
