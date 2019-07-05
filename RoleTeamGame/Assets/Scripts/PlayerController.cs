using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space(10)]
    [Header("Movement")]
    public Transform target;
    public Transform target2;

    Transform[] targets;
    
    public float speed;
    public GameObject panelMove;

    // VARIABLES PRIVADAS
    bool moveTarget1, moveTarget2 = false;
    

    // Start is called before the first frame update
    void Start()
    {
        if (target != null && target2 != null)
        {
            target.parent = null;
            target2.parent = null;
            targets = target.GetComponentsInChildren<Transform>();
        }
        panelMove.SetActive(false);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float fixedSpeed = speed * Time.deltaTime;

        if (moveTarget1)
        {
            MovePlayer(fixedSpeed,target.position);
        }
        if (moveTarget2)
        {
            MovePlayer(fixedSpeed, target2.position);
        }

        if(transform.position == target.position)
        {
            moveTarget1 = false;
            moveTarget2 = true;
        }       

        if (transform.position == target2.position)
        {
            moveTarget2 = false;
            target.position = transform.position;
        }


    }


    //  MOVE
    void MovePlayer(float fixedSpeed, Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fixedSpeed);
    }      

    // LEFT
    public void MoveLeftPlayer()
    {        
        target.position = targets[4].position;
        moveTarget1 = true;
        target2.position = targets[4].position;
        HidePanelMove();
    }

    public void MoveLeftUp()
    {
        target.position = targets[4].position;
        moveTarget1 = true;
        target2.position = targets[1].position;
        HidePanelMove();
    }

    public void MoveLeftDown()
    {
        target.position = targets[4].position;
        moveTarget1 = true;
        target2.position = targets[3].position;
        HidePanelMove();
    }

    //  RIGHT
    public void MoveRightPlayer()
    {
        target.position = targets[2].position;
        moveTarget1 = true;
        target2.position = targets[2].position;
        HidePanelMove();
    }
    public void MoveRightUpPlayer()
    {
        target.position = targets[2].position;
        moveTarget1 = true;
        target2.position = targets[1].position;
        HidePanelMove();
    }
    public void MoveRightDownPlayer()
    {
        target.position = targets[2].position;
        moveTarget1 = true;
        target2.position = targets[3].position;
        HidePanelMove();
    }




    public void ShowMove()
    {
        panelMove.SetActive(true);
    }
    void HidePanelMove()
    {
        panelMove.SetActive(false);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }
}
