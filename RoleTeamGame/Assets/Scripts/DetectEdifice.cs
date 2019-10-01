using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: codigo repedido en el player controller, mejorar
public class DetectEdifice : MonoBehaviour
{
    #region Variables
    [Space(10)]
    [Header("Movement")]
    public Transform target;
    public Transform target2;
    Transform[] targets;

    //buttons
    public GameObject up;
    public GameObject right;
    public GameObject down;
    public GameObject left;
    public GameObject upRight;
    public GameObject upLeft;
    public GameObject downRight;
    public GameObject downLeft;
    public GameObject rightUp;
    public GameObject rightDown;
    public GameObject leftUp;
    public GameObject leftDown;

    //Detectar los edificios adyacentes
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    #endregion

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

    public void DetectEdificeToMovePerson()
    {
        //  EDIFICIOS A LA DERECHA E IZQUIERDA
        if (GetNeighbor(adjacentDirections[1]).tag == "Edifice" && GetNeighbor(adjacentDirections[3]).tag == "Edifice")
        {
            // HAY BORDE ARRIBA
            if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isBorder)
            {
                down.SetActive(true);
            }// HAY BORDE ABAJO
            else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isBorder)
            {
                up.SetActive(true);
            }
            else
            {
                up.SetActive(true);
                down.SetActive(true);
            }
            upRight.SetActive(true);
            upLeft.SetActive(true);
            downRight.SetActive(true);
            downLeft.SetActive(true);
        }

        // EDIFICIOS ARRIBA Y ABAJO
        if (GetNeighbor(adjacentDirections[0]).tag == "Edifice" && GetNeighbor(adjacentDirections[2]).tag == "Edifice")
        {
            // HAY BORDE DERECHA
            if (GetNeighbor(adjacentDirections[1]).tag == "Street" && GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isBorder)
            {
                left.SetActive(true);
            }// HAY BORDE IZQUIERDA
            else if (GetNeighbor(adjacentDirections[3]).tag == "Street" && GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isBorder)
            {
                right.SetActive(true);
            }
            else
            {
                right.SetActive(true);
                left.SetActive(true);
            }
            rightUp.SetActive(true);
            rightDown.SetActive(true);
            leftUp.SetActive(true);
            leftDown.SetActive(true);
        }


        //EDIFICIO A LA DERECHA y BOSQUE A LA IZQUIERDA
        if (GetNeighbor(adjacentDirections[1]).tag == "Edifice" && GetNeighbor(adjacentDirections[3]).tag == "Border")
        {
            // si tengo una esquina arriba
            if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isCorner)
            {
                down.SetActive(true);
            }// si tengo una esquina abajo
            else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isCorner)
            {
                up.SetActive(true);
            }
            else
            {
                up.SetActive(true);
                down.SetActive(true);
            }

            downRight.SetActive(true);
            upRight.SetActive(true);
        }

        //EDIFICIO A LA IZQUIEDA y AGUA A LA DERECHA
        if (GetNeighbor(adjacentDirections[3]).tag == "Edifice" && GetNeighbor(adjacentDirections[1]).tag == "River")
        {
            // si tengo una esquina arriba
            if (GetNeighbor(adjacentDirections[0]).GetComponent<Street>().isCorner)
            {
                down.SetActive(true);
            }// si tengo una esquina abajo
            else if (GetNeighbor(adjacentDirections[2]).GetComponent<Street>().isCorner)
            {
                up.SetActive(true);
            }
            else
            {
                up.SetActive(true);
                down.SetActive(true);
            }

            downLeft.SetActive(true);
            upLeft.SetActive(true);
        }

        //EDIFICIO ABAJO && BORDE ARRIBA
        if (GetNeighbor(adjacentDirections[2]).tag == "Edifice" && GetNeighbor(adjacentDirections[0]).tag == "Border")
        {
            // si tengo una esquina derecha
            if (GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isCorner)
            {
                left.SetActive(true);
            }// si tengo una esquina izquierda
            else if (GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isCorner)
            {
                right.SetActive(true);
            }
            else
            {
                right.SetActive(true);
                left.SetActive(true);
            }

            rightDown.SetActive(true);
            leftDown.SetActive(true);
        }

        //EDIFICIO ARRIBA y BORDE ABAJO
        if (GetNeighbor(adjacentDirections[0]).tag == "Edifice" && GetNeighbor(adjacentDirections[2]).tag == "Border")
        {
            // si tengo una esquina derecha
            if (GetNeighbor(adjacentDirections[1]).GetComponent<Street>().isCorner)
            {
                left.SetActive(true);
            }// si tengo una esquina izquierda
            else if (GetNeighbor(adjacentDirections[3]).GetComponent<Street>().isCorner)
            {
                right.SetActive(true);
            }
            else
            {
                right.SetActive(true);
                left.SetActive(true);
            }
            rightUp.SetActive(true);
            leftUp.SetActive(true);
        }
    }


    #endregion

}
