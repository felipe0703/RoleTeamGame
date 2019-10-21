using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public grumbleAMP musicScript;
    private AudioSource musicSrc;
    float timeLeft = 1.0f;
    public float minDelay;
    public float maxDelay;
    bool resetTimer = true;


    // Start is called before the first frame update
    void Start()
    {
        musicSrc = gameObject.GetComponent<AudioSource>();
        StartCoroutine(IdleMusicTimer());

        timeLeft = UnityEngine.Random.Range(minDelay, maxDelay);

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator IdleMusicTimer()
    {
        while (true)
        {
            if (!musicScript.isPlaying())
            {
                timeLeft -= Time.deltaTime;
                if (timeLeft < 0)
                {
                    PlayIdleMusic();
                    resetTimer = true;
                }

            }

            if (musicScript.isPlaying() && resetTimer)
            {
                timeLeft = UnityEngine.Random.Range(minDelay, maxDelay);
                resetTimer = false;
            }
            yield return null;
        }
    }

    void PlayIdleMusic()
    {
        int song = UnityEngine.Random.Range(0, 2);
        //Debug.Log("Reproduciendo música de reposo");
        musicScript.PlaySong(song, 0, 0);
    }
}