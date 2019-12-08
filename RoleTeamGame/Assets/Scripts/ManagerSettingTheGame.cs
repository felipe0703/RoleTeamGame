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

    public GUIAnimFREE bkgr,panelSettings,buttonOk,buttonBck,mapTxt,map1,map2,map3;

    #endregion // Variables

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour

    void Start()
    {
        bkgr.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        panelSettings.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonOk.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonBck.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        mapTxt.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map1.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map2.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map3.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
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
    public void clickAnims(){
        bkgr.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        panelSettings.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonOk.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonBck.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        mapTxt.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map1.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map2.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        map3.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);

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

        GameManager.sharedInstance.maxNumbersActions = action;
    }

    public void SetLimitPlayers(int option)
    {
        byte limit = 2;

        switch (option)
        {
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
