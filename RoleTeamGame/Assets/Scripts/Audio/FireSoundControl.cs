using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSoundControl : MonoBehaviour
{

    public grumbleAMP musicScript;


    // Start is called before the first frame update
    void Start()
    {
        musicScript.PlaySong(0, 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeFireSound(int n)
    {
        if (n == 0)
        {
            musicScript.CrossFadeToNewLayer(0);
        }

        if (n == 1)
        {
            musicScript.CrossFadeToNewLayer(1);
        }

        if (n == 2)
        {
            musicScript.CrossFadeToNewLayer(2);
        }

        if (n > 2)
        {
            musicScript.CrossFadeToNewLayer(3);
        }
    }
}
