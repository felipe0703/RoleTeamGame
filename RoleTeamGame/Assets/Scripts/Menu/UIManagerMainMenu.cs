using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class UIManagerMainMenu : MonoBehaviour
{
    public GUIAnimFREE topPanel,loginPanel;


    //public GUIAnimFREE buttonCreateRoom, buttonJoinRandom, buttonRoomList;


    public Canvas canvas;


    private void Awake()
    {
        if (enabled)
        {
            // Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
            GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
        }
    }

    void Start()
    {
        //bkgr.DOAnchorPosY(0, 0.2f);
        //buttonMultiplayer.DOAnchorPosX(-150 , 0.8f);
        //buttonTutorial.DOAnchorPosX(150, 0.8f);
        //buttonInstruction.DOAnchorPosX(150, 0.8f);
        //buttonCredit.DOAnchorPosY(-290, 0.8f);
        StartCoroutine(MoveInTopPanel());

        // Disable all scene switch buttons
        GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, false);
    }

    IEnumerator MoveInTopPanel()
    {
        yield return new WaitForSeconds(1.0f);

        topPanel.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

        StartCoroutine(MoveInLoginPanel());
    }

    IEnumerator MoveInLoginPanel()
    {
        yield return new WaitForSeconds(.8f);

        loginPanel.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

        // Enable all scene switch buttons
        StartCoroutine(EnableAllDemoButtons());

    }

    // Enable/Disable all scene switch Coroutine
    IEnumerator EnableAllDemoButtons()
    {
        yield return new WaitForSeconds(1.0f);

        // Enable all scene switch buttons
        GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, true);
    }

    /*
    public void AnimButtonRoomList()
    {
        buttonRoomList.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
        buttonRoomList.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        //StartCoroutine(ResetScaleButton());
    }

    IEnumerator ResetScaleButton()
    {
        yield return new WaitForSeconds(1.0f);
        

    }*/


}
