﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Com.BrumaGames.Llamaradas
{
    public class RoomListEntry : MonoBehaviour
    {
        public TextMeshProUGUI RoomNameText;
        public TextMeshProUGUI RoomPlayersText;
        public Button JoinRoomButton;

        private string roomName;

        public void Start()
        {
            JoinRoomButton.onClick.AddListener(() =>
            {
                StartCoroutine(OnJoinRoom());
            });
        }

        IEnumerator OnJoinRoom()
        {
            yield return new WaitForSeconds(.7f);
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }

            PhotonNetwork.JoinRoom(roomName);
        }

        public void Initialize(string name, byte currentPlayers, byte maxPlayers)
        {
            roomName = name;

            RoomNameText.text = name;
            RoomPlayersText.text = (currentPlayers - 1)  + " / " + (maxPlayers-1);
        }
    }
}


