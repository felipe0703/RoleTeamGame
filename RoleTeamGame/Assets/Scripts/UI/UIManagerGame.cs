
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Cinemachine;
#endregion //Namespaces

namespace Com.BrumaGames.Llamaradas
{
    public class UIManagerGame : MonoBehaviour
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables
        public static UIManagerGame sharedInstance;

        public GameObject panelButtons;
        public GameObject buttons;
        public GameObject panelUI;
        public GameObject panelActions;
        public GameObject panelMinimap;
        public GameObject panelNotification;
        public TextMeshProUGUI textNotification;
        public GameObject vCam1;
        public GameObject vCam2;
        public GameObject minimapCamera;
        public GameObject vCamMinimap;
        public GameObject vCamHabitant;
        
        public CinemachineVirtualCamera cine;
        public GameObject arrow;

        [Space(10)]
        public TextMeshProUGUI textTurn;
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
        #endregion

        // ########################################
        // Funciones MonoBehaviour 
        // ########################################

        #region MonoBehaviour
        private void Start()
        {
            if (sharedInstance == null)
            {
                sharedInstance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            pvUI = GetComponent<PhotonView>();

        }

        private void Update()
        {
            if (!foundPlayer)
            {
               foundPlayer = FindPlayer();
            }

            SetArrowWind();
        }
        #endregion

        #region FindPlayer
        bool FindPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                return false;

            pv = player.GetComponent<PhotonView>();

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

            RectTransform transform = arrow.GetComponent<RectTransform>();
            transform.localRotation =  Quaternion.Euler(0,0,grade);
        }
        #endregion

        #region Methods Call
        //TODO: BUSCAR EL PLAYERCONTROLLER PARA LLAMAR AL METODO DE DETECCION DE EDIFICIO
        public void CallDetectEdificeToMove()
        {
            if (player != null && pv.IsMine)
            {
                //controller.DetectEdificeToMove();
                controller.GetComponent<DetectEdifice>().DetectEdificeToMovePerson();
            }
        }

        public void CallDetectEdificeTakeOutHabitant()
        {
            if (player != null && pv.IsMine)
            {
                controller.GetComponent<DetectEdifice>().DetectEdificeTakeOutHabitant();
            }
        }

        public void CallDetectEdificeToInspect()
        {
            if (player != null && pv.IsMine)
            {
                //controller.DetectEdificeToInspect();
                controller.GetComponent<DetectEdifice>().DetectEdificeToInspect();
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
        public void ActivateDeactivateCams(bool cam1, bool cam2, bool minimap, bool habitant)
        {
            vCam1.SetActive(cam1);
            vCam2.SetActive(cam2);
            vCamMinimap.SetActive(minimap);
            vCamHabitant.SetActive(habitant);
        }

        public void ChangeCam()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                ActivateDeactivateCams(true, false, false, false);
                cine.m_Lens.OrthographicSize = 15f;
            }
            else
            {
                ActivateDeactivateCams(true, false, false, false);
                cine.m_Lens.OrthographicSize = 15f;
            }
        }


        public void ChangeCamMinimap(string target)
        {
            ActivateDeactivateCams(false, false, true, false);
            // Asigno al player como objetivo para que la camara lo siga
            CinemachineVirtualCamera vCam = vCamMinimap.GetComponent<CinemachineVirtualCamera>();
            vCam.m_Lens.OrthographicSize = 30f;
            if (vCam.Follow == null)
                vCam.Follow = GameObject.FindGameObjectWithTag(target).transform;
            GameObject player = GameObject.FindGameObjectWithTag(target);
            player.GetComponent<ActivatingDectectionZone>().ActivateZone();
        }
        public void ChangeCamMinimapTransform(Transform position)
        {
            ActivateDeactivateCams(false, false, false, true);
            // Asigno al target como objetivo para que la camara lo siga
            CinemachineVirtualCamera vCam = vCamHabitant.GetComponent<CinemachineVirtualCamera>();
            vCam.m_Lens.OrthographicSize = 20f;
            if (vCam.Follow == null)
                vCam.Follow = position;
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
            buttons.SetActive(true);
            //HidePanelUI();
            HidePanelActions();
            HideButtonsActions();
        }

        public void HidePanelMove()
        {
            buttons.SetActive(false);
            //ShowPanelUI();
            ShowPanelActions();
            ShowButtonsActions();
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
            panelMinimap.SetActive(true);
            minimapCamera.SetActive(true);
            minimapCamera.GetComponent<Minimap>().FindPlayer();
        }

        public void HidePanelMinimap()
        {
            panelMinimap.SetActive(false);
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<ActivatingDectectionZone>().DeactivateZone();
        }

        //PANEL NOTIFICATIONS
        public void ShowPanelNotification(string notification)
        {
            panelNotification.SetActive(true);
            textNotification.text = notification;
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
        #endregion

    }

}
