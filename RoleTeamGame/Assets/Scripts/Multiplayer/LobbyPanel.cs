using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Com.BrumaGames.Llamaradas{

    public class LobbyPanel : MonoBehaviour
    {
        private readonly string connectionStatusMessage = "Estado conexión: ";

        [Header("UI References")]
        public TextMeshProUGUI ConnectionStatusText;

        #region UNITY

        public void Update()
        {
            ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
        }

        #endregion
    }
}