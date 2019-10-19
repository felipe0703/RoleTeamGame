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

    public GameObject [] avisos;

    public GUIAnimFREE turno;

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
        //StartTurnPlayer1();
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
        DesactivarAvisos();
        avisos[1].SetActive(true);
        StartCoroutine(AnimacionTurnos());
        playerScript.DetectFireLevel(); //Para cambiar el sonido si el fuego del edificio cercano avanzó

    }

    IEnumerator AnimacionTurnos(){
        turno.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        yield return new WaitForSeconds(3.1f);
        turno.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
    }

    public void StartTurnPlayer2()
    {
        SetTurnGame(TurnGame.player2);
    }
    
    public void StartTurnFire()
    {
        SetTurnGame(TurnGame.fire);
        DesactivarAvisos();
        avisos[0].SetActive(true);
        turno.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
        turno.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
    }

    void DesactivarAvisos(){
        for(int i = 0; i < avisos.Length; i++){
            avisos[i].SetActive(false);
        }
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
            BoardManager.sharedInstance.IncreaseFire();
            BoardManager.sharedInstance.WindGeneration();
            BoardManager.sharedInstance.EdificeNeighborWithFire();
            StartTurnPlayer1();
        }
    }


    #endregion

}
