using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerMinimap : MonoBehaviour
{
    public GameObject vCam1;
    public GameObject vCam2;
    public GameObject panelMinimap;

    Camera cam;
    public Camera camMinimap;
    public Transform target;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (cam != null)
            UpdateTargetPosition();
    }



    public void UpdateTargetPosition()
    {
        Vector3 newPosition = Vector3.zero;
        bool positionFound = false;


        newPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        //Debug.Log("X: " + newPosition.x + " Y: " + newPosition.y);

        Vector3 newPosition2 = camMinimap.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log("X2: " + (newPosition2.x - newPosition.x) + " Y2: " + (newPosition2.y - newPosition.y));



        Vector3 position = cam.transform.position;
        Vector3 position2 = camMinimap.transform.position;
        Vector3 positionTarget = position2 - position;


        /*Debug.Log("X: " + position.x + " Y: " + position.y);
        Debug.Log("X2: " + position2.x + " Y2: " + position2.y);
        Debug.Log("X3: " + positionTarget.x + " Y3: " + positionTarget.y);*/
       // Debug.Log("X4: " + (newPosition.x + positionTarget.x) + " Y4: " + (newPosition.y + positionTarget.y));


        positionFound = true;

        if (positionFound && newPosition != target.position)
        {
            target.position = newPosition + positionTarget;            
        }
    }

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
