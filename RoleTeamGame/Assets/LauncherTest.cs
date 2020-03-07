using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LauncherTest : MonoBehaviour
{

    public int nextPlayersTeam;

    public Transform[] spawnPointsTeamOne;

    // Start is called before the first frame update
    void Start()
    {

        //loadBalancingClient.ConnectToRegionMaster("us");

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectAndLoad());
    }

    IEnumerator DisconnectAndLoad()
    {
        PhotonNetwork.Disconnect();
        while (PhotonNetwork.IsConnected)
            yield return null;

        //cargar escena menu
        //LoadingScene.loadlevel();
    }

    public void UpdateTeam()
    {
        if (nextPlayersTeam == 1)
            nextPlayersTeam = 2;
        else
            nextPlayersTeam = 1;

            
    }
}
