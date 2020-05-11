using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JoinGame : MonoBehaviour
{
    public void SetJoinGame()
    {
        GameManager.sharedInstance.JoinRoom = true;
    }
}

