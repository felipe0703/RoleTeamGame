#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
#endregion // Namespaces

namespace Com.BrumaGames.Llamaradas
{
    public class PlayerController : MonoBehaviourPun //MonoBehaviour
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables 

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        CinemachineVirtualCamera playerVirtualCam;

        [Space(10)]
        [Header("Movement")]
        public Transform target;
        public Transform target2;
        public GameObject street;
        Transform[] targets;
        public float speed;

        [Space(10)]
        [Header("Actions")]
        // NUMERO DE ACCIONES
        public GameObject canvas;
        public GameObject canvasUI;
        public GameObject panelActions;
        public GameObject[] actions;        
        public int numbersActions = 0;

        //button ShowAActions
        public GameObject buttonShowActions;

        // VARIABLES PRIVADAS    
        float distanceEdifice = 8f;
        bool moveTarget1, moveTarget2 = false;


        GameManager gm;
        PhotonView pv;

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

        public LayerMask detectedByThePlayer;

        // Animacion
        private Animator animator;
        private Vector2 animDir = Vector2.zero;
        private Vector2 animDir2 = Vector2.zero;

        [Space(10)]
        [Header("Turn System")]
        //Sistema de turno
        public bool myTurn = false;
        public bool finishTurn = false;
        public int MyTurn;

        [Space(10)]
        [Header("Score")]
        // Sistema de puntuación
        public int savedHabitants = 0;
        public int deadHabitants = 0;

        
        //Para corrutina de audio del fuego 'DetectFireEdifice'
        int fireLvl;
        bool detectFire = true;
        bool moving = true;
        GameObject vecino;
        GameObject bufferNvlFgo;
        GameObject fireGmObj;

        [Space(10)]
        [Header("Audio")]
        public FireSoundControl FireSound;
        public SfxControl ScriptEfectos;



        

        #endregion

        // ########################################
        // MonoBehaviours Functions
        // ########################################

        #region MonoBehaviour

        private void Awake()
        {
            // # Importante
            // utilizado en GameController.cs: hacemos un seguimiento de la instancia localPlayer para evitar la creación de instancias cuando los niveles están sincronizados
            if (photonView.IsMine)
            {
                PlayerController.LocalPlayerInstance = this.gameObject;
            }
        }

        void Start()
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
            pv = GetComponent<PhotonView>();
            animator = GetComponent<Animator>();
            myTurn = false;
            finishTurn = false;
            //numbersActions = GameManager.sharedInstance.maxNumbersActions;

            if (pv.IsMine)
            {
                int actor = PhotonNetwork.LocalPlayer.ActorNumber;
                UIManagerGame.sharedInstance.textPlayer.text = "Soy Jugador: " + actor;
                //if (actor == 1)// si soy el primer jugador, es mi turno
                  //  myTurn = true;
            }
            

            // Posicionamiento del player de forma aleatoria
            // TODO: QUE EL POSICIONAMIENTO DE UN JUGADOR NO SEA IGUAL A OTRO JUGADOR
            int i1 = Random.Range(0, 2);
            int i2 = Random.Range(0, 5);
            //transform.localPosition = new Vector3(positionA[i1], positionB[i2], 0);

            if (target != null && target2 != null)
            {
                target.parent = null;
                target2.parent = null;
                targets = target.GetComponentsInChildren<Transform>();
            }

            // control de acciones del jugador
            canvas = GameObject.Find("Canvas");
            canvasUI = canvas.transform.GetChild(0).gameObject;
            panelActions = canvasUI.transform.GetChild(0).gameObject;

            for(int i = 0; i < panelActions.transform.childCount; i++)
            {
                actions[i] = panelActions.transform.GetChild(i).gameObject;
            }

            //activar visualización de las energías
            //ActiveActions();

            //obtener el boton de acciones
            GameObject canvasActions = canvas.transform.GetChild(1).gameObject;
            buttonShowActions = canvasActions.transform.GetChild(2).gameObject;

