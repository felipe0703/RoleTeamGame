using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public RectTransform boton1, boton2, boton3; 

    // Start is called before the first frame update
    void Start()
    {
        boton1.DOAnchorPosX(0, 0.5f);
        boton2.DOAnchorPosX(0, 0.5f);
        boton3.DOAnchorPosX(0, 0.5f);
    }

}
