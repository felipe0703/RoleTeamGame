using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;

public class UIManagerMainMenu : MonoBehaviour
{
    //public RectTransform bkgr, buttonMultiplayer, buttonTutorial, buttonInstruction, buttonCredit;
    public GUIAnimFREE bkgr,buttonCreateGame,buttonJoinGame,buttonAudio,buttonCredits;
    // Start is called before the first frame update

    void Start()
    {
        //bkgr.DOAnchorPosY(0, 0.2f);
        //buttonMultiplayer.DOAnchorPosX(-150 , 0.8f);
        //buttonTutorial.DOAnchorPosX(150, 0.8f);
        //buttonInstruction.DOAnchorPosX(150, 0.8f);
        //buttonCredit.DOAnchorPosY(-290, 0.8f);
        bkgr.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonCreateGame.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonJoinGame.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonAudio.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
        buttonCredits.PlayInAnims(GUIAnimSystemFREE.eGUIMove.SelfAndChildren);
    }

}
