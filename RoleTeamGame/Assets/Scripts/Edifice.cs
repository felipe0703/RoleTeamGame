using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edifice : MonoBehaviour
{
    public int id;
    public GameObject btn;
    public GameObject panelInfo;


    public void IsSelected()
    {
        panelInfo.SetActive(true);
        btn.SetActive(false);
    }
}
