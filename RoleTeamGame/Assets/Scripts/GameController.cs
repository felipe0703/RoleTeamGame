
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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
        //int[] positionX = { 20, 140 }; // posición en X
        //int[] positionY = { 30, 50, 70, 90, 110, 130 }; //posición en Y
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
        public TextMeshProUGUI textTimerMaster;
        //public GameObject buttonShowActions;
        public Canvas canvasActions;

        // HABITANTES DE EDIFICIOS    
        public int[,] populationInEdifices = new int[36, 3];
        public int[,] populationInEdificesClient = new int[36, 3];
        public List<int> listPopulationInEdifice = new List<int>();
        int contPopulation;
        public int contDisabledPerson;
        public int contPerson;
        public int contPet;

        public static int totalPopulation = 90;
        public static int totalDisabledPerson = 40;
        public static int totalPerson = 30;
        public static int totalPet = 20;
        public GameObject disabledPerson;
        public GameObject person;
        public GameObject pet;

        public int totalBurnedEdifice = 0;
        public List<int> listScorePlayers = new List<int>();

        //public TextMeshProUGUI prueba;
        //TEST


        //botones para aumentar o disminuir fuego
        public bool buttonAddFire = false;
        public bool buttonSubtractFire = false;

        public string scoreSaved;
        public string scoreDead;

        private int optionTime;
        public int optionR_Time;
        public int optionR_TimeNum;



        #endregion // Variables

        // ########################################
        // MonoBehaviour Functions
        // ########################################

        #region MonoBehaviour

        private void Awake()
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
        }

        public override void OnEnable()
        {
            base.OnEnable();
            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }
        
        void Start()
        {
            //actualizando propiedades personalizadas
            Hashtable props = new Hashtable
            {
                {LlamaradaGame.PLAYER_LOADED_LEVEL, true}
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            optionTime = (int)PhotonNetwork.CurrentRoom.CustomProperties["TIMER"];
            optionR_Time = (int)PhotonNetwork.CurrentRoom.CustomProperties["R_TIME"];

            if (optionTime == 0)
            {
                textTimer.enabled = false;
                textTimerMaster.enabled = false;
            }
            else if (optionTime == 1)
            {
                textTimer.enabled = false;
            }

            if (optionR_Time > 0)
            {
                if (optionR_Time == 1) optionR_TimeNum = 30;
                else if (optionR_Time == 2) optionR_TimeNum = 45;
                else if (optionR_Time == 3) optionR_TimeNum = 60;
            }

            //INSTANCIAR BOARMANAGER
            if (boarManagerPrefab == null)
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
                    //lista de jugadores
                    Player[] players = PhotonNetwork.PlayerList;

                    if(players.Length > 1)
                    {
                        if (!PhotonNetwork.IsMasterClient)
                        {
                            Vector3 positionPlayer = new Vector3();

                            positionPlayer = LlamaradaGame.GetPosition(PhotonNetwork.LocalPlayer.GetPlayerNumber());

                            GameObject player = PhotonNetwork.Instantiate(this.playerPrefab.name, positionPlayer, Quaternion.identity, 0);
                        }
                    }
                    else
                    {
                        Vector3 positionPlayer = new Vector3();

                        positionPlayer = LlamaradaGame.GetPosition(PhotonNetwork.LocalPlayer.GetPlayerNumber());

                        PhotonNetwork.Instantiate(this.playerPrefab.name, positionPlayer, Quaternion.identity, 0);
                    }
                    
                }
            }


            //  TIMER - Cambiado a CronometerTimer.cs
            /*escalaDeTiempoInicial = escalaDeTiempo;                                 //  Establecer la escala de tiempo original    
            tiempoAMostrarEnSegundos = GameManager.sharedInstance.timeTurn;         //  Inicializamos la variables que acumular
            ActualizarReloj(tiempoInicial);*/


            if (PhotonNetwork.IsMasterClient)
            {
                SetPopulationInEdifice();
            }
        }


        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }


        void Update()
        {
            if(totalBurnedEdifice >= 5)
            {
                //Debug.Log("Juego terminado");
                GameOver();
                
            }
            else
            {
                //Debug.Log("aun no termina");
            }


            /*if (!estaPausado)
            {
                //  La siguiente variable representa el tiempo de cada frame considerando la escala de tiempo
                tiempoDelFrameConTimeScale = Time.deltaTime * escalaDeTiempo;

                //  La siguiente variable va acumulando el tiempo transcurrido para luego mostrarlo en el reloj
                tiempoAMostrarEnSegundos += tiempoDelFrameConTimeScale;
                ActualizarReloj(tiempoAMostrarEnSegundos);
            }*/
        }
        #endregion // MonoBehaviour        

        #region SynchronizationPopulationInEdifices
        void SetPopulationInEdifice()
        {            
            // bucle para designar los habitantes en cada edificio
            for (int i = 0; i < 36; i++)
            {
                //contadores de habitantes en el edificio
                contPopulation = 0;
                contDisabledPerson = 0;
                contPerson = 0;
                contPet = 0;

                //personas y mascotas disponibles
                int disabledPerson = totalDisabledPerson;
                int person = totalPerson;
                int pet = totalPet;
                int population = disabledPerson + person + pet;

                //asignar maximo de personas en el edificio
                int maxPopulationInEdifice = 0;

                if (population >= 3)
                {
                    maxPopulationInEdifice = 3;
                }
                else if (population == 2)
                {
                    maxPopulationInEdifice = 2;
                }
                else
                {
                    maxPopulationInEdifice = 1;
                }

                
                //TODO: ver si hay edificios sin personas
                contPopulation = Random.Range(1, maxPopulationInEdifice + 1); // personas que habrá en este edificio

                if (contPopulation > 0)
                {
                    int j = contPopulation;
                    //int j = 0;
                    do
                    {
                        int numRandom = Random.Range(0, 3);// número que asigna el habitante, (0=disablePerson, 1=person, 2=pet)

                        // TODO: confirmar si hay del habitante que salio si no intentar sacar otro
                        if (numRandom == 0 && disabledPerson > 0)
                        {
                            contDisabledPerson++;
                            //texts[j].text = "0";
                            totalDisabledPerson--;
                            j--;
                        }

                        if (numRandom == 1 && person > 0)
                        {
                            contPerson++;
                            //texts[j].text = "1";
                            totalPerson--;
                            j--;
                        }

                        if (numRandom == 2 && pet > 0)
                        {
                            contPet++;
                            //texts[j].text = "2";
                            totalPet--;
                            j--;
                        }
                        //j++;
                    } while (j > 0);
                }
                //TODO: HAY QUE ASIGNAR LOS CONTADORES DE HABITANTES A ESTE EDIFICIO

                for (int j = 0; j < 3; j++)
                {
                    int set = 0;

                    if (j == 0)
                    {
                        set = contDisabledPerson;
                    }
                    else if (j == 1)
                    {
                        set = contPerson;
                    }
                    else
                    {
                        set = contPet;
                    }

                    populationInEdifices[i, j] = set;
                    /*
                    string p1 = i.ToString() + set.ToString(); 
                    prueba.text += p1.ToString() + " - ";
                    */
                    //enviar para sincronizar con el cliente
                    PhotonView pv = GetComponent<PhotonView>();
                    pv.RPC("AddDatePopulationList", RpcTarget.AllBuffered, set);

                    //Debug.Log("Edifice: " + i + " position: " + j + " = " + populationInEdifices[i, j] + " - Disabled: " + contDisabledPerson + " Person: " + contPerson + " pet: " + contPet);
                }
            }            
        }

        [PunRPC]
        void AddDatePopulationList(int dato)
        {
            listPopulationInEdifice.Add(dato);
        }
        
        public void FillPopulationList(List<int> listPopulationInEdifice)
        {
            int contador = 0;
            for (int i = 0; i < 36; i++)
            {
                populationInEdificesClient[i, 0] = listPopulationInEdifice[0 + contador];
                populationInEdificesClient[i, 1] = listPopulationInEdifice[1 + contador ];
                populationInEdificesClient[i, 2] = listPopulationInEdifice[2 + contador ];

                contador = contador + 3;
            }
        }
        
        #endregion


        #region Photon Callbacks

        //Llamado cuando el jugador local salió de la sala. Necesitamos cargar la escena del lanzador.
        /*public override void OnLeftRoom()
        {
            loadingScenes.GetComponent<LoadingScene>().LoadLevel(1);
        }*/

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            Debug.Log("LEAVE ROOM!!!");

            GameOver();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("se desconecto un jugador");
            //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverWindow");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("salio de sala");
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {

            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("salio un jugdor de la sala");
        }

        //se llama cuando se actualiza una propiedad personalizada
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(LlamaradaGame.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
            }
        }

        #endregion

        #region Public Methods

        

        public void UpdateTotalBurnedEdifice()
        {
            totalBurnedEdifice++;
        }

        public void GameOver()
        {

            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); // busco jugador
            Debug.Log("Tu puntaje es: " + controller.PostScoreText().ToString());

            LocalScore.saved = controller.PostScoreText();
            LocalScore.dead = controller.PostDeadText();



            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverWindow");
            /* 
             controller.CallPostScorePlayer();//envio los puntajes

             Debug.Log("tamaño de la lista: " + listScorePlayers.Count);
             //imprimir puntajes
             for (int i = 0; i < listScorePlayers.Count; i++)
             {
                 //Debug.Log("tamaño de la lista: " + scorePlayers.Count);
                 Debug.Log("puntaje jugador: " + listScorePlayers[i]);
             }*/
        }


        public void CallAddFireCoach()
        {
            BoardManager.sharedInstance.ActiveAddFireCoach();
        }
        public void CallSubtractFireCoach()
        {
            BoardManager.sharedInstance.ActiveSubtractFireCoach();
        }
        public void CallChangeWind()
        {
            BoardManager.sharedInstance.ActiveChangeWind();
        }
        #endregion



        // ########################################
        // función de manejo del tiempo
        // ########################################

        #region Timer
        //  TIMER
        public void ActualizarReloj(float tiempoEnSegundos)
        {
            return;
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
            textTimerMaster.text = textoDelReloj;
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


        private void StartGame()
        {
            TurnSystemManager.sharedInstance.SetTurnInit();

            //sistema de turno iniciar


            //Comienzo del cronometro
            Hashtable props = new Hashtable
                {
                    {CronometerTimer.CronometerStartTime, (float) PhotonNetwork.Time}
                };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            if (optionR_Time > 0)
            { 
                //Comienzo del tiempo por ronda
                Hashtable props2 = new Hashtable
                    {
                        {RoundCountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props2);
            }
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(LlamaradaGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }



    }
}
