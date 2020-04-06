
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;

namespace Com.BrumaGames.Llamaradas
{
    /* Este es un temporizador de cuenta regresiva básico. Para iniciar el temporizador, 
     * el MasterClient puede agregar una determinada entrada a las Propiedades de la sala personalizada, 
     * que contiene el nombre de la propiedad 'StartTime' y la hora de inicio real que describe el 
     * momento en que se inició el temporizador.
     * 
     * Para tener un temporizador sincronizado, la mejor práctica es usar PhotonNetwork.Time.
     * 
     * Para suscribirse al evento CountdownTimerHasExpired puede llamar a 
     * CountdownTimer.OnCountdownTimerHasExpired + = OnCountdownTimerIsExpired; 
     * de la función OnEnable de Unity, por ejemplo. Para cancelar la suscripción, simplemente llame a
     * CountdownTimer.OnCountdownTimerHasExpired - = OnCountdownTimerIsExpired; 
     * Puede hacerlo desde la función OnDisable de Unity, por ejemplo. 
    */
    public class CronometerTimer : MonoBehaviourPunCallbacks
    {
        public const string CronometerStartTime = "GameTime";

        private bool isTimerRunning;

        private float startTime;

        [Header("Reference to a Text component for visualizing the countdown")]
        public TextMeshProUGUI TextClient;
        public TextMeshProUGUI TextMaster;

        public void Start()
        {
            if(PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    if (TextMaster == null)
                    {
                        Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                        return;
                    }
                }
                else
                {
                    if (TextClient == null)
                    {
                        Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                        return;
                    }
                }
            }
            else
            {
                if (TextClient == null)
                {
                    Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                    return;
                }
            }
            
        }

        public void Update()
        {

            //Debug.Log("isrunning " + isTimerRunning);
            if (!isTimerRunning)
            {
                return;
            }

            float timer = (float)PhotonNetwork.Time - startTime;
            //float countdown = Countdown - timer;

            int minutos = 0;
            int segundos = 0;
            string textoDelReloj;

            //  Calcular minutos y segundos
            minutos = (int)timer / 60;
            segundos = (int)timer % 60;


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

            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                    TextMaster.text = textoDelReloj;
                else
                    TextClient.text = textoDelReloj;
            }
            else
            {
                TextClient.text = textoDelReloj;
                Debug.Log("------------------------------");
            }
                



        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;

            if (propertiesThatChanged.TryGetValue(CronometerStartTime, out startTimeFromProps))
            {
                isTimerRunning = true;
                startTime = (float)startTimeFromProps;
            }
        }
    }
}
 