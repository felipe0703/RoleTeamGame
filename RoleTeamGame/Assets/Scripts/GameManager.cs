using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // AUDIO
    AudioSource audioSource;
    [Space(10)]
    [Header("Sound")]
    public AudioClip[] stepSoundEffects;
    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOneStepSound()
    {
        int randomIndex = Random.Range(0, stepSoundEffects.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        audioSource.pitch = randomPitch;
        audioSource.clip = stepSoundEffects[randomIndex];
        audioSource.Play();
    }
}
