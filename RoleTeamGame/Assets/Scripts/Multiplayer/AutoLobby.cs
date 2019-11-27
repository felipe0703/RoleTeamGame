using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class AutoLobby : MonoBehaviourPunCallbacks
{
    public Button connectButton;
    public Button joinRandomButton;

    public Text log;
    public Text playerCountText;

    public byte maxPlayerPerRoom = 4;
    public byte minPlayerPerRoom = 2;
    public int playersCount;
    bool isLoading = false;

    


    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                log.text += "\nConnectd to Server";
            }
            else
            {
                log.text += "\nFalling Connecting to Server";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        connectButton.interactable = false;
        joinRandomButton.interactable = true;
    }

    public void JoinRandom() // conectarse a una sala aleatoria
    {
        if (!PhotonNetwork.JoinRandomRoom()) 
        {
            log.text += "\nFall Joining room";
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message) // fallo la conexión a una sala
    {
        log.text += "\nNo Rooms to Join, creating one...";
        if (PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayerPerRoom })) // creamos una sala 
        {
            log.text += "\nRoom Created";
        }
        else
        {
            log.text += "\nfail Creating Room";
        }
    }

    public override void OnJoinedRoom()
    {
        log.text += "\nJoinned";
        joinRandomButton.interactable = false;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            playerCountText.text = playersCount + " / " + maxPlayerPerRoom;
        }

        if (!isLoading && playersCount >= minPlayerPerRoom)
        {
            LoadMap();
        }
    }

    void LoadMap()
    {
        isLoading = true;
        PhotonNetwork.LoadLevel("Game");
    }
}
