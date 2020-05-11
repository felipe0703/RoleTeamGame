using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class UIManager : MonoBehaviour
{

    public Canvas canvas;
    public GUIAnimFREE buttonStart,title,textStart, pressStartText, textLang, langDropdown;

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
    }


    IEnumerator MoveInTitleGameObjects()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveIn title
        title.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);        

        // MoveIn all primary buttons
        StartCoroutine(MoveInButton());
    }


    // MoveIn all primary buttons
    IEnumerator MoveInButton()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveIn all primary buttons
        buttonStart.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textStart.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        pressStartText.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

        yield return new WaitForSeconds(1.0f);

        textLang.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
        langDropdown.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);

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
        buttonStart.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
        textStart.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);

        // MoveOut title
        StartCoroutine(HideTitleText());
    }

    // MoveOut title
    IEnumerator HideTitleText()
    {
        yield return new WaitForSeconds(1.0f);

        // MoveOut title
        title.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
    }
}
