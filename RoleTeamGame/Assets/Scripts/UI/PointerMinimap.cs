using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMinimap : MonoBehaviour
{
    public GameObject vCam1;
    public GameObject vCam2;
    public GameObject panelMinimap;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Person"))
        {
            ChangeCam();
            panelMinimap.SetActive(false);
            //collision.GetComponent<DetectEdifice>().DetectEdificeToMovePerson();
        }
    }

    public void ChangeCam()
    {
        vCam1.SetActive(false);
        vCam2.SetActive(true);
    }
}
