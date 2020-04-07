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

        /*public GameObject avisoFuego;
        public GameObject avisoP1;
        public GameObject avisoP2;
        public GameObject avisoP3;
        public GameObject avisoP4;

        public GUIAnimFREE fuego;
        public GUIAnimFREE p1;
        public GUIAnimFREE p2;
        public GUIAnimFREE p3;
        public GUIAnimFREE p4;*/

        //Marco: esto es para guardar el componente del objeto Text_turn que esta en el uimanager
        private GUIAnimFREE textoTurno;

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
            // se consigue el componente del text_turn
            textoTurno = UIManagerGame.sharedInstance.textTurn.GetComponent<GUIAnimFREE>();
        }


        #region PUN CALLBACKS

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(LlamaradaGame.PLAYER_TURN))
            {
                Player[] players = PhotonNetwork.PlayerList;

                //notificacion cambio de turno
                UIManagerGame.sharedInstance.AnimationChangeTurn();
                SetTurnText(targetPlayer.NickName);

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

                if (GameController.sharedInstance.optionR_Time > 0)
                {
                    Debug.Log("-----> REINICIAR TIMER");
                    Hashtable props2 = new Hashtable
                    {
                        {RoundCountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props2);
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
                    SetTurnText(players[1].NickName);
                    players[1].SetCustomProperties(
                            new Hashtable{
                            { LlamaradaGame.PLAYER_TURN, true }
                            }
                        );
                }
                else
                {
                    SetTurnText(PhotonNetwork.LocalPlayer.NickName);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(
                            new Hashtable{
                            { LlamaradaGame.PLAYER_TURN, true }
                            }
                        );
                }
            }
        }



        //Use este metodo para los mensajes de turno, llama a la corutina para activar las animaciones
        //Faltaria avusar el fuego

        //en cuanto a lo del viento, tengo una sugerencia para que se note hacia a donde va,preguntando, me dijieron 
        //que se podria agregar una segunda camara que se activa para que muestre en el centro de la pantalla la brujula
        //con zoom para que se note
        private void SetTurnText(string turn)
        {
            //UIManagerGame.sharedInstance.textTurn.text = "Turno Jugador: " + turn;
            UIManagerGame.sharedInstance.textTurnStatic.enabled = true;
            UIManagerGame.sharedInstance.textTurnStaticMaster.enabled = true;
            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                    UIManagerGame.sharedInstance.textTurnStaticMaster.text = "Turno: " + turn;
                else
                    UIManagerGame.sharedInstance.textTurnStatic.text = "Turno: " + turn;
            }
            else
                UIManagerGame.sharedInstance.textTurnStatic.text = "Turno: " + turn;

            StartCoroutine(AnimacionTurnos());
        }

       

        IEnumerator AnimacionTurnos(){
            textoTurno.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
            yield return new WaitForSeconds(3.1f);
            textoTurno.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
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
