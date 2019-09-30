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
    public GameObject street;
    Transform[] targets;    
    public float speed;

    // live
    public GameObject[] actions;

    // VARIABLES PRIVADAS    
    float distanceEdifice = 8f;
    bool moveTarget1, moveTarget2 = false;


    GameManager gm;

    //  POSICIONES DEL JUGADOR AL INICIAR
    int[] positionA = { 20, 140 };
    int[] positionB = { 30, 50, 70, 90, 110, 130 };

    //Detectar los edificios adyacentes
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    // Animacion
    private Animator animator;
    private Vector2 animDir = Vector2.zero;
    private Vector2 animDir2 = Vector2.zero;

    //Sistema de turno
    public bool myTurn = true;

    //Para corrutina de audio del fuego 'DetectFireEdifice'
    int fireLvl;
    bool detectFire = true;
    bool moving = true;
    GameObject vecino;
    GameObject bufferNvlFgo;
    GameObject fireGmObj;
    public FireSoundControl FireSound;



    #endregion

    // ########################################
    // MonoBehaviours Functions
    // ########################################

    #region MonoBehaviour

    private void Awake()
    {
    }

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();

        animator = GetComponent<Animator>();
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
            animator.SetBool("PlayerMoving",true);
            animator.SetFloat("MoveX",animDir.x);
            animator.SetFloat("MoveY",animDir.y);
            MovePlayer(fixedSpeed,target.position);
        }

        if (moveTarget2)
        {
            animator.SetBool("PlayerMoving",true);
            animator.SetFloat("MoveX",animDir2.x);
            animator.SetFloat("MoveY",animDir2.y);
            MovePlayer(fixedSpeed, target2.position);
            if (moving == true && detectFire == false) 
            {
                detectFire = true;
                moving = false;
            }
        }

        if(transform.position == target.position)
        {
            moveTarget1 = false;
            moveTarget2 = true;
        }
        if (transform.position != target.position)
        {
            moving = true;
        }

        if (transform.position == target2.position)
        {
            moveTarget2 = false;
            StopAnim();
            target.position = transform.position;
            
            //TODO:  quizás sea mejor manejar el fin del turno en el gamecontroller
            if (GameController.sharedInstance.numbersActions == 0 && myTurn)
            {
                myTurn = false;
                TurnSystemManager.sharedInstance.StartTurnFire();
            }
        }
        DetectFireLevel();
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

    //Este es lo mismo que arriba, pero calculando desde la posición destino del movimiento. 
    //Esto es para hacer una transición suave entre distintos niveles de sonido del fuego
    private GameObject GetFutureNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(target2.transform.position, direction);
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
                if (!edifice.GetComponent<Edifice>().isInspected)
                {
                    edifice.GetComponent<Edifice>().btn.SetActive(true);
                    edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);

                }
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
                edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }

    // detecta si tenemos algun edificio a los lados y activa el boton del edificio
    public void DetectEdificeTakeOutHabitant()
    {
        GameObject edifice;

        for (int i = 0; i < adjacentDirections.Length; i++)
        {
            if (GetNeighbor(adjacentDirections[i]).tag == "Edifice")
            {
                edifice = GetNeighbor(adjacentDirections[i]);

                if (edifice.GetComponent<Edifice>().isInspected) // el edificio fue inspeccionado 
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (edifice.GetComponent<Edifice>().habitants[j].image.enabled) // si hay habitantes
                        {
                            edifice.GetComponent<Edifice>().habitants[j].interactable = true;                            
                        }
                    }
                    edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);
                    edifice.GetComponent<Edifice>().idPositionEdifice = i;
                }
                else
                {
                    Debug.Log("no hay personas que salvar");
                }
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
        UpdateNumberOfActions();
        UIManagerGame.sharedInstance.HidePanelMove();
        UIManagerGame.sharedInstance.HideButtonsActions();
    } 

    // LEFT
    public void MoveLeftPlayer()
    { 
        InAllMovements(targets[4].position);
        target2.position = targets[4].position;
        SetDirectionAnim1(-1,0);
        SetDirectionAnim2(-1,0);
    }

    public void MoveLeftUp()
    {
        InAllMovements(targets[4].position);
        target2.position = targets[1].position;
        SetDirectionAnim1(-1,0);
        SetDirectionAnim2(0,1);
    }

    public void MoveLeftDown()
    {
        InAllMovements(targets[4].position);
        target2.position = targets[3].position;
        SetDirectionAnim1(-1,0);
        SetDirectionAnim2(0,-1);
    }

    //  RIGHT
    public void MoveRightPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[2].position;
        SetDirectionAnim1(1,0);
        SetDirectionAnim2(1,0);
    }
    public void MoveRightUpPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[1].position;
        SetDirectionAnim1(1,0);
        SetDirectionAnim2(0,1);
    }
    public void MoveRightDownPlayer()
    {
        InAllMovements(targets[2].position);
        target2.position = targets[3].position;
        SetDirectionAnim1(1,0);
        SetDirectionAnim2(0,-1);
    }

    //  UP
    public void MoveUpPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[1].position;
        SetDirectionAnim1(0,1);
        SetDirectionAnim2(0,1);
    }
    public void MoveUpRightPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[2].position;
        SetDirectionAnim1(0,1);
        SetDirectionAnim2(1,0);
    }
    public void MoveUpLeftPlayer()
    {
        InAllMovements(targets[1].position);
        target2.position = targets[4].position;
        SetDirectionAnim1(0,1);
        SetDirectionAnim2(-1,0);
    }

    // DOWN
    public void MoveDownPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[3].position;
        SetDirectionAnim1(0,-1);
        SetDirectionAnim2(0,-1);
    }
    public void MoveDownRightPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[2].position;
        SetDirectionAnim1(0,-1);
        SetDirectionAnim2(1,0);
    }
    public void MoveDownLeftPlayer()
    {
        InAllMovements(targets[3].position);
        target2.position = targets[4].position;
        SetDirectionAnim1(0,-1);
        SetDirectionAnim2(-1,0);
    }
    #endregion // Movements

    #region Audio

    void PlayStepSound()
    {
        gm.PlayOneStepSound();
    }

    

    #endregion

    #region Animacion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(target2.position, 0.5f);
    }

    private void SetDirectionAnim1(float x, float y){
        animDir = new Vector2(x,y);
    }

    private void SetDirectionAnim2(float x, float y){
        animDir2 = new Vector2(x,y);
    }

    private void StopAnim(){
        animator.SetBool("PlayerMoving",false);
        animator.SetFloat("MoveX",0);
        animator.SetFloat("MoveY",0);
        animator.SetFloat("LastMoveX",animDir2.x);
        animator.SetFloat("LastMoveY",animDir2.y);
    }
    #endregion // Animación

    public void UpdateNumberOfActions()
    {
        GameController.sharedInstance.SubtractActions();
        //animacion de restar acción
        int i = GameController.sharedInstance.numbersActions;
        actions[i].SetActive(false);
    }

    public void ActiveActions()
    {
        int maxActions = GameManager.sharedInstance.maxNumbersActions;
        for (int i = 0; i < maxActions; i++)
        {
            actions[i].SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Street"))
        {
            street = collision.gameObject;
        }
    }

    public void DetectFireLevel()
    {
        if (detectFire == true)
        {
            fireLvl = 0;
            for (int i = 0; i < 4; i++)
            {
                vecino = GetFutureNeighbor(adjacentDirections[i]);
                if (vecino.tag == "Edifice")
                {
                    if (vecino.transform.Find("FireEdifice") != null) //Si el vecino detectado es un edificio
                    {
                        bufferNvlFgo = vecino.transform.Find("FireEdifice/Level1").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 2)
                        {
                            fireLvl = 1;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireEdifice/Level2").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 3)
                        {
                            fireLvl = 2;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireEdifice/Level3").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 4)
                        {
                            fireLvl = 3;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireEdifice/Level4").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 5)
                        {
                            fireLvl = 4;
                        }
                    }

                    if (vecino.transform.Find("FireHouse") != null) //Si el vecino detectado es una casa
                    {
                        bufferNvlFgo = vecino.transform.Find("FireHouse/Level1").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 2)
                        {
                            fireLvl = 1;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireHouse/Level2").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 3)
                        {
                            fireLvl = 2;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireHouse/Level3").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 4)
                        {
                            fireLvl = 3;
                        }
                        bufferNvlFgo = vecino.transform.Find("FireHouse/Level4").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 5)
                        {
                            fireLvl = 4;
                        }
                    }

                    if (vecino.transform.Find("FirePark") != null) //Si el vecino detectado es un parque
                    {
                        bufferNvlFgo = vecino.transform.Find("FirePark/Level1").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 2)
                        {
                            fireLvl = 1;
                        }
                        bufferNvlFgo = vecino.transform.Find("FirePark/Level2").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 3)
                        {
                            fireLvl = 2;
                        }
                        bufferNvlFgo = vecino.transform.Find("FirePark/Level3").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 4)
                        {
                            fireLvl = 3;
                        }
                        bufferNvlFgo = vecino.transform.Find("FirePark/Level4").gameObject;
                        if (bufferNvlFgo.activeSelf && fireLvl < 5)
                        {
                            fireLvl = 4;
                        }
                    }


                }


            }
            Debug.Log("Nivel fuego cercano: " + fireLvl);
            FireSound.ChangeFireSound(fireLvl);
            detectFire = false;
        }
    }
}
