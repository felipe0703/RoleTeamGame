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
    public GameObject[] panelButtonsMove;

    // VARIABLES PRIVADAS
    bool moveTarget1, moveTarget2 = false;
    [SerializeField]
    bool edificeUp, edificeRight, edificeDown, edificeLeft = false;
    float distanceEdifice = 8f;
    [SerializeField] private LayerMask whatIsEdifice;

    //  POSICIONES DEL JUGADOR AL INICIAR
    int[] positionA = { 0, 100 };
    int[] positionB = { 10, 30, 50, 70, 90 };
    

    void Start()
    {
        // Posicionamiento del player de forma aleatoria
        int i1 = Random.Range(0, 2);
        int i2 = Random.Range(0, 5);
        transform.localPosition = new Vector3(positionA[i1], positionB[i2], 0);

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

    //  TEST
    public void Test()
    {
      
    }

    void DetectEdifice()
    {
        edificeUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up, distanceEdifice, whatIsEdifice);
        edificeRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, distanceEdifice, whatIsEdifice);
        edificeDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, distanceEdifice, whatIsEdifice);
        edificeLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, distanceEdifice, whatIsEdifice);

        if (!edificeUp)
        {
            panelButtonsMove[0].SetActive(true);
        }
        if (!edificeRight)
        {
            panelButtonsMove[1].SetActive(true);
        }
        if (!edificeDown)
        {
            panelButtonsMove[2].SetActive(true);
        }
        if (!edificeLeft)
        {
            panelButtonsMove[3].SetActive(true);
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

    //  UP
    public void MoveUpPlayer()
    {
        target.position = targets[1].position;
        moveTarget1 = true;
        target2.position = targets[1].position;
        HidePanelMove();
    }
    public void MoveUpRightPlayer()
    {
        target.position = targets[1].position;
        moveTarget1 = true;
        target2.position = targets[2].position;
        HidePanelMove();
    }
    public void MoveUpLeftPlayer()
    {
        target.position = targets[1].position;
        moveTarget1 = true;
        target2.position = targets[4].position;
        HidePanelMove();
    }

    // DOWN
    public void MoveDownPlayer()
    {
        target.position = targets[3].position;
        moveTarget1 = true;
        target2.position = targets[3].position;
        HidePanelMove();
    }
    public void MoveDownRightPlayer()
    {
        target.position = targets[3].position;
        moveTarget1 = true;
        target2.position = targets[2].position;
        HidePanelMove();
    }
    public void MoveDownLeftPlayer()
    {
        target.position = targets[3].position;
        moveTarget1 = true;
        target2.position = targets[4].position;
        HidePanelMove();
    }




    public void ShowMove()
    {
        panelMove.SetActive(true);
        DetectEdifice();
    }
    void HidePanelMove()
    {
        panelMove.SetActive(false);
        for (int i = 0; i < panelButtonsMove.Length; i++)
        {
            panelButtonsMove[i].SetActive(false);
        }
    }


    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }*/
}
