using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSoundControl : MonoBehaviour
{
    private AudioSource fireAudio;
    public AudioClip[] clips;


    // Start is called before the first frame update
    void Start()
    {
        fireAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFireSound(int n)
    {
        if (n == 0)
        {
            fireAudio.Stop();
        }

        if (n == 1)
        {
            fireAudio.clip = clips[0];
            if (!fireAudio.isPlaying) { fireAudio.Play(); }
        }

        if (n == 2)
        {
            fireAudio.clip = clips[1];
            if (!fireAudio.isPlaying) { fireAudio.Play(); }
        }

        if (n > 2)
        {
            fireAudio.clip = clips[2];
            if (!fireAudio.isPlaying) { fireAudio.Play(); }
        }
    }
}
