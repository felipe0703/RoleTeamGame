using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UImanagerscene2 : MonoBehaviour
{
    public RectTransform bkgr,boton;
    // Start is called before the first frame update
    void Start()
    {
        bkgr.DOAnchorPosY(0,0.5f);
        boton.DOAnchorPosY(0,0.5f);
    }

}
