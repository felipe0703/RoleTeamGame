using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void StartTurnPlayer2()
    {
        SetTurnGame(TurnGame.player2);
    }
    public void StartTurnFire()
    {
        SetTurnGame(TurnGame.fire);
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
