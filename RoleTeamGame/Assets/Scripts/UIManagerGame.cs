
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion //Namespaces


public class UIManagerGame : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public static UIManagerGame sharedInstance;

    public GameObject panelButtons;
    public GameObject buttons;
    public GameObject panelUI;
   // public GameObject[] panelButtonsMove;

    public GameObject up;
    public GameObject right;
    public GameObject down;
    public GameObject left;
    public GameObject upRight;
    public GameObject upLeft;
    public GameObject downRight;
    public GameObject downLeft;
    public GameObject rightUp;
    public GameObject rightDown;
    public GameObject leftUp;
    public GameObject leftDown;
    public GUIAnimFREE boton;
    private bool showingPanel = false;

    #endregion

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour
    private void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion
         
    // BUTTONS
    public void ShowButtonsActions()
    {
        panelButtons.SetActive(true);
    }

    public void HideButtonsActions()
    {
        panelButtons.SetActive(false);
    }


    // PANEL MOVE
    public void ShowPanelMove()
    {
        buttons.SetActive(true);
        HidePanelUI();
        HideButtonsActions();
    }

    public void HidePanelMove()
    {
        buttons.SetActive(false);
        ShowPanelUI();
        ShowButtonsActions();
        HideAllButtonsMove();
    }

    public void TogglePanel(){
        if(!boton.gameObject.activeSelf){
            boton.gameObject.SetActive(true);
            boton.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
            showingPanel = true;
        }
        else{
            if(showingPanel)
                boton.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
            else
                boton.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
            showingPanel = !showingPanel;
        }
    }

    public void HideAllButtonsMove()
    {
        up.SetActive(false);
        down.SetActive(false);
        right.SetActive(false);
        left.SetActive(false);
        rightUp.SetActive(false);
        rightDown.SetActive(false);
        leftUp.SetActive(false);
        leftDown.SetActive(false);
        upRight.SetActive(false);
        upLeft.SetActive(false);
        downRight.SetActive(false);
        downLeft.SetActive(false);
    }

    //  PANEL UI
    public void ShowPanelUI()
    {
        panelUI.SetActive(true);
    }

    public void HidePanelUI()
    {
        panelUI.SetActive(false);
    }
}
