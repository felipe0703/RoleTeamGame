using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

    public class TurnSystemManager : MonoBehaviourPunCallbacks
    {
        public TurnGame currentTurnGame = TurnGame.noTurn;

        public static TurnSystemManager sharedInstance;
        public static int turn = 0;
        public int playerTurn;
        int turnLimit;
        int turnInit;

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

        // marco

        public GameObject[] avisos;

        public GUIAnimFREE turno;

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
        }


        #region PUN CALLBACKS

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(LlamaradaGame.PLAYER_TURN))
            {
                Player[] players = PhotonNetwork.PlayerList;

                //notificacion cambio de turno
                UIManagerGame.sharedInstance.AnimationChangeTurn();

                if (targetPlayer.ActorNumber == players.Length  && !(bool)targetPlayer.CustomProperties[LlamaradaGame.PLAYER_TURN])
                {
                    //Debug.Log("se modifico el turno: " + targetPlayer.ActorNumber);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        BoardManager.sharedInstance.CallIncreaseFire();
                        BoardManager.sharedInstance.CallEdificeNeighborWithFire();
                        BoardManager.sharedInstance.CallWindGeneration();
                    }
                    UIManagerGame.sharedInstance.AnimationAdvanceOfFire();
                }
                return;
            }            
        }

        #endregion



        #region Turn

        public void SetTurnInit()
        {
            Player[] players = PhotonNetwork.PlayerList;
            if (PhotonNetwork.IsMasterClient)
            {
                if (players.Length > 1)
                {
                    players[1].SetCustomProperties(
                            new Hashtable{
                            { LlamaradaGame.PLAYER_TURN, true }
                            }
                        );
                }
                else
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(
                            new Hashtable{
                            { LlamaradaGame.PLAYER_TURN, true }
                            }
                        );
                }
            }
        }

        private void SetTurnText(int turn)
        {
            UIManagerGame.sharedInstance.textTurn.text = "Turno Jugador: " + turn;
        }

       

        IEnumerator AnimacionTurnos(){
            turno.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
            yield return new WaitForSeconds(3.1f);
            turno.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        }

    

        void DesactivarAvisos(){
            for(int i = 0; i < avisos.Length; i++){
                avisos[i].SetActive(false);
            }
        }


        /*
        public void StartTurnPlayer1()
        {
            SetTurnGame(TurnGame.player1);

            //TODO: IMPLEMENTAR LUEGO
            //controller.DetectFireLevel(); //Para cambiar el sonido si el fuego del edificio cercano avanzó
            /*
            DesactivarAvisos();
            avisos[1].SetActive(true);
            StartCoroutine(AnimacionTurnos());
    
        }
    */
        public void StartTurnFire()
        {
            //SetTurnGame(TurnGame.fire);

            //TODO: IMPLEMENTAR TODO ESTO
           /* avisoFuego.SetActive(true);
            fuego.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
            fuego.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
            avisoFuego.SetActive(false);*/
            /*
            DesactivarAvisos();
            avisos[0].SetActive(true);
            turno.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
            turno.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);*/

        }
        
        #endregion

    }

}
