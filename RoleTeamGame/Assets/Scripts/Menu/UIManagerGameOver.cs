using System.Collections;
using System.Collections.Generic;
using Com.BrumaGames.Llamaradas;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
//using DG.Tweening;

public class UIManagerGameOver : MonoBehaviour
{

    public Canvas canvas;
    public GUIAnimFREE title, textGameOv;

    public TextMeshProUGUI textScoreSaved;
    public TextMeshProUGUI textScoreDead;

    private void Awake()
    {
        if (enabled)
        {
            // Set GUIAnimSystemFREE.Instance.m_AutoAnimation to false in Awake() will let you control all GUI Animator elements in the scene via scripts.
            GUIAnimSystemFREE.Instance.m_AutoAnimation = false;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // MoveIn title
        StartCoroutine(MoveInTitleGameObjects());

        // Disable all scene switch buttons
        GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, false);

        //if(GameController.sharedInstance != null)
        //{
        textScoreSaved.text = LocalScore.saved + "";
        textScoreDead.text = LocalScore.dead + "";
        //}
    }


    IEnumerator MoveInTitleGameObjects()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveIn title
        title.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textGameOv.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

        // MoveIn all primary buttons
        StartCoroutine(MoveInButton());
    }


    // MoveIn all primary buttons
    IEnumerator MoveInButton()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveIn all primary buttons
        title.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textGameOv.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

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


    // MoveOut all primary buttons
    public void HideAllGUIs()
    {
        title.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textGameOv.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

        // MoveOut title
        StartCoroutine(HideTitleText());
    }

    // MoveOut title
    IEnumerator HideTitleText()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveOut title
        title.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textGameOv.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
    }
}
