using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MenuSoundControl : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource mnuAudio;

    void Start()
    {
        mnuAudio = gameObject.GetComponent<AudioSource>();
    }

        public void buttonSound()
    {
        mnuAudio.clip = clips[0];
        mnuAudio.PlayOneShot(mnuAudio.clip);
        Debug.Log("Boton apretadoms");
    }


}
