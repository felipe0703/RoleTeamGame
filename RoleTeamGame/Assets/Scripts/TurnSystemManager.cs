using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TurnGame
{
    noTurn,
    player1,
    player2,
    fire
}

public class TurnSystemManager : MonoBehaviour
{
    public TurnGame currentTurnGame = TurnGame.noTurn;
    public static TurnSystemManager sharedInstance;
    //public int numberPlayers;
    public static int turn = 0;
    public int turnLimit = 3;

    public PlayerController playerScript;

    public GameObject avisoFuego;
    public GameObject avisoP1;
    public GameObject avisoP2;
    public GameObject avisoP3;
    public GameObject avisoP4;

    public GUIAnimFREE fuego;
    public GUIAnimFREE p1;
    public GUIAnimFREE p2;
    public GUIAnimFREE p3;
    public GUIAnimFREE p4;

    SfxControl ScriptEfSonido;

    private void Awake()
    {
        ScriptEfSonido = GameObject.Find("Sound/Efectos interaccion").GetComponent<SfxControl>();

    }

    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //modo desarrollador
        StartTurnPlayer1();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BoardManager.sharedInstance.WindGeneration();
        }
    }

    #region TurnFire

    public void StartTurnPlayer1()
    {
        SetTurnGame(TurnGame.player1);
        ScriptEfSonido.PlayTurnPlayerSound();
    }

    public void StartTurnPlayer2()
    {
        SetTurnGame(TurnGame.player2);
        ScriptEfSonido.PlayTurnPlayerSound();
    }
    public void StartTurnFire()
    {
        SetTurnGame(TurnGame.fire);
        ScriptEfSonido.PlayTurnFireSound();
        avisoFuego.SetActive(true);
        fuego.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
        fuego.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
        avisoFuego.SetActive(false);
    }


    void SetTurnGame(TurnGame newTurnGame)
    {
        if (newTurnGame == TurnGame.noTurn)
        {
            Debug.Log("Turno jugador: 0");
            currentTurnGame = TurnGame.noTurn;
        }

        if (newTurnGame == TurnGame.player1)
        {
   //     Debug.Log("Turno jugador: 1");
            currentTurnGame = TurnGame.player1;
            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GameController.sharedInstance.numbersActions = GameManager.sharedInstance.maxNumbersActions;
            controller.ActiveActions();
            controller.myTurn = true;
            GameController.sharedInstance.HidePanelEndTurn();
        }

        if (newTurnGame == TurnGame.player2)
        {
            Debug.Log("Turno jugador: 2");
            currentTurnGame = TurnGame.player2;
        }

        if (newTurnGame == TurnGame.fire)
        {
    //     Debug.Log("Turno Fuego");
            currentTurnGame = TurnGame.fire;
            GameController.sharedInstance.ShowPanelEndTurn();
            BoardManager.sharedInstance.IncreaseFire();
            BoardManager.sharedInstance.WindGeneration();
            BoardManager.sharedInstance.EdificeNeighborWithFire();
            StartTurnPlayer1();
        }
    }


    #endregion

}
