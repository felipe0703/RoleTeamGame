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

    //  COMPONENTES
    //GameController controller;

    //  POSICIONES DEL JUGADOR AL INICIAR
    int[] positionA = { 0, 100 };
    int[] positionB = { 10, 30, 50, 70, 90 };

    // para detectar los caramelos adyacentes
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    void Start()
    {
        //controller = GameObject.Find("GameController").GetComponent<GameController>();

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
            if(GameController.sharedInstance.numbersActions == GameController.sharedInstance.maxNumbersActions)
            {
                GameController.sharedInstance.ShowPanelEndTurn();
            }
        }
    }


    void DetectEdifice()
    {
        edificeUp = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.up, distanceEdifice, whatIsEdifice);
        edificeRight = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.right, distanceEdifice, whatIsEdifice);
        edificeDown = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.down, distanceEdifice, whatIsEdifice);
        edificeLeft = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), Vector2.left, distanceEdifice, whatIsEdifice);

      /*  Debug.Log(GetNeighbor(adjacentDirections[0]).name);
        Debug.Log(GetNeighbor(adjacentDirections[1]).name);
        Debug.Log(GetNeighbor(adjacentDirections[2]).name);
        Debug.Log(GetNeighbor(adjacentDirections[3]).name);*/


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

    // obtengo el vecino
    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    //  MOVE
    void MovePlayer(float fixedSpeed, Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, fixedSpeed);
    }      

    void InAllMovements(Vector3 targetPosition)
    {
        target.position = targetPosition;
        moveTarget1 = true;                
        GameController.sharedInstance.AddActions();
        HidePanelMove();
    } 

    // LEFT
    public void MoveLeftPlayer()
    { 
        InAllMovements(targets[4].position);
        target2.position = targets[4].position;
    }

    public void MoveLeftUp()
    {
        InAllMovements(targets[4].position);
        target2.position = targets[1].position;
    }

    public void MoveLeftDown()
    {
        InAllMovements(targets[4].position);
        target2.position = targets[3].position;
    }

    //  RIGHT
    public void MoveRightPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[2].position;
    }
    public void MoveRightUpPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[1].position;
    }
    public void MoveRightDownPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[3].position;
    }

    //  UP
    public void MoveUpPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[1].position;
    }
    public void MoveUpRightPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[2].position;
    }
    public void MoveUpLeftPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[4].position;
    }

    // DOWN
    public void MoveDownPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[3].position;
    }
    public void MoveDownRightPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[2].position;
    }
    public void MoveDownLeftPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[4].position;
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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }
}
