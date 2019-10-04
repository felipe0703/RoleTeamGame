using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxControl : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource fxAudio;
    int edificeType;

    // Start is called before the first frame update
    void Start()
    {
        fxAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayClickSound()
    {
        fxAudio.PlayOneShot(clips[3]);
    }

    public void PlayHiClickSound()
    {
        fxAudio.PlayOneShot(clips[4]);
    }

    public void PlayCommunicationsFx()
    {
        fxAudio.PlayOneShot(clips[2], 0.3f);
    }

    public void PlayDoorFx(int id)
    {
        fxAudio.PlayOneShot(clips[0], 0.3f);
        /*
        if (id < 3)
        {
            fxAudio.PlayOneShot(clips[0], 0.5f);
            return;
        }
        if (id == 3)
        {
            fxAudio.PlayOneShot(clips[1], 0.5f);
            return;
        }*/
    }
    /*
    public void DetectEdifice(GameObject edifice)
    {
        if (edifice.GetComponent<Edifice>().id != 3)
        {
            edificeType = 1;
            return;
        }
        else
        {
            edificeType = 2;
            return;
        }
    }
    */

}
