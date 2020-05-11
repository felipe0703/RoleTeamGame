using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxControl : MonoBehaviour
{

    public AudioClip gateFx;
    public AudioClip doorFx;
    public AudioClip communicationsFx;
    public AudioClip clickSound;
    public AudioClip hiClickSound;
    public AudioClip fireTurnSound;
    public AudioClip playerTurnSound;
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

    public void PlayTurnFireSound()
    {
        fxAudio.PlayOneShot(fireTurnSound);
    }
    public void PlayTurnPlayerSound()
    {
        fxAudio.PlayOneShot(playerTurnSound);
    }

    public void PlayClickSound()
    {
        fxAudio.PlayOneShot(clickSound);
    }

    public void PlayHiClickSound()
    {
        fxAudio.PlayOneShot(hiClickSound);
    }

    public void PlayCommunicationsFx()
    {
        fxAudio.PlayOneShot(communicationsFx, 0.3f);
    }

    public void PlayDoorFx(int id)
    {
        fxAudio.PlayOneShot(doorFx, 0.3f);
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
