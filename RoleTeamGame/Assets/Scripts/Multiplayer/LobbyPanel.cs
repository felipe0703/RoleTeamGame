using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Com.BrumaGames.Llamaradas{

    public class LobbyPanel : MonoBehaviour
    {
        //private readonly string connectionStatusMessage = "Estado conexión: ";
        private string connectionStatusMessage;

        [Header("UI References")]
        public TextMeshProUGUI ConnectionStatusText;

        #region UNITY
        public void Awake()
        {
            connectionStatusMessage = I18nManager.sharedInstance.GetText("connectionStatus");
        }
        public void Update()
        {
            ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        }

        #endregion
    }
}