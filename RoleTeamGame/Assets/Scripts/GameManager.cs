#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion //Namespace

public enum GameState
{
    login,
    mainMenu,
    turnPlayer1
}
public class GameManager : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public GameState currentGameState = GameState.login;
    public static GameManager sharedInstance;

    // AUDIO
    AudioSource audioSource;
    [Space(10)]
    [Header("Sound")]
    public AudioClip[] stepSoundEffects;
    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    #endregion // Variables

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour

    void Start()
    {
        // SINGLETON
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(sharedInstance);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    #endregion // Monobehaviour

    // ########################################
    // Funciones de Audio
    // ########################################

    #region Audio

    public void PlayOneStepSound()
    {
        int randomIndex = Random.Range(0, stepSoundEffects.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        audioSource.pitch = randomPitch;
        audioSource.clip = stepSoundEffects[randomIndex];
        audioSource.Play();
    }
    #endregion // Audio

    // ########################################
    // Funciones de Estados del juego
    // ########################################

    #region GameState

    public void Login()
    {
        SetGameState(GameState.login);
    }

    public void GoToMainMenu()
    {
        SetGameState(GameState.mainMenu);
    }

    void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.login)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.login;

        }
    }
    #endregion // GameState
}
