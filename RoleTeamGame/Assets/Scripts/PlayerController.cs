using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space(10)]
    [Header("Movement")]
    public Transform target;
    public Transform target2;
    public Transform[] targetRight;
    public Transform[] targetUp;
    public Transform[] targetLeft;
    public Transform[] targetDown;

    bool left, leftUp, leftDown = false;
    bool inTarget = false;

    public float speed;
    public GameObject panelMove;

    // VARIABLES PRIVADAS
    bool move = false;
    
    // COMPONENTES


    // Start is called before the first frame update
    void Start()
    {
        if (targetDown != null)
        {
            target.parent = null;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float fixedSpeed = speed * Time.deltaTime;

        if (move)
        {
            MovePlayer(fixedSpeed);
        }

        if(transform.position == target2.position)
        {
            move = false;
        }
    }


    //  MOVE

    void MovePlayer(float fixedSpeed)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, fixedSpeed);

        if(target.position == transform.position)
        {
            if (left)
            {
                target2.position = targetLeft[1].position;

            }
            if (leftUp)
            {
                target2.position = targetLeft[2].position;
            }
            if (leftDown)
            {
                target2.position = targetLeft[3].position;
            }
        }


    }
    //  DOWN
    public void MoveDownPlayer()
    {
        target.position = targetDown[0].position;
        move = true;
        panelMove.SetActive(false);
    }
    //  RIGHT
    public void MoveRightPlayer()
    {
        target.position = targetRight[1].position;
        move = true;
        panelMove.SetActive(false);
    }
    //  UP
    public void MoveUpPlayer()
    {
        target.position = targetUp[1].position;
        move = true;
        panelMove.SetActive(false);
    }


    // LEFT
    void MoveLeftPlayer()
    {
        target.position = targetLeft[0].position;
        target2.position = targetLeft[1].position;
        //move = true;
    }

    public void MoveLeftLef()
    {
        left = true;
        MoveLeftPlayer();
    }
    public void MoveLeftUp()
    {
        leftUp = true;
        MoveLeftPlayer();
    }
    public void MoveLeftDown()
    {
        leftDown = true;
    }

    public void ShowMove()
    {
        panelMove.SetActive(true);
    }

}
