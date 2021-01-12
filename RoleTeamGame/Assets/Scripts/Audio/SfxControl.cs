using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxControl : MonoBehaviour
{
    public AudioClip fireTurnSound;
    public AudioClip playerTurnSound;
    public AudioClip windTurnSound;
    public AudioClip clickSound;
    public AudioClip hiClickSound;
    public AudioClip specialClickSound;
    public AudioClip doorFx;
    public AudioClip Point1Sound;
    public AudioClip Point2Sound;
    public AudioClip Point3Sound;
    public AudioClip DeathSound;
    public AudioClip[] stepSoundEffects;
    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;
    private AudioSource fxAudio;


    void Start()
    {
        fxAudio = gameObject.GetComponent<AudioSource>();
    }

    // SONIDO PASOS JUGADOR

    public void PlayOneStepSound()
    {
        int randomIndex = Random.Range(0, stepSoundEffects.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        fxAudio.pitch = randomPitch;
        fxAudio.PlayOneShot(stepSoundEffects[randomIndex]);
        fxAudio.pitch = 1.0f;
    }

    // SONIDOS TURNOS

    public void PlayTurnFireSound() => fxAudio.PlayOneShot(fireTurnSound);

    public void PlayTurnPlayerSound() => fxAudio.PlayOneShot(playerTurnSound);

    public void PlayTurnWindSound() => fxAudio.PlayOneShot(windTurnSound);

    //SONIDOS UI

    public void PlayClickSound() => fxAudio.PlayOneShot(clickSound);

    public void PlayHiClickSound() => fxAudio.PlayOneShot(hiClickSound);

    public void PlaySpecialClickSound() => fxAudio.PlayOneShot(specialClickSound);

    public void PlayDoorFx() => fxAudio.PlayOneShot(doorFx, 0.08f);

    //SONIDOS PUNTAJES

    public void PlayPoint1Sound() => fxAudio.PlayOneShot(Point1Sound);

    public void PlayPoint2Sound() => fxAudio.PlayOneShot(Point2Sound);

    public void PlayPoint3Sound() => fxAudio.PlayOneShot(Point3Sound);

    public void PlayDeathSound() => fxAudio.PlayOneShot(DeathSound);

}
