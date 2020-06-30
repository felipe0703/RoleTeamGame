
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
#endregion //Namespaces

namespace Com.BrumaGames.Llamaradas
{
    public class UIManagerGame : MonoBehaviourPunCallbacks
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables
        public static UIManagerGame sharedInstance;

        public GameObject panelLoadLevel;
        public GameObject panelButtons;
        public GameObject buttons;
        public GameObject panelUI;
        public GameObject panelActions;
        public GameObject panelNotification;
        public GUIAnimFREE panelChangeTurnClient;
        public GUIAnimFREE panelChangeTurnMaster;
        public GUIAnimFREE panelAdvanceFireClient;
        public GUIAnimFREE panelAdvanceFireMaster;
        public GUIAnimFREE panelChangeWindClient;
        public GUIAnimFREE panelChangeWindMaster;
        public TextMeshProUGUI textNotification;

        public GameObject vCam1;
        public GameObject vCam2;
        public GameObject vCamMaster;       
        public CinemachineVirtualCamera cine;


        public GameObject arrowClient;
        public GameObject arrowMaster;

        [Space(10)]
        public TextMeshProUGUI textTurn;
        public TextMeshProUGUI textTurnStatic;
        public TextMeshProUGUI textTurnStaticMaster;
        public TextMeshProUGUI textPlayer;
        public TextMeshProUGUI textSetTurn;

        public TextMeshProUGUI textScoreSaved;
        public TextMeshProUGUI textScoreDead;



        // public GameObject[] panelButtonsMove;

        // BUTTONS        
        private GameObject[] allButtons = new GameObject[12];
        [Space(10)]
        [Header("Buttons")]
        [Space(10)]
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
        public GUIAnimFREE boton;
        private bool showingPanel = false;

        public GameObject player;
        bool foundPlayer = false;
        PlayerController controller;
        PhotonView pv;
        PhotonView pvUI;

        public TextMeshProUGUI energies;
        public Dictionary<int, GameObject> playerListEntries;
        public GameObject[] actions;

        public GameObject canvasMaster;
        public GameObject canvasClient;
        public Canvas canvas;
        bool firtTurn;
        Camera camera;

        Player[] players;
        bool setNamePlayer = false;
        int contSetName = 0;
        public bool showMove = false;
        #endregion

        // ########################################
        // Funciones MonoBehaviour 
        // ########################################

        #region MonoBehaviour
        private void Awake()
        {
            if (sharedInstance == null) sharedInstance = this;
            else Destroy(gameObject);
            playerListEntries = new Dictionary<int, GameObject>();
        }

