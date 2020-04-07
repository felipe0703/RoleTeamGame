
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
    public class RoundCountdownTimer : MonoBehaviourPunCallbacks
    {
        public const string CountdownStartTime = "RoundTime";

        // OnCountdownTimerHasExpired delegate.
        public delegate void CountdownTimerHasExpired();

        // Called when the timer has expired.
        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

        private bool isTimerRunning;

        private float startTime;

        [Header("Reference to a Text component for visualizing the countdown")]
        public TextMeshProUGUI TextClient;
        public TextMeshProUGUI TextMaster;

        private float Countdown;

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
            float countdown = Countdown - timer;


            if (PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                    TextMaster.text = string.Format(countdown.ToString("n2"));
                else
                    TextClient.text = string.Format(countdown.ToString("n2"));
            }else
                TextClient.text = string.Format(countdown.ToString("n2"));




            if (countdown > 0.0f)
            {
                return;
            }

            isTimerRunning = false;

            if(PhotonNetwork.PlayerList.Length > 1)
            {
                if(PhotonNetwork.IsMasterClient)
                    TextMaster.text = string.Empty;
                else
                    TextClient.text = string.Empty;

            }else
                TextClient.text = string.Empty;



            if (OnCountdownTimerHasExpired != null)
            {
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                Countdown = GameController.sharedInstance.optionR_TimeNum;
                isTimerRunning = false;
                startTime = (float)startTimeFromProps;
                StartCoroutine(waiting());
                if (PhotonNetwork.PlayerList.Length > 1)
                {
                    if (PhotonNetwork.IsMasterClient)
                        TextMaster.text = string.Format(Countdown.ToString("n2"));
                    else
                        TextClient.text = string.Format(Countdown.ToString("n2"));
                }
                else
                    TextClient.text = string.Format(Countdown.ToString("n2"));
                IEnumerator waiting()
                {
                    yield return new WaitForSeconds(3);
                    startTime = (float)PhotonNetwork.Time;
                    isTimerRunning = true;
                }
        }
        }
    }
}
 