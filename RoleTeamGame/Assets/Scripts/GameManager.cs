#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion //Namespace

public enum GameState
{
    welcome,
    login,
    mainMenu,
    settingGame,
    game,
    turnPlayer1
}
public class GameManager : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public GameState currentGameState = GameState.welcome;
    public static GameManager sharedInstance;

    // SETTINGS
    public int maxNumbersActions;
    public int timeTurn;
    public byte limitPlayers = 2;
    public bool JoinRoom = false;


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

    void Awake()
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

    public void Welcome()
    {
        SetGameState(GameState.welcome);
    }
     public void Login()
    {
        SetGameState(GameState.login);
    }

    public void GoToMainMenu()
    {
        SetGameState(GameState.mainMenu);
    }
    public void GoToSettingGame()
    {
        SetGameState(GameState.settingGame);
    }
    public void GoToGame()
    {
        SetGameState(GameState.game);
    }

    public void CloseGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.welcome)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.welcome;

        }
        else if (newGameState == GameState.login)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.login;

        }
        else if (newGameState == GameState.mainMenu)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.mainMenu;

        }
        else if (newGameState == GameState.settingGame)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.settingGame;
        }
        else if (newGameState == GameState.game)
        {
            //  TODO: colocar logica del login
            currentGameState = GameState.game;

        }
        this.currentGameState = newGameState;
    }
    #endregion // GameState
}
