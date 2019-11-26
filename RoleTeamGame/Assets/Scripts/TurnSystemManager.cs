using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.BrumaGames.Llamaradas
{
    public enum TurnGame
    {
        noTurn,
        player1,
        player2,
        player3,
        player4,
        player5,
        player6,
        player7,
        player8,
        fire
    }

    public class TurnSystemManager : MonoBehaviour
    {
        public TurnGame currentTurnGame = TurnGame.noTurn;

        public static TurnSystemManager sharedInstance;
        public static int turn = 0;
        public int playerTurn;
        int turnLimit;

        GameObject player;
        PlayerController controller;

        public GameObject[] players;
        public List<PlayerController> controllers = new List<PlayerController>();
        public bool[] turnBoolsPlayer = new bool[8];
        PhotonView pv;

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

        // Start is called before the first frame update
        void Start()
        {
            if (sharedInstance == null)
            {
                sharedInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            turn = 1;
            DetectedPlayers();
            WhoseTurnIsIt(turn);

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                pv = player.GetComponent<PhotonView>();

                if (pv.IsMine)
                {
                    controller = player.GetComponent<PlayerController>();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            //cuanta cuantos jugadores hay conectado en la sala y determinas la cantidad de turnos
            turnLimit = PhotonNetwork.CurrentRoom.PlayerCount;

            DetectedPlayers();

            SetTurnText(turn); // muestra en pantalla en que turno estan
            Debug.Log("Turno: " + turn);
            //Debug.Log("Limite de turno: " + turnLimit);

            if (turn > turnLimit)
            {
                turn = 1;
                if (PhotonNetwork.IsMasterClient)
                {
                    StartTurnFire();
                }
            }   
        }

        #region Turn

        private void SetTurnText(int turn)
        {
            UIManagerGame.sharedInstance.textTurn.text = "Turno Jugador: " + turn;
        }

       

        public void StartTurnPlayer1()
        {
            SetTurnGame(TurnGame.player1);

            //TODO: IMPLEMENTAR LUEGO
            //controller.DetectFireLevel(); //Para cambiar el sonido si el fuego del edificio cercano avanzó
        }

        public void StartTurnPlayer2()
        {
            SetTurnGame(TurnGame.player2);
        }
        public void StartTurnPlayer3()
        {
            SetTurnGame(TurnGame.player3);
        }
        public void StartTurnPlayer4()
        {
            SetTurnGame(TurnGame.player4);
        }
        public void StartTurnPlayer5()
        {
            SetTurnGame(TurnGame.player5);
        }
        public void StartTurnPlayer6()
        {
            SetTurnGame(TurnGame.player6);
        }
        public void StartTurnPlayer7()
        {
            SetTurnGame(TurnGame.player7);
        }
        public void StartTurnPlayer8()
        {
            SetTurnGame(TurnGame.player8);
        }
        public void StartTurnFire()
        {
            SetTurnGame(TurnGame.fire);

            //TODO: IMPLEMENTAR TODO ESTO
           /* avisoFuego.SetActive(true);
            fuego.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
            fuego.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
            avisoFuego.SetActive(false);*/
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
                currentTurnGame = TurnGame.player1;
                playerTurn = 1;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player2)
            {
                currentTurnGame = TurnGame.player2;
                playerTurn = 2;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player3)
            {
                currentTurnGame = TurnGame.player3;
                playerTurn = 3;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player4)
            {
                currentTurnGame = TurnGame.player4;
                playerTurn = 4;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player5)
            {
                currentTurnGame = TurnGame.player5;
                playerTurn = 5;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player6)
            {
                currentTurnGame = TurnGame.player6;
                playerTurn = 6;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player7)
            {
                currentTurnGame = TurnGame.player7;
                playerTurn = 7;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.player8)
            {
                currentTurnGame = TurnGame.player8;
                playerTurn = 8;
                SetTurnPlayer(playerTurn);
            }

            if (newTurnGame == TurnGame.fire )
            {
                Debug.Log("Turno Fuego");
                if (PhotonNetwork.IsMasterClient)
                {
                    currentTurnGame = TurnGame.fire;
                    BoardManager.sharedInstance.CallIncreaseFire();
                    BoardManager.sharedInstance.CallWindGeneration();
                    BoardManager.sharedInstance.CallEdificeNeighborWithFire();
                }               
            }
        }

        //Método que pregunto de quien es el turno
        public void WhoseTurnIsIt(int turn)
        {
            switch (turn)
            {
                case 1:
                    StartTurnPlayer1();
                    break;
                case 2:
                    StartTurnPlayer2();
                    break;
                case 3:
                    StartTurnPlayer3();
                    break;
                case 4:
                    StartTurnPlayer4();
                    break;
                case 5:
                    StartTurnPlayer5();
                    break;
                case 6:
                    StartTurnPlayer6();
                    break;
                case 7:
                    StartTurnPlayer7();
                    break;
                case 8:
                    StartTurnPlayer8();
                    break;

                default:
                    break;
            }
        }
        void SetTurnPlayer(int playerturn)
        {
            //buscar el player que necesito en el turno
            int i = 0;
            while(controllers[i].photonView.ControllerActorNr != playerturn)
            {
                i++;
            }
            // verifica que es el player que corresponde al turno y activo su turno
            if (controllers[i].photonView.ControllerActorNr == playerturn)
            {
                if (!controllers[i].myTurn)
                {
                    controllers[i].ActiveActions();
                }
            }
            else
            {
                Debug.Log("algo paso");
            }
        }

        //meotdo que pregunto si llegaste al ultimo turno o no
        public void ExceedTurnLimit()
        {
            if(turn <= turnLimit)
            {
                WhoseTurnIsIt(turn);
            }
            else
            {
                WhoseTurnIsIt(1);
            }
        }
        #endregion


        #region DetectedPlayers
        // detecta los jugadores hay en la sala
        void DetectedPlayers()
        {
            players = GameObject.FindGameObjectsWithTag("Player");

            // borro todos los elementos de la lista 
            controllers.RemoveRange(0, controllers.Count);

            // agrego elementos a la lista
            for (int i = 0; i < players.Length; i++)
            {
                controllers.Add(players[i].GetComponent<PlayerController>());
                //controller.numbersActions = GameManager.sharedInstance.maxNumbersActions;
                //controller.myTurn = false;
            }

            for (int i = 0; i < players.Length; i++)
            {
                if(players[i] != null)
                    turnBoolsPlayer[i] = players[i].GetComponent<PlayerController>().myTurn;
            }

            //TODO:  crear metodo que ordene a los players



        }
        #endregion
    }

}
