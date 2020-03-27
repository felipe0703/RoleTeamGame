// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using TMPro;

namespace Com.BrumaGames.Llamaradas
{
    public class PlayerListEntry : MonoBehaviour
    {
        [Header("UI References")]
        public TextMeshProUGUI PlayerNameText;

        public Image PlayerColorImage;
        public Button PlayerReadyButton;
        public Image PlayerReadyImage;

        private int ownerId;
        private bool isPlayerReady;
        private bool isTurn;

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                PlayerReadyButton.gameObject.SetActive(false);
            }
            else
            {
                Hashtable initialProps = new Hashtable() {
                    { LlamaradaGame.PLAYER_READY, isPlayerReady },
                    { LlamaradaGame.PLAYER_ENERGIES, LlamaradaGame.PLAYER_MAX_ENERGIES },
                    { LlamaradaGame.PLAYER_TURN, isTurn}
                };

                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                PlayerReadyButton.onClick.AddListener(() =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() {
                        { LlamaradaGame.PLAYER_READY, isPlayerReady }
                    };

                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        FindObjectOfType<LobbyMain>().LocalPlayerPropertiesUpdated();
                    }
                });
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        //entrega id del player y nombre
        public void Initialize(int playerId, string playerName)
        {
            string coach = "";

            if (playerId == 1)
                coach = " Coach";

            ownerId = playerId;
            PlayerNameText.text = playerName + coach;
        }

        // Color al player
        private void OnPlayerNumberingChanged()
        {
           
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == ownerId)
                {
                    Debug.Log("Número: " + p.GetPlayerNumber());
                    PlayerColorImage.color = LlamaradaGame.GetColor(p.GetPlayerNumber());
                }
            }
        }

        //activa la imagen 
        public void SetPlayerReady(bool playerReady)
        {
            PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = playerReady ? "Listo!" : "Listo?";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}