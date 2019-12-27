using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlPruebaMusica : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public grumbleAMP musicScript;
    private AudioSource musicSrc;
    public bool visibleCount;
    public GameObject canvas;
    GameObject timerText;
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

        if (visibleCount)
        {
            timerText = canvas.transform.Find("ParteSuperior/PanelSegundos/Numero").gameObject;
            textMesh = timerText.GetComponent<TextMeshProUGUI>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (musicScript.isPlaying())
        {
            Debug.Log("Esta sonandooo");
        }
        if (!musicScript.isPlaying())
        {
            Debug.Log("No esta sonandooo");
        }
        if (visibleCount)
        {
            textMesh.text = Math.Truncate(timeLeft).ToString();
        }
    }

    IEnumerator IdleMusicTimer()
    {
        while (true)
        {
            if (!musicScript.isPlaying())
            {
                if (visibleCount)
                {
                    timeLeft -= Time.deltaTime;
                }
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
        Debug.Log("Reproduciendo música de reposo: "+song);
        musicScript.PlaySong(song, 0, 0);
     /*   timeLeft = UnityEngine.Random.Range(minDelay, maxDelay);
        resetTimer = false;
        */
    }
}
