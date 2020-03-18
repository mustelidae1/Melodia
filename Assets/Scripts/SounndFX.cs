using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SounndFX : MonoBehaviour {
    public static SounndFX s;
    private AudioSource audio; 

    public AudioClip successSound;
    public AudioClip badSound;

    public void Start()
    {
        s = this;
        audio = this.GetComponent<AudioSource>(); 
    }

    public void playSuccess()
    {
        audio.PlayOneShot(successSound); 
    }

    public void playBad()
    {
        audio.PlayOneShot(badSound);
    }
}
