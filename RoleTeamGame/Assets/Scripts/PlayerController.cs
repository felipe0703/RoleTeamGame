using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space(10)]
    [Header("Movement")]
    public Transform target;
    public Transform target2;
    public int MyTurn = 1;

    Transform[] targets;
    
    public float speed;


    // VARIABLES PRIVADAS    
    float distanceEdifice = 8f;
    bool moveTarget1, moveTarget2 = false;


    //  COMPONENTES

    //  POSICIONES DEL JUGADOR AL INICIAR
    int[] positionA = { 0, 100 };
    int[] positionB = { 10, 30, 50, 70, 90 };



    // para detectar los caramelos adyacentes
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

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
                TurnSystemManager.sharedInstance.turn++;
                GameController.sharedInstance.ShowPanelEndTurn();
            }
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

    public void DetectEdificeToMove()
    {

        //TODO: MODIFICAR EL BORDE PARA QUE NO TENGA QUE PREGUNTAR TODO EL RATO SI ES NULL

        if ((GetNeighbor(adjacentDirections[0]) != null && GetNeighbor(adjacentDirections[0]).tag == "Edifice") || (GetNeighbor(adjacentDirections[2]) != null && GetNeighbor(adjacentDirections[2]).tag == "Edifice"))
        {
            //Debug.Log(GetNeighbor(adjacentDirections[0]).name);

            if ((GetNeighbor(adjacentDirections[1]) != null && GetNeighbor(adjacentDirections[1]).tag == "Street"))
            {
                UIManagerGame.sharedInstance.panelButtonsMove[1].SetActive(true);
            }

            if ((GetNeighbor(adjacentDirections[3]) != null && GetNeighbor(adjacentDirections[3]).tag == "Street"))
            {
                UIManagerGame.sharedInstance.panelButtonsMove[3].SetActive(true);
            }
        }

        if ((GetNeighbor(adjacentDirections[1]) != null && GetNeighbor(adjacentDirections[1]).tag == "Edifice") || (GetNeighbor(adjacentDirections[3]) != null && GetNeighbor(adjacentDirections[3]).tag == "Edifice"))
        {
            //Debug.Log(GetNeighbor(adjacentDirections[1]).name);

            if ((GetNeighbor(adjacentDirections[0]) != null && GetNeighbor(adjacentDirections[0]).tag == "Street"))
            {
                UIManagerGame.sharedInstance.panelButtonsMove[0].SetActive(true);
            }

            if ((GetNeighbor(adjacentDirections[2]) != null && GetNeighbor(adjacentDirections[2]).tag == "Street"))
            {
                UIManagerGame.sharedInstance.panelButtonsMove[2].SetActive(true);
            }
        }
    }

    public void DetectEdificeToInspect()
    {
        GameObject edifice;

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (GetNeighbor(adjacentDirections[i]) != null && GetNeighbor(adjacentDirections[i]).tag == "Edifice")
            {
                edifice = GetNeighbor(adjacentDirections[i]);
                edifice.GetComponent<Edifice>().btn.SetActive(true);
            }
        }       
    }
    public void HideAllButtonsInspect()
    {
        GameObject edifice;

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (GetNeighbor(adjacentDirections[i]) != null && GetNeighbor(adjacentDirections[i]).tag == "Edifice")
            {
                edifice = GetNeighbor(adjacentDirections[i]);
                edifice.GetComponent<Edifice>().btn.SetActive(false);
            }
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
        UIManagerGame.sharedInstance.HidePanelMove();
        UIManagerGame.sharedInstance.HideButtonsActions();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }
}
