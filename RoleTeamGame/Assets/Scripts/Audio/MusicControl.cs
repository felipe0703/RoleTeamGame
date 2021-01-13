using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioClip[] musicTracks;
    private AudioSource musicSrc;
    float timeLeft = 1.0f;
    public float minDelay;
    public float maxDelay;
    bool resetTimer = true;
    int lastSong;


    // Start is called before the first frame update
    void Start()
    {
        musicSrc = gameObject.GetComponent<AudioSource>();
        timeLeft = UnityEngine.Random.Range(minDelay, maxDelay);
        StartCoroutine(IdleMusicTimer());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator IdleMusicTimer()
    {
        while (true)
        {
            if (!musicSrc.isPlaying)
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    PlayIdleMusic();
                    resetTimer = true;
                }

            }

            if (musicSrc.isPlaying && resetTimer)
            {
                timeLeft = UnityEngine.Random.Range(minDelay, maxDelay);
                resetTimer = false;
            }
            yield return null;
        }
    }

    void PlayIdleMusic()
    {
        int song = UnityEngine.Random.Range(0, musicTracks.Length-1);
        while (song == lastSong) {song = UnityEngine.Random.Range(0, musicTracks.Length - 1); }
        musicSrc.clip = musicTracks[song];
        musicSrc.Play();
        lastSong = song;
    }
}