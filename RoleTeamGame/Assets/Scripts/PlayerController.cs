#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion // Namespaces

public class PlayerController : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
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
    int[] positionA = { 20, 140 };
    int[] positionB = { 30, 50, 70, 90, 110, 130 };



    // para detectar los caramelos adyacentes
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    #endregion

    // ########################################
    // MonoBehaviours Functions
    // ########################################

    #region MonoBehaviour
    void Start()
    {

        // Posicionamiento del player de forma aleatoria
        // TODO: QUE EL POSICIONAMIENTO DE UN JUGADOR NO SEA IGUAL A OTRO JUGADOR
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

    #endregion // MonoBehaviour

    // ########################################
    // Dectect Edifices Functions
    // ########################################

    #region DetectEdifices

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

        //TODO:CUANDO TENGA UN BORDER O RIO QUE NO MUESTRE BOTONES
        //  EDIFICIOS A LA DERECHA E IZQUIERDA
        if (GetNeighbor(adjacentDirections[1]).tag == "Edifice" && GetNeighbor(adjacentDirections[3]).tag == "Edifice")
        {          

            // HAY BORDE ARRIBA
            if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isBorder)
            {
                UIManagerGame.sharedInstance.down.SetActive(true);
            }// HAY BORDE ABAJO
            else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isBorder)
            {
                UIManagerGame.sharedInstance.up.SetActive(true);
            }
            else
            {
                UIManagerGame.sharedInstance.up.SetActive(true);
                UIManagerGame.sharedInstance.down.SetActive(true);
            }
            UIManagerGame.sharedInstance.upRight.SetActive(true);
            UIManagerGame.sharedInstance.upLeft.SetActive(true);
            UIManagerGame.sharedInstance.downRight.SetActive(true);
            UIManagerGame.sharedInstance.downLeft.SetActive(true);
        }

        // EDIFICIOS ARRIBA Y ABAJO
        if (GetNeighbor(adjacentDirections[0]).tag == "Edifice" && GetNeighbor(adjacentDirections[2]).tag == "Edifice")
        {
            // HAY BORDE DERECHA
            if (GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isBorder)
            {
                UIManagerGame.sharedInstance.left.SetActive(true);
            }// HAY BORDE IZQUIERDA
            else if (GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isBorder)
            {
                UIManagerGame.sharedInstance.right.SetActive(true);
            }
            else
            {
                UIManagerGame.sharedInstance.right.SetActive(true);
                UIManagerGame.sharedInstance.left.SetActive(true);
            }
            UIManagerGame.sharedInstance.rightUp.SetActive(true);
            UIManagerGame.sharedInstance.rightDown.SetActive(true);
            UIManagerGame.sharedInstance.leftUp.SetActive(true);
            UIManagerGame.sharedInstance.leftDown.SetActive(true);
        }


        //EDIFICIO A LA DERECHA
        if (GetNeighbor(adjacentDirections[1]).tag == "Edifice" )
        {
            // BOSQUE A LA IZQUIERDA
            if(GetNeighbor(adjacentDirections[3]).tag == "Border")
            {
                // si tengo una esquina arriba
                if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.down.SetActive(true);
                }// si tengo una esquina abajo
                else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.up.SetActive(true);
                }
                else
                {
                    UIManagerGame.sharedInstance.up.SetActive(true);
                    UIManagerGame.sharedInstance.down.SetActive(true);
                }

                UIManagerGame.sharedInstance.downRight.SetActive(true);
                UIManagerGame.sharedInstance.upRight.SetActive(true);
            }
        }
        
        //EDIFICIO A LA IZQUIEDA
        if (GetNeighbor(adjacentDirections[3]).tag == "Edifice" )
        {
            // AGUA A LA DERECHA
            if(GetNeighbor(adjacentDirections[1]).tag == "River")
            {
                // si tengo una esquina arriba
                if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.down.SetActive(true);
                }// si tengo una esquina abajo
                else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.up.SetActive(true);
                }
                else
                {
                    UIManagerGame.sharedInstance.up.SetActive(true);
                    UIManagerGame.sharedInstance.down.SetActive(true);
                }

                UIManagerGame.sharedInstance.downLeft.SetActive(true);
                UIManagerGame.sharedInstance.upLeft.SetActive(true);
            }
        } 
        
        //EDIFICIO ABAJO
        if (GetNeighbor(adjacentDirections[2]).tag == "Edifice" )
        {
            // BORDE ARRIBA
            if(GetNeighbor(adjacentDirections[0]).tag == "Border")
            {
                // si tengo una esquina derecha
                if (GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.left.SetActive(true);
                }// si tengo una esquina izquierda
                else if (GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.right.SetActive(true);
                }
                else
                {
                    UIManagerGame.sharedInstance.right.SetActive(true);
                    UIManagerGame.sharedInstance.left.SetActive(true);
                }

                UIManagerGame.sharedInstance.rightDown.SetActive(true);
                UIManagerGame.sharedInstance.leftDown.SetActive(true);
            }
        } 
        
        //EDIFICIO ARRIBA
        if (GetNeighbor(adjacentDirections[0]).tag == "Edifice" )
        {
            // BORDE ABAJO
            if(GetNeighbor(adjacentDirections[2]).tag == "Border")
            {
                // si tengo una esquina derecha
                if (GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.left.SetActive(true);
                }// si tengo una esquina izquierda
                else if (GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isCorner)
                {
                    UIManagerGame.sharedInstance.right.SetActive(true);
                }
                else
                {
                    UIManagerGame.sharedInstance.right.SetActive(true);
                    UIManagerGame.sharedInstance.left.SetActive(true);
                }
                UIManagerGame.sharedInstance.rightUp.SetActive(true);
                UIManagerGame.sharedInstance.leftUp.SetActive(true);
            }
        }
    }
    // detecta si tenemos algun edificio a los lados y activa el boton del edificio
    public void DetectEdificeToInspect()
    {
        GameObject edifice;

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (GetNeighbor(adjacentDirections[i]).tag == "Edifice")
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
            if (GetNeighbor(adjacentDirections[i]).tag == "Edifice")
            {
                edifice = GetNeighbor(adjacentDirections[i]);
                edifice.GetComponent<Edifice>().btn.SetActive(false);
            }
        }
    }
    #endregion //DetectEdifices

    // ########################################
    // Movements Functions
    // ########################################

    #region Movements
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
    #endregion // Movements

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }
}