            UpdateScoreSaved(savedHabitants);
            UpdateScoreDead(0);
            //BoardManager.sharedInstance.SetIdEdifice();
        }

        
        // Update is called once per frame
        void FixedUpdate()
        {
            #region Move
            //velocidad jugador
            float fixedSpeed = speed * Time.deltaTime;

            // movimiento a la esquina
            if (moveTarget1)
            {
                animator.SetBool("PlayerMoving", true);
                animator.SetFloat("MoveX", animDir.x);
                animator.SetFloat("MoveY", animDir.y);
                MovePlayer(fixedSpeed, target.position);
            }
            //moviendo al jugador al destino
            if (moveTarget2)
            {
                animator.SetBool("PlayerMoving", true);
                animator.SetFloat("MoveX", animDir2.x);
                animator.SetFloat("MoveY", animDir2.y);
                MovePlayer(fixedSpeed, target2.position);
                if (moving == true && detectFire == false)
                {
                    detectFire = true;
                    moving = false;
                }
            }

            if (transform.position == target.position)
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

                // si llegue al destino y no tengo mas acciones finaliza mi turno
                if (pv.IsMine)
                {
                    if (numbersActions == 0 && myTurn)
                    {
                        finishTurn = true;
                        myTurn = false;
                    }

                    if (finishTurn)
                    {
                        finishTurn = false;
                        pv.RPC("SetTurn", RpcTarget.AllViaServer);
                    }
                }
            }
            #endregion
        }

        //IMPLEMENTAR LUEGO
        private void LateUpdate()
        {
            // DetectFireLevel();
            //TODO: detecta una referencia nula

            if(TurnSystemManager.sharedInstance != null)
            {
                if (PhotonNetwork.LocalPlayer.ActorNumber == TurnSystemManager.sharedInstance.playerTurn)
                {
                    //UIManagerGame.sharedInstance.textSetTurn.text = "Es mi turno";
                    buttonShowActions.SetActive(true);
                }
                else
                {
                    //UIManagerGame.sharedInstance.textSetTurn.text = "No es mi turno";
                    buttonShowActions.SetActive(false);
                }
            }            
        }
        #endregion // MonoBehaviour

        // ########################################
        // Dectect Edifices Functions
        // ########################################

        #region DetectEdifices

        // obtengo el vecino
        private GameObject GetNeighbor(Vector2 direction, string layerMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, LayerMask.GetMask(layerMask));
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

        public void HideAllButtonsInspect()
        {
            GameObject edifice;

            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], "Edifice").tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], "Edifice");
                    edifice.GetComponent<Edifice>().btn.SetActive(false);
                    edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        public void DetectEdificeToMove()
        {
            //  EDIFICIOS A LA DERECHA E IZQUIERDA
            if (GetNeighbor(adjacentDirections[1], "Edifice").tag == "Edifice" && GetNeighbor(adjacentDirections[3], "Edifice").tag == "Edifice")
            {

                // HAY BORDE ARRIBA
                if (GetNeighbor(adjacentDirections[0], "Streets").GetComponent<Street>().isBorder)
                {
                    UIManagerGame.sharedInstance.down.SetActive(true);
                }// HAY BORDE ABAJO
                else if (GetNeighbor(adjacentDirections[2], "Streets").GetComponent<Street>().isBorder)
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
            if (GetNeighbor(adjacentDirections[0], "Edifice").tag == "Edifice" && GetNeighbor(adjacentDirections[2], "Edifice").tag == "Edifice")
            {
                // HAY BORDE DERECHA
                if (GetNeighbor(adjacentDirections[1], "Streets").GetComponent<Street>().isBorder)
                {
                    UIManagerGame.sharedInstance.left.SetActive(true);
                }// HAY BORDE IZQUIERDA
                else if (GetNeighbor(adjacentDirections[3], "Streets").GetComponent<Street>().isBorder)
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
            if (GetNeighbor(adjacentDirections[1], "Edifice").tag == "Edifice")
            {
                // BOSQUE A LA IZQUIERDA
                if (GetNeighbor(adjacentDirections[3], "Streets").tag == "Border")
                {
                    // si tengo una esquina arriba
                    if (GetNeighbor(adjacentDirections[0], "Streets").GetComponent<Street>().isCorner)
                    {
                        UIManagerGame.sharedInstance.down.SetActive(true);
                    }// si tengo una esquina abajo
                    else if (GetNeighbor(adjacentDirections[2], "Streets").GetComponent<Street>().isCorner)
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
            if (GetNeighbor(adjacentDirections[3], "Edifice").tag == "Edifice")
            {
                // AGUA A LA DERECHA
                if (GetNeighbor(adjacentDirections[1], "Water").tag == "River")
                {
                    // si tengo una esquina arriba
                    if (GetNeighbor(adjacentDirections[0], "Streets").GetComponent<Street>().isCorner)
                    {
                        UIManagerGame.sharedInstance.down.SetActive(true);
                    }// si tengo una esquina abajo
                    else if (GetNeighbor(adjacentDirections[2], "Streets").GetComponent<Street>().isCorner)
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
            if (GetNeighbor(adjacentDirections[2], "Edifice").tag == "Edifice")
            {
                // BORDE ARRIBA
                if (GetNeighbor(adjacentDirections[0], "Streets").tag == "Border")
                {
                    // si tengo una esquina derecha
                    if (GetNeighbor(adjacentDirections[1], "Streets").GetComponent<Street>().isCorner)
                    {
                        UIManagerGame.sharedInstance.left.SetActive(true);
                    }// si tengo una esquina izquierda
                    else if (GetNeighbor(adjacentDirections[3], "Streets").GetComponent<Street>().isCorner)
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
            if (GetNeighbor(adjacentDirections[0], "Edifice").tag == "Edifice")
            {
                // BORDE ABAJO
                if (GetNeighbor(adjacentDirections[2], "Streets").tag == "Border")
                {
                    // si tengo una esquina derecha
                    if (GetNeighbor(adjacentDirections[1], "Streets").GetComponent<Street>().isCorner)
                    {
                        UIManagerGame.sharedInstance.left.SetActive(true);
                    }// si tengo una esquina izquierda
                    else if (GetNeighbor(adjacentDirections[3], "Streets").GetComponent<Street>().isCorner)
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
            bool detected = false;

            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], "Edifice").tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], "Edifice");
                
                    if (!edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice)
                    {
                        edifice.GetComponent<Edifice>().btn.SetActive(true);
                        edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);
                        detected = true;
                    }
                }
            }

            if (!detected)
            {
                UIManagerGame.sharedInstance.ShowPanelNotification("No hay edificios que ver");
            }
        }

        

        // detecta si tenemos algun edificio a los lados y activa el boton del edificio
        public void DetectEdificeTakeOutHabitant()
        {
            GameObject edifice;
            bool detected = false;
            //ver en las 4 direcciones
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], "Edifice").tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], "Edifice");

                    if (edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice) // el edificio fue inspeccionado 
                    {
                        //ScriptEfectos.DetectEdifice(edifice);
                        for (int j = 0; j < 3; j++)
                        {
                            if (edifice.GetComponent<Edifice>().habitants[j].image.enabled) // si hay habitantes
                            {
                                edifice.GetComponent<Edifice>().habitants[j].interactable = true;
                                edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);
                                detected = true;
                            }
                        }
                        edifice.GetComponent<Edifice>().idPositionEdifice = i;
                    }
                }
            }

            if (!detected)
            {
                UIManagerGame.sharedInstance.ShowPanelNotification("No hay habitantes visualizados");
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
            SetDirectionAnim1(-1, 0);
            SetDirectionAnim2(-1, 0);
        }

        public void MoveLeftUp()
        {
            InAllMovements(targets[4].position);
            target2.position = targets[1].position;
            SetDirectionAnim1(-1, 0);
            SetDirectionAnim2(0, 1);
        }

        public void MoveLeftDown()
        {
            InAllMovements(targets[4].position);
            target2.position = targets[3].position;
            SetDirectionAnim1(-1, 0);
            SetDirectionAnim2(0, -1);
        }

        //  RIGHT
        public void MoveRightPlayer()
        {
            InAllMovements(targets[2].position);
            target2.position = targets[2].position;
            SetDirectionAnim1(1, 0);
            SetDirectionAnim2(1, 0);
        }
        public void MoveRightUpPlayer()
        {
            InAllMovements(targets[2].position);
            target2.position = targets[1].position;
            SetDirectionAnim1(1, 0);
            SetDirectionAnim2(0, 1);
        }
        public void MoveRightDownPlayer()
        {
            InAllMovements(targets[2].position);
            target2.position = targets[3].position;
            SetDirectionAnim1(1, 0);
            SetDirectionAnim2(0, -1);
        }

        //  UP
        public void MoveUpPlayer()
        {
            InAllMovements(targets[1].position);
            target2.position = targets[1].position;
            SetDirectionAnim1(0, 1);
            SetDirectionAnim2(0, 1);
        }
        public void MoveUpRightPlayer()
        {
            InAllMovements(targets[1].position);
            target2.position = targets[2].position;
            SetDirectionAnim1(0, 1);
            SetDirectionAnim2(1, 0);
        }
        public void MoveUpLeftPlayer()
        {
            InAllMovements(targets[1].position);
            target2.position = targets[4].position;
            SetDirectionAnim1(0, 1);
            SetDirectionAnim2(-1, 0);
        }

        // DOWN
        public void MoveDownPlayer()
        {
            InAllMovements(targets[3].position);
            target2.position = targets[3].position;
            SetDirectionAnim1(0, -1);
            SetDirectionAnim2(0, -1);
        }
        public void MoveDownRightPlayer()
        {
            InAllMovements(targets[3].position);
            target2.position = targets[2].position;
            SetDirectionAnim1(0, -1);
            SetDirectionAnim2(1, 0);
        }
        public void MoveDownLeftPlayer()
        {
            InAllMovements(targets[3].position);
            target2.position = targets[4].position;
            SetDirectionAnim1(0, -1);
            SetDirectionAnim2(-1, 0);
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

        private void SetDirectionAnim1(float x, float y)
        {
            animDir = new Vector2(x, y);
        }

        private void SetDirectionAnim2(float x, float y)
        {
            animDir2 = new Vector2(x, y);
        }

        private void StopAnim()
        {
            animator.SetBool("PlayerMoving", false);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
            animator.SetFloat("LastMoveX", animDir2.x);
            animator.SetFloat("LastMoveY", animDir2.y);
        }
        #endregion // Animación

        #region Public Methods

        // ########################################
        // Funciones de Contador Acciones
        // ########################################

        #region Acciones
        
        
        //Activa UI Actions(Energias), recargo mis acciones y activo mi turno        
        public void ActiveActions()
        {
            //TODO: en el cliente no se carga al iniciar, el pv
            pv = GetComponent<PhotonView>();
            if (pv.IsMine)
            {
                numbersActions = GameManager.sharedInstance.maxNumbersActions;
                myTurn = true;
                for (int i = 0; i < numbersActions; i++)
                {
                    actions[i].SetActive(true);
                }
            }            
        }

        //   AGREGAR ACCION USADA
        public void AddActions()
        {
            if (numbersActions >= GameManager.sharedInstance.maxNumbersActions)
            {
                numbersActions = GameManager.sharedInstance.maxNumbersActions;
            }
            else
            {
                numbersActions++;
            }
        }

        //  RESTAR ACCION
        public void SubtractActions()
        {
            if (numbersActions <= 0)
            {
                numbersActions = 0;
            }
            else
            {
                numbersActions--;
            }
        }
        
        public void UpdateNumberOfActions()
        {
            //GameController.sharedInstance.SubtractActions();
            SubtractActions();
            //animacion de restar acción
            // int i = GameController.sharedInstance.numbersActions;
            int i = numbersActions;
            actions[i].SetActive(false);
        }

        #endregion //Acciones    

        #region DectectFireMusic
        //IMPLEMENTAR ACA Y EN TURNSYSTEM

        /* public void DetectFireLevel()
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
                 //TODO: HABILITAR LUEGO
                 //FireSound.ChangeFireSound(fireLvl);
                 detectFire = false;
             }
         }*/

        #endregion


        public void UpdateScoreSaved(int score)
        {
            savedHabitants += score;
            UIManagerGame.sharedInstance.UpdateScoreSavedText(savedHabitants);
        }
        
        public void UpdateScoreDead(int score)
        {
            deadHabitants += score; 
            UIManagerGame.sharedInstance.UpdateScoreDeadText(deadHabitants);
        }

        public void CallPostScorePlayer()
        {
            Debug.Log("llamando a post score");
            pv = GetComponent<PhotonView>();
            pv.RPC("PostScorePlayers", RpcTarget.AllBuffered, savedHabitants);
        }

        [PunRPC]
        void PostScorePlayers(int score)
        {
            Debug.Log("Puntaje enviado: " + score);
            GameController.sharedInstance.listScorePlayers.Add(score);
            
        }

        public int PostScoreText()
        {
            return savedHabitants;
        }
        #endregion // Public Methods

        #region Private Methods

        #region Turn
        // metodo que envia al servido el cambio de turno de jugador
        [PunRPC]
        void SetTurn()
        {
            TurnSystemManager.turn++;
            TurnSystemManager.sharedInstance.ExceedTurnLimit();
        }
        #endregion // Turn

        #region CollisionStreet
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Street"))
            {
                street = collision.gameObject;
            }
        }
        #endregion

        #endregion //Private Methods

    }

}
