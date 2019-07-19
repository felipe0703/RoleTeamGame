using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public RectTransform botonStart;
    public RectTransform title;

    // Start is called before the first frame update
    void Start()
    {
        title.DOAnchorPosY(160f , 0.5f);
        botonStart.DOAnchorPosX(0, 0.5f);
    }

}
