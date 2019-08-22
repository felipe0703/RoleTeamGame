using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerGame : MonoBehaviour
{
    public static UIManagerGame sharedInstance;


    public GameObject panelButtons;
    public GameObject panelMove;
    public GameObject panelUI;
    public GameObject[] panelButtonsMove;

    private void Start()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }



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
        panelMove.SetActive(true);
        HidePanelUI();
        HideButtonsActions();
    }

    public void HidePanelMove()
    {
        panelMove.SetActive(false);
        ShowPanelUI();
        ShowButtonsActions();
        HideAllPanelMove();
    }

    public void HideAllPanelMove()
    {
        for (int i = 0; i < panelButtonsMove.Length; i++)
        {
            panelButtonsMove[i].SetActive(false);
        }
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
