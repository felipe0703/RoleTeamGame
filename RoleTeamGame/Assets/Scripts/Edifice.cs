using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edifice : MonoBehaviour
{
    public int id;
    public GameObject btn;
    public GameObject panelInfo;

    public GameObject level1;


    public void IsSelected()
    {
        panelInfo.SetActive(true);
        btn.SetActive(false);
    }

    public void FireStart()
    {
        level1.SetActive(true);
    }
}
