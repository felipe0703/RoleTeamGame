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
        int action = 0;
        if (option == 0)
        {
            action = 3;
        }
        else if (option == 1)
        {
            action = 4;
        }
        else if (option == 2)
        {
            action = 5;
        }

        GameManager.sharedInstance.maxNumbersActions = action;
    }
    #endregion  //time/actions
}
