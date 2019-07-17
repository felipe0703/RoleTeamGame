using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundControl : MonoBehaviour
{
    public string Etiqueta;
    public AudioClip[] clips;
    public float minDelay;
    public float maxDelay;
    private float delayTime;
    private int clipIndex = 0;
    private AudioSource bgAudio;

    // Start is called before the first frame update
    void Start()
    {
        bgAudio = gameObject.GetComponent<AudioSource>();
        delayTime = Random.Range(minDelay, maxDelay);
        StartCoroutine(PlaySounds()); //Para controlar el tiempo entre cada PlayOne Shot
    }

    IEnumerator PlaySounds()
    {
        while (true)
        {
            clipIndex = Random.Range(0, clips.Length);
            bgAudio.clip = clips[clipIndex];
            bgAudio.PlayOneShot(bgAudio.clip);
            yield return new WaitForSeconds(delayTime);
        }
        
    }
}

