
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
#endregion // Namespace

namespace Com.BrumaGames.Llamaradas
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables
        public static GameController sharedInstance;

        [Tooltip("El prefab que representa al player")]
        public GameObject playerPrefab;

        [Tooltip("Prefab del BoarManager que se instanciara al iniciar la partida")]
        public GameObject boarManagerPrefab;

        [Tooltip("Prefab del turnSystemManager que se instanciara al iniciar la partida")]
        public GameObject turnSystemManager;

        public GameObject loadingScenes;

        //  POSICIONES DEL JUGADOR AL INICIAR
        int[] positionX = { 20, 140 }; // posición en X
        int[] positionY = { 30, 50, 70, 90, 110, 130 }; //posición en Y
        public bool[] initialPositionPlayer = new bool[12];


       // public int maxNumbersActions = 5;

        //  Variables Públicas
        [Tooltip("Tiempo inicial en segundo")]
        public int tiempoInicial;

        [Tooltip("Escala del tiempo del Reloj")] [Range(-10f, 10f)]
        public float escalaDeTiempo = 1;

        //[HideInInspector]
        //public int numbersActions = 0;

        //  TIEMPO 
        private float tiempoDelFrameConTimeScale = 0f;
        private float tiempoAMostrarEnSegundos = 0f;
        private float escalaDeTiempoAlPausar, escalaDeTiempoInicial;
        private bool estaPausado = false;


        //  COMPONENTES
        public TextMeshProUGUI textTimer;
        public GameObject buttonShowActions;
        public Canvas canvasActions;

        // HABITANTES DE EDIFICIOS    
        public int totalPopulation = 90;
        public int totalDisabledPerson = 40;
        public int totalPerson = 30;
        public int totalPet = 20;
        public GameObject disabledPerson;
        public GameObject person;
        public GameObject pet;


        #endregion // Variables

        // ########################################
        // MonoBehaviour Functions
        // ########################################

        #region MonoBehaviour
        void Start()
        {
            //      SINGLETON
            if (sharedInstance == null)
            {
                sharedInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }


            //INSTANCIAR BOARMANAGER
            if(boarManagerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> boarManagerPrefab Reference. Please set it up in GameObject 'Game Controller'", this);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateSceneObject(boarManagerPrefab.name, Vector3.zero, Quaternion.identity);
                }
            }

            //INSTACIAR TURNSYSTEM
            if (turnSystemManager == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> turnSystenManagerPrefabs Reference. Please set it up in GameObject 'Game Controller'", this);
            }
            else
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.InstantiateSceneObject(turnSystemManager.name, Vector3.zero, Quaternion.identity);
                }
            }

            //INSTANCIAR PLAYER
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Controller'", this);
            }
            else
            {
                if (PlayerController.LocalPlayerInstance == null)
                {
                    // instanciar player
                    bool selectPosition = false;
                    int i3 = 0;
                    Vector3 positionPlayer = new Vector3();

                    do
                    {
                        int i1 = Random.Range(0, 2);
                        int i2 = Random.Range(0, 5);
                        positionPlayer = new Vector3(positionX[i1], positionY[i2], 0);

                        if (i1 == 0)
                        {
                            i3 = i2;
                        }
                        else
                        {
                            i3 = i2 + 6;
                        }

                        if (!initialPositionPlayer[i3])
                        {
                            Debug.Log("posicion vacia");
                            selectPosition = true;
                        }else
                        {
                            Debug.Log("posicion ocupada");
                        }
                    } while (!selectPosition);                    


                    PhotonView pv = GetComponent<PhotonView>();
                    pv.RPC("PositionPlayer", RpcTarget.AllBuffered, i3);
                    PhotonNetwork.Instantiate(this.playerPrefab.name, positionPlayer, Quaternion.identity, 0);
                }
            }

            //  ACTIONS se cambio a playerController
            //numbersActions = GameManager.sharedInstance.maxNumbersActions;

            //  TIMER
            escalaDeTiempoInicial = escalaDeTiempo;                                 //  Establecer la escala de tiempo original    
            tiempoAMostrarEnSegundos = GameManager.sharedInstance.timeTurn;         //  Inicializamos la variables que acumular
            ActualizarReloj(tiempoInicial);

            //turnSystem = GameObject.FindGameObjectWithTag("TurnSystem").GetComponent<TurnSystemManager>();
            //turnSystem.StartTurnPlayer1();
        }


        void Update()
        {
            if (!estaPausado)
            {
                //  La siguiente variable representa el tiempo de cada frame considerando la escala de tiempo
                tiempoDelFrameConTimeScale = Time.deltaTime * escalaDeTiempo;

                //  La siguiente variable va acumulando el tiempo transcurrido para luego mostrarlo en el reloj
                tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale;
                ActualizarReloj(tiempoAMostrarEnSegundos);
            }
        }
        #endregion // MonoBehaviour        

        #region sincrizacionPositionPlayer

        [PunRPC]
        void PositionPlayer(int i)
        {
            initialPositionPlayer[i] = true; 
        }

        #endregion

        #region Photon Callbacks

        //Llamado cuando el jugador local salió de la sala. Necesitamos cargar la escena del lanzador.
        public override void OnLeftRoom()
        {
            loadingScenes.GetComponent<LoadingScene>().LoadLevel(1);
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        // ########################################
        // función de manejo del tiempo
        // ########################################

        #region Timer
        //  TIMER
        public void ActualizarReloj(float tiempoEnSegundos)
        {
            int minutos = 0;
            int segundos = 0;
            string textoDelReloj;

            //  Asegurar que el tiempo no sea negativo
            if (tiempoEnSegundos < 0) tiempoEnSegundos = 0;

            //  Calcular minutos y segundos
            minutos = (int)tiempoEnSegundos / 60;
            segundos = (int)tiempoEnSegundos % 60;


            // Formato con el que se ve el tiempo
            //  Crear la cadena de caracteres con 2 dígitos para los minutos y segundos, separados por  ":"
            if (minutos >= 10)
            {
                textoDelReloj = minutos.ToString("00") + ":" + segundos.ToString("00");
            }
            else if (minutos >= 1)
            {
                textoDelReloj = minutos.ToString("0") + ":" + segundos.ToString("00");
            }
            else if (segundos < 10)
            {
                textoDelReloj = segundos.ToString("0");
            }
            else
            {
                textoDelReloj = segundos.ToString("00");
            }

            //  Actualizar el elemento de text de UI con la cadena de caracteres
            textTimer.text = textoDelReloj;
        }

        public void Pausar()
        {
            if (!estaPausado)
            {
                estaPausado = true;
                escalaDeTiempoAlPausar = escalaDeTiempo;
                escalaDeTiempo = 0;
            }
        }

        public void Continuar()
        {
            if (estaPausado)
            {
                estaPausado = false;
                escalaDeTiempo = escalaDeTiempoAlPausar;
            }
        }

        public void Reiniciar()
        {
            estaPausado = false;
            escalaDeTiempo = escalaDeTiempoInicial;
            tiempoAMostrarEnSegundos = tiempoInicial;
            ActualizarReloj(tiempoAMostrarEnSegundos);
        }
        #endregion //TIMER
    }

}
