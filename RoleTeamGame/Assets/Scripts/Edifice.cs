using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edifice : MonoBehaviour
{
    public int id;
    public GameObject btn;
    public GameObject panelInfo;

    public GameObject level1;

    //AUDIO
    private AudioSource edificeAudio;
    public AudioClip[] clips;

    public void Start()
    {
        edificeAudio = gameObject.GetComponent<AudioSource>();

    }

    public void Update()
    {
        
        if (level1.activeSelf)
        {
            edificeAudio.clip = clips[0];
        }
        /*  Como no hay más nivel de fuego que el 1 no me puedo referir a los demas

        if (level2.activeSelf)
        {
            edificeAudio.clip = clips[1];
        }
        if (level3.activeSelf)
        {
            edificeAudio.clip = clips[2];
        }
        if (level4.activeSelf)
        {
            edificeAudio.clip = clips[2];

        }
        */

        if (!edificeAudio.isPlaying)
        {
            edificeAudio.Play();
        }
    }

    public void IsSelected()
    {
        panelInfo.SetActive(true);
        btn.SetActive(false);
    }

    public void FireStart()
    {
        level1.SetActive(true);
    }

}
 