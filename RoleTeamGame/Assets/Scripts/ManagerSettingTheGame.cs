#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion // Namespace

public class ManagerSettingTheGame : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public static ManagerSettingTheGame sharedInstance;

    #endregion // Variables

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour

    void Start()
    {
        // SINGLETON
        if (sharedInstance == null)
        {
            sharedInstance = this;
            DontDestroyOnLoad(sharedInstance);
        }
        else
        {
            Destroy(gameObject);
        }
        GameManager.sharedInstance.maxNumbersActions = 2;
        GameManager.sharedInstance.limitPlayers = 1;

    }
    #endregion // Monobehaviour

    // ########################################
    // Funciones de Tiempo/Acciones
    // ########################################

    #region Time/Action
    public void SetTimeTurn(int option)
    {
        int time = 0;

        if(option == 0)
        {
            time = -1;
        }else if(option == 1)
        {
            time = 30;
        }else if(option == 2)
        {
            time = 45;
        }
        else if(option == 3)
        {
            time = 60;
        }

        GameManager.sharedInstance.timeTurn = time;
    }

    public void SetActionTurn(int option)
    {
        int action = option + 2;
        Debug.Log(option);
        GameManager.sharedInstance.maxNumbersActions = action;
    }

    public void SetLimitPlayers(int option)
    {
        byte limit = 1;

        switch (option)
        {
            case 0:
                limit = 1;
                break;
            case 1:
                limit = 2;
                break;
            case 2:
                limit = 3;
                break;
            case 3:
                limit = 4;
                break;
            case 4:
                limit = 5;
                break;
            case 5:
                limit = 6;
                break;
            case 6:
                limit = 7;
                break;
            case 7:
                limit = 8;
                break;
            default:
                break;            
        }
        Debug.Log(option);
        Debug.Log(limit);
        GameManager.sharedInstance.limitPlayers = limit;
    }
    #endregion  //time/actions
}
