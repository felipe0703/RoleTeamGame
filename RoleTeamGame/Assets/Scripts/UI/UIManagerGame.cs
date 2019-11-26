
#region Namespaces
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
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

        [Space(10)]
        public TextMeshProUGUI textTurn;
        public TextMeshProUGUI textPlayer;
        public TextMeshProUGUI textSetTurn;



        // public GameObject[] panelButtonsMove;

        // BUTTONS        
        private GameObject[] allButtons = new GameObject[12];
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

        GameObject player;
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

            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player"); 
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
            }
        }

        //USADO PARA PRUEBAS
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                pvUI.RPC("TestRPC", RpcTarget.AllViaServer);
            }
        }
        #endregion

        //USADO PARA PRUEBAS
        [PunRPC]
        private void TestRPC()
        {
            panelNotification.SetActive(true);
            textNotification.text = "esto es una prueba de rpc";
        }

        #region Methods Call
        //TODO: BUSCAR EL PLAYERCONTROLLER PARA LLAMAR AL METODO DE DETECCION DE EDIFICIO
        public void CallDetectEdificeToMove()
        {
            if (player != null && pv.IsMine)
            {                
                controller.DetectEdificeToMove();
            }
        }

        public void CallDetectEdificeTakeOutHabitant()
        {
            if (player != null && pv.IsMine)
            {
                controller.DetectEdificeTakeOutHabitant();
            }
        }

        public void CallDetectEdificeToInspect()
        {
            if (player != null && pv.IsMine)
            {
                controller.DetectEdificeToInspect();
            }
        }

        public void CallHideAllButtonsInspect()
        {
            if (player != null && pv.IsMine)
            {
                controller.HideAllButtonsInspect();
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

        #region Animation

        public void TogglePanel()
        {

            vCam1.SetActive(true);
            vCam2.SetActive(false);
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
        }

        public void HidePanelMinimap()
        {
            panelMinimap.SetActive(false);
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

        #endregion

    }

}
