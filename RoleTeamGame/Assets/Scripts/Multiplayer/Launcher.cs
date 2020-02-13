using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;


namespace Com.BrumaGames.Llamaradas
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("El número máximo de jugadores por habitación. Cuando una sala está llena, no pueden unirse nuevos jugadores, por lo que se creará una nueva sala")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        [Tooltip("El número mínimo de jugadores por habitación.")]
        [SerializeField]
        private byte minPlayerPerRoom = 1;

        [Tooltip("El panel de la interfaz de usuario para permitir al usuario conectarse y jugar")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("El panel de la interfaz de usuario para permitir al usuario volver al menu")]
        [SerializeField]
        private GameObject buttonMenu;

        [Tooltip("La etiqueta UI para informar al usuario que la conexión está en progreso")]
        [SerializeField]
        private GameObject progressLabel;

        [Tooltip("La etiqueta UI para informar al usuario que la conexión está en progreso")]
        [SerializeField]
        private TextMeshProUGUI log;

        [Tooltip("La etiqueta UI para informar al usuario cuantos se han conectado")]
        [SerializeField]
        private GameObject countLabel;

        [Tooltip("La etiqueta UI para informar al usuario cuantos se han conectado")]
        [SerializeField]
        private TextMeshProUGUI countText;

        [Tooltip("La etiqueta UI para informar al usuario la espera de jugadores")]
        [SerializeField]
        private TextMeshProUGUI waitingPlayersText;

        private bool isLoading = false;

        #endregion

        #region Private Fields
        /*
         * El número de versión de este cliente. GameVersion separa a los usuarios entre sí (lo que te permite hacer cambios importantes)
        */
        string gameVersion = "1";

        // Mantenga un registro del proceso actual. Como la conexión es asíncrona y se basa en varias devoluciones de llamada de Photon,
        // necesitamos hacer un seguimiento de esto para ajustar correctamente el comportamiento cuando recibimos una llamada de Photon.
        // Normalmente, esto se usa para la devolución de llamada OnConnectedToMaster ().
        bool isConnecting;

        int playerCount = 0;

        #endregion
        
        #region MonoBehaviour CallBacks
        /*    
         * El método MonoBehaviour llamado en GameObject por Unity durante la fase de inicialización temprana.
         */
        private void Awake()
        {
            /* #Critical
            * esto asegura que podamos usar PhotonNetwork.LoadLevel () en el cliente maestro y todos los clientes en la misma sala sincronizan su nivel automáticamente
            */
            PhotonNetwork.AutomaticallySyncScene = true; //Cuando esto es cierto, el MasterClient puede llamar a PhotonNetwork.LoadLevel () y todos los jugadores conectados cargarán automáticamente ese mismo nivel.
        }


        /*
         * Método MonoBehaviour llamado en GameObject por Unity durante la fase de inicialización.
         */
        private void Start()
        {
            //TODO: VER COMO SE PRESENTARA LA INTERFAZ DE CONEXIÓN
            minPlayerPerRoom = maxPlayersPerRoom = GameManager.sharedInstance.limitPlayers;
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Connect();
        }

        private void FixedUpdate()
        {
            if (PhotonNetwork.CurrentRoom != null)
            {
                playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                countText.text = playerCount + "/" + maxPlayersPerRoom;
            }

            if (!isLoading && playerCount >= minPlayerPerRoom)
            {
                //carga la habitación del nivel.
                //PhotonNetwork.LoadLevel("Game");
                LoadingLevel();
                //StartCoroutine(LoadScene());
            }

            if (PhotonNetwork.LevelLoadingProgress > 0 && PhotonNetwork.LevelLoadingProgress < 1)
            {
                Debug.Log(PhotonNetwork.LevelLoadingProgress);
            }


        }

        #endregion

        void LoadingLevel()
        {
            isLoading = true;
            PhotonNetwork.LoadLevel("Game Felipe Multiplayer");            
        }

        /*
        IEnumerator LoadScene()
        {
            AsyncOperation operation = scenem
        }*/
        // TODO: PROBAR ESTE METODO DE CARGA
        /*
        private IEnumerator MoveToGameScene()
        {
            // Temporary disable processing of futher network messages
            PhotonNetwork.IsMessageQueueRunning = false;
            LoadNewScene(newSceneName); // custom method to load the new scene by name
            while (newSceneDidNotFinishLoading)
            {
                yield return null;
            }
            PhotonNetwork.IsMessageQueueRunning = true;
        }
        */


        #region Public Methods

        /*
         * Inicia el proceso de conexión.
         * - Si ya está conectado, intentamos unirnos a una sala aleatoria
         * - si aún no está conectado, conecte esta instancia de aplicación a Photon Cloud Network
         */

        public void Connect()
        {
            //no pierdas de vista la voluntad de unirte a una sala, porque cuando volvamos del juego recibiremos una devolución de llamada de que estamos conectados, por lo que debemos saber qué hacer.
            isConnecting = true;


            //UI
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            buttonMenu.SetActive(false);


            // comprobamos si estamos conectados o no, nos unimos si lo estamos, de lo contrario iniciamos la conexión con el servidor.
            if (PhotonNetwork.IsConnected)
            {
                // # Crítico, necesitamos en este punto intentar unirnos a una sala aleatoria. Si falla, seremos notificados en OnJoinRandomFailed () y crearemos uno.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // # Crítico, primero debemos conectarnos a Photon Online Server.                
                if(PhotonNetwork.ConnectUsingSettings())//punto de partida para conectarse a Photon Cloud.
                {
                    PhotonNetwork.GameVersion = gameVersion;
                    log.text = "Conectando con el servidor...";
                }
                else
                    log.text = "Error en la conexión con el servidor...";
            }
        }

        #endregion

        #region MonoBehavioursPunCallBacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Launcher Llamaradas: OnConnectedToMaster() fue llamado por PUN");

            // no queremos hacer nada si no intentamos unirnos a una habitación.
            // este caso donde isConnecting es falso es típicamente cuando pierdes o sales del juego, cuando se carga este nivel, 
            // se llamará a OnConnectedToMaster, en ese caso no queremos hacer nada.
            if (isConnecting)
            {
                log.text = "Conectado al servidor...";
                
                // # Crítico: lo primero que intentamos hacer es unirnos a una sala existente potencial. Si hay, bueno, de lo contrario, seremos llamados de nuevo con OnJoinRandomFailed ()
                if (!PhotonNetwork.JoinRandomRoom())
                {
                    log.text = "Eror en la unión a una sala...";
                    Debug.Log("Error en la unión a una sala");
                }
            }
            else
                Debug.Log("error en la conexión con el master");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            buttonMenu.SetActive(true);

            Debug.LogWarningFormat("PUN Basics Launcher Llamaradas: OnDisconnected() fue llamado por PUN con causa {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basisc Launcher Llamaradas: OnJoinRandomFailed() fue llamado por PUN. No habia sala disponible, creamos una.\nLlamamos a: PhotonNetwork.CreateRoom ");

            // # Crítico: no pudimos unirnos a una sala aleatoria, quizás no exista ninguno o estén todos llenos. No te preocupes, creamos una nueva sala
            if (!GameManager.sharedInstance.JoinRoom)
            {
                Debug.Log("if: " +maxPlayersPerRoom);
                if (!PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }))
                {
                    Debug.Log("otro if: " +maxPlayersPerRoom);
                    log.text = "Error en la creación de una sala...";
                    Debug.Log("Error en la cración de la una sala");
                }
            }            
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Launcher Llamaradas: OnJoinedRoom() llamado por PUN. Ahora este cliente esta en la sala.");
            log.text = "Unido a la sala...";
            waitingPlayersText.gameObject.SetActive(true);
            //countLabel.SetActive(true);
        }
        #endregion
    }
}