        private void Start()
        {
            pvUI = GetComponent<PhotonView>();
            camera = Camera.main;
            firtTurn = true;

            players = PhotonNetwork.PlayerList;

            if (players.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    canvasMaster.SetActive(true);
                    canvasClient.SetActive(false);
                    canvas = canvasMaster.transform.GetChild(0).gameObject.GetComponent<Canvas>();
                }
                else
                {
                    canvasMaster.SetActive(false);
                    canvasClient.SetActive(true);
                    canvas = canvasClient.transform.GetChild(1).gameObject.GetComponent<Canvas>();
                }
            }
            else
            {
                canvasMaster.SetActive(false);
                canvasClient.SetActive(true);
                canvas = canvasClient.transform.GetChild(1).gameObject.GetComponent<Canvas>();
            }

            
            ActiveActions();
        }

        private void Update()
        {
            if (!foundPlayer) foundPlayer = FindPlayer();
            SetArrowWind();
            energies.text = PhotonNetwork.LocalPlayer.CustomProperties[LlamaradaGame.PLAYER_TURN].ToString();
            if (contSetName < 100) SetNamePlayerUI();
        }
        #endregion

        public void ActiveActions()
        {
            object maxEnergies = PhotonNetwork.LocalPlayer.CustomProperties[LlamaradaGame.PLAYER_ENERGIES];
            for (int i = 0; i < 5; i++)
            {
                if(i < (int)maxEnergies)
                    actions[i].gameObject.SetActive(true);
                else
                    actions[i].gameObject.SetActive(false);
            }
        }

        #region PUN CALLBACKS
        
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            ActiveActions();
            //Debug.Log("se actualizo una propiedad personalizada");
        }

        
        
        #endregion  

        #region FindPlayer
        bool FindPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null) return false;
            pv = player.GetComponent<PhotonView>();

            //activo botones 
            if (pv.IsMine)
            {
                controller = player.GetComponent<PlayerController>();
                GameObject canvasPlayer = player.transform.GetChild(1).gameObject;
                buttons = canvasPlayer.transform.GetChild(1).gameObject;

                for (int i = 0; i < buttons.transform.childCount; i++)
                {
                    allButtons[i] = buttons.transform.GetChild(i).gameObject;
                }
                SetButtons(allButtons);
            }

            ChangeCam();

            return true;
        }

        void SetNamePlayerUI()
        {
            GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in playersGO)
            {
                //Debug.Log("Nombre del jugador: " + player.GetComponent<PhotonView>().Owner.NickName);
                player.GetComponent<PlayerController>().Initialize(player.GetComponent<PhotonView>().Owner.NickName);
                if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length > 1) player.GetComponent<PlayerController>().namePlayer.fontSize = 2f;
            }
            contSetName++;
        }
        #endregion

        #region Directionwind
        public void SetArrowWind()
        {
            int directionWind = BoardManager.directionWind;
            int grade = 0;
            switch (directionWind)
            {
                case 0:
                    grade = 0;
                    break;
                case 1:
                    grade = -90;
                    break;
                case 2:
                    grade = 180;
                    break;
                case 3:
                    grade = 90;
                    break;
                default:
                    break;
            }

            RectTransform transform;

            if (players.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    transform = arrowMaster.GetComponent<RectTransform>();
                    transform.localRotation = Quaternion.Euler(0, 0, grade);
                }
                else
                {
                    transform = arrowClient.GetComponent<RectTransform>();
                    transform.localRotation = Quaternion.Euler(0, 0, grade);
                }
            }
            else
            {
                transform = arrowClient.GetComponent<RectTransform>();
                transform.localRotation = Quaternion.Euler(0, 0, grade);
            }
        }
        #endregion

        #region Methods Call
        //TODO: BUSCAR EL PLAYERCONTROLLER PARA LLAMAR AL METODO DE DETECCION DE EDIFICIO
        public void CallDetectEdificeToMove()
        {
            if (showMove && player != null && pv.IsMine) controller.GetComponent<DetectEdifice>().DetectEdificeToMovePerson();
        }

        public void CallDetectEdificeTakeOutHabitant()
        {
            if (player != null && pv.IsMine)
            {
                if(!vCam1.activeSelf) ActivateDeactivateCams(true, false, false);
                if (showMove)
                {
                    showMove = false;
                    HidePanelMove();
                }

                controller.GetComponent<DetectEdifice>().DetectEdificeTakeOutHabitant();
                controller.GetComponent<DetectEdifice>().HideInspect();
            }
        }

        public void CallDetectEdificeToInspect()
        {
            if (player != null && pv.IsMine)
            {
                if (!vCam1.activeSelf) ActivateDeactivateCams(true, false, false);
                if (showMove)
                {
                    showMove = false;
                    HidePanelMove();
                } 
                controller.GetComponent<DetectEdifice>().DetectEdificeToInspect();
                controller.GetComponent<DetectEdifice>().HideTakeOutHabitant();
            }
        }

        public void CallHideAllButtonsInspect()
        {
            if (player != null && pv.IsMine)
            {
                //controller.HideAllButtonsInspect();
                controller.GetComponent<DetectEdifice>().HideAllButtonsInspect();
            }
        }
        #endregion

        #region SetButtons
        private void SetButtons(GameObject[] buttons)
        {
            up = buttons[0];
            down = buttons[1];
            right = buttons[2];
            left = buttons[3];
            upRight = buttons[4];
            upLeft = buttons[5];
            downRight = buttons[6];
            downLeft = buttons[7];
            leftUp = buttons[8];
            rightUp = buttons[9];
            rightDown = buttons[10];
            leftDown = buttons[11];
        }
        #endregion

        #region ChangeCam
        public void ActivateDeactivateCams(bool cam1, bool cam2, bool master)
        {            
            if (vCam2.activeSelf)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<ActivatingDectectionZone>().DeactivateZone();
            }
            player.GetComponent<ActivatingDectectionZone>().DeactivateZone();
            vCam1.SetActive(cam1);
            vCam2.SetActive(cam2);
            vCamMaster.SetActive(master);
            if (cam1 || cam2) camera.GetComponent<Camera>().orthographic = true;            
            if (master) camera.GetComponent<Camera>().orthographic = false;
        }

        public void ChangeCam()
        {
            if(PhotonNetwork.PlayerList.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient) ActivateDeactivateCams(false, false, true);
                else ActivateDeactivateCams(true, false, false);
            }
            
        }

        public void ZoomIn()
        {
            ActivateDeactivateCams(true, false, false);
        }
        public void ZoomOut()
        {
            ActivateDeactivateCams(false, false, true);            
        }

        public void ChangeCamMinimap(string target)
        {
            
            if (vCam2.activeSelf)
            {
                ActivateDeactivateCams(true, false, false);                
            }
            else if (vCam1.activeSelf)
            {
                ActivateDeactivateCams(false, true, false);
                // Asigno al player como objetivo para que la camara lo siga
                CinemachineVirtualCamera vCam = vCam2.GetComponent<CinemachineVirtualCamera>();
                if (vCam.Follow == null) vCam.Follow = GameObject.FindGameObjectWithTag(target).transform;
                GameObject player = GameObject.FindGameObjectWithTag(target);
                player.GetComponent<ActivatingDectectionZone>().ActivateZone();
            }

            if (showMove)
            {
                showMove = false;
                HidePanelMove();
            }
            controller.GetComponent<DetectEdifice>().HideInspect();
            controller.GetComponent<DetectEdifice>().HideTakeOutHabitant();
        }

        public void ChangeCamMinimapTransform(Transform position)
        {
            ActivateDeactivateCams(false, false, false);
            // Asigno al target como objetivo para que la camara lo siga
            //CinemachineVirtualCamera vCam = vCam2.GetComponent<CinemachineVirtualCamera>();
            //vCam.m_Lens.OrthographicSize = 20f;
            //if (vCam.Follow == null) vCam.Follow = position;
        }

        #endregion

        #region Animation        
        
        public void TogglePanel()
        {

            ChangeCam();

            if (!boton.gameObject.activeSelf)
            {
                boton.gameObject.SetActive(true);
                boton.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
                showingPanel = true;
            }
            else
            {
                if (showingPanel)
                {
                    boton.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Children);
                    ShowPanelUI();
                }
                else
                    boton.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Children);
                showingPanel = !showingPanel;
            }
        }

        // ANIMACION CAMBIO DE TURNO
        #region AnimacionCambioTurno
        public void AnimationChangeTurn()
        {
            if (!firtTurn)
            {
                Debug.Log("cambio de turno");
                if (players.Length > 1)
                {
                    if (PhotonNetwork.IsMasterClient)
                        StartCoroutine(MoveInPanelChangeTurn(panelChangeTurnMaster));
                    else
                        StartCoroutine(MoveInPanelChangeTurn(panelChangeTurnClient));
                }
                else
                    StartCoroutine(MoveInPanelChangeTurn(panelChangeTurnClient));

                // Disable all scene switch buttons
                GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, false);                
            }else
                firtTurn = false;

        }

        IEnumerator MoveInPanelChangeTurn(GUIAnimFREE panelChangeTurn)
        {
            yield return new WaitForSeconds(.2f);
            panelChangeTurn.gameObject.SetActive(true);
            panelChangeTurn.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
            StartCoroutine(MoveOutPanelChangeTurn(panelChangeTurn));
        }

        IEnumerator MoveOutPanelChangeTurn(GUIAnimFREE panelChangeTurn)
        {
            yield return new WaitForSeconds(1.5f);

            panelChangeTurn.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
            panelChangeTurn.gameObject.SetActive(false);
            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }
        #endregion
        
        //ANIMACION AVANCE DEL FUEGO
        #region AnimacionAdvanceFire
        public void AnimationAdvanceOfFire()
        {
            if (players.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(MoveInPanelAdvanceOfFire(panelAdvanceFireMaster));
                else
                    StartCoroutine(MoveInPanelAdvanceOfFire(panelAdvanceFireClient));
            }
            else
                StartCoroutine(MoveInPanelAdvanceOfFire(panelAdvanceFireClient));

            // Disable all scene switch buttons
            GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, false);
        }

        IEnumerator MoveInPanelAdvanceOfFire(GUIAnimFREE panelAdvanceOfFire)
        {
            yield return new WaitForSeconds(1.8f);
            panelAdvanceOfFire.gameObject.SetActive(true);
            panelAdvanceOfFire.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
            StartCoroutine(MoveOutPanelAdvanceOfFire(panelAdvanceOfFire));
        }

        IEnumerator MoveOutPanelAdvanceOfFire(GUIAnimFREE panelAdvanceOfFire)
        {
            yield return new WaitForSeconds(1.5f);

            panelAdvanceOfFire.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
            panelAdvanceOfFire.gameObject.SetActive(false);
            // Enable all scene switch buttons
            //StartCoroutine(EnableAllDemoButtons());
            AnimationChangeWind();
        }
        #endregion

        //ANIMACION AVANCE DEL FUEGO
        #region AnimacionChangeWind
        public void AnimationChangeWind()
        {
            if (players.Length > 1)
            {
                if (PhotonNetwork.IsMasterClient)
                    StartCoroutine(MoveInPanelChangeWind(panelChangeWindMaster));
                else
                    StartCoroutine(MoveInPanelChangeWind(panelChangeWindClient));
            }
            else
                StartCoroutine(MoveInPanelChangeWind(panelChangeWindClient));

            // Disable all scene switch buttons
            //GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, false);
        }

        IEnumerator MoveInPanelChangeWind(GUIAnimFREE panelChangeWind)
        {
            yield return new WaitForSeconds(.3f);
            panelChangeWind.gameObject.SetActive(true);
            panelChangeWind.PlayInAnims(GUIAnimSystemFREE.eGUIMove.Self);
            StartCoroutine(MoveOutPanelChangeWind(panelChangeWind));
        }

        IEnumerator MoveOutPanelChangeWind(GUIAnimFREE panelChangeWind)
        {
            yield return new WaitForSeconds(1.5f);

            panelChangeWind.PlayOutAnims(GUIAnimSystemFREE.eGUIMove.Self);
            panelChangeWind.gameObject.SetActive(false);
            // Enable all scene switch buttons
            StartCoroutine(EnableAllDemoButtons());
        }
        #endregion

        // Enable/Disable all scene switch Coroutine
        IEnumerator EnableAllDemoButtons()
        {
            yield return new WaitForSeconds(.5f);
            // Enable all scene switch buttons
            GUIAnimSystemFREE.Instance.SetGraphicRaycasterEnable(canvas, true);
        }



        #endregion

        #region Show/Hide Panel-Buttons

        public void HideAllButtonsMove()
        {
            up.SetActive(false);
            down.SetActive(false);
            right.SetActive(false);
            left.SetActive(false);
            rightUp.SetActive(false);
            rightDown.SetActive(false);
            leftUp.SetActive(false);
            leftDown.SetActive(false);
            upRight.SetActive(false);
            upLeft.SetActive(false);
            downRight.SetActive(false);
            downLeft.SetActive(false);
        }

        // PANEL MOVE
        public void ShowPanelMove()
        {
            if (!vCam1.activeSelf)
            {
                ActivateDeactivateCams(true, false, false);
            }
            if (!showMove)
            {
                buttons.SetActive(true);
                showMove = true;
            }
            else
            {
                showMove = false;
                HidePanelMove();
            }
            controller.GetComponent<DetectEdifice>().HideInspect();
            controller.GetComponent<DetectEdifice>().HideTakeOutHabitant();
        }

        public void HidePanelMove()
        {
            buttons.SetActive(false);
            HideAllButtonsMove();
        }
        
        //  PANEL UI
        public void ShowPanelUI()
        {
            panelUI.SetActive(true);
        }

        public void HidePanelUI()
        {
            panelUI.SetActive(false);
        }

        // BUTTONS
        public void ShowButtonsActions()
        {
            panelButtons.SetActive(true);
        }

        public void HideButtonsActions()
        {
            panelButtons.SetActive(false);
        }

        //  PANEL ACTIONS
        public void ShowPanelActions()
        {
            panelActions.SetActive(true);
        }

        public void HidePanelActions()
        {
            panelActions.SetActive(false);
        }

        //  PANEL MINIMAP
        public void ShowPanelMinimap()
        {
            //minimapCamera.SetActive(true);
            //minimapCamera.GetComponent<Minimap>().FindPlayer();
        }

        public void HidePanelMinimap()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActivatingDectectionZone>().DeactivateZone();
        }

        //PANEL NOTIFICATIONS
        public void ShowPanelNotification(string notification)
        {
            panelNotification.SetActive(true);
            textNotification.text = notification;
            StartCoroutine(HideNotification());
        }

        IEnumerator HideNotification()
        {
            yield return new WaitForSeconds(1f);
            panelNotification.SetActive(false);
        }

        public void HidePanelNotification()
        {
            panelNotification.SetActive(false);
        }


        // PANEL SCORE
        public void UpdateScoreSavedText(int score)
        {
            textScoreSaved.text = score.ToString();
        }

        public void UpdateScoreDeadText(int score)
        {
            textScoreDead.text = score.ToString();
        }

        //PANEL LOAD LEVEL

        void CallDesactivatePanelLevel()
        {
            PhotonView pv = GetComponent<PhotonView>();
            pv.RPC("DeactivatePanelLoalLevel", RpcTarget.AllBuffered);
        }
        [PunRPC]
        void DeactivatePanelLoalLevel()
        {
            Debug.Log("desactivar panel");
            panelLoadLevel.SetActive(false);
        }

        #endregion

    }
}