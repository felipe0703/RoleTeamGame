using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.BrumaGames.Llamaradas
{
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
        [Header("Buttons")]
        public GameObject[] buttons = new GameObject[12];

        [Header("Layer")]
        public LayerMask mask;

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
        private GameObject GetNeighbor(Vector2 direction, LayerMask mask)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 40f, mask);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            else
            {
                Debug.Log("no detecto nada en Detectedifice");
                return null;
            }
        }

        private GameObject DetectionAddress(string direction)
        {
            switch (direction)
            {
                case "up":
                    return GetNeighbor(adjacentDirections[0], mask);
                case "right":
                    return GetNeighbor(adjacentDirections[1], mask);
                case "down":
                    return GetNeighbor(adjacentDirections[2], mask);
                case "left":
                    return GetNeighbor(adjacentDirections[3], mask);

                default:
                    return null;
            }
        }

        private GameObject GetButton(string btn)
        {
            switch (btn)
            {
                case "up":
                    return buttons[0];
                case "down":
                    return buttons[1];
                case "right":
                    return buttons[2];
                case "left":
                    return buttons[3];
                case "upRight":
                    return buttons[4];
                case "upLeft":
                    return buttons[5];
                case "downRight":
                    return buttons[6];
                case "downLeft":
                    return buttons[7];
                case "leftUp":
                    return buttons[8];
                case "rightUp":
                    return buttons[9];
                case "leftDown":
                    return buttons[10];
                case "rightDown":
                    return buttons[11];

                default:
                    return null;
            }
        }

        private void DetectionCornerUpDown()
        {
            // si tengo una esquina arriba
            if (DetectionAddress("up").GetComponent<Street>().isCorner)
            {
                GetButton("down").SetActive(true);
            }// si tengo una esquina abajo
            else if (DetectionAddress("down").GetComponent<Street>().isCorner)
            {
                GetButton("up").SetActive(true);
            }
            else
            {
                GetButton("up").SetActive(true);
                GetButton("down").SetActive(true);
            }
        }
        private void DetectionCornerRightLeft()
        {
            // si tengo una esquina derecha
            if (DetectionAddress("right").GetComponent<Street>().isCorner)
            {
                GetButton("left").SetActive(true);
            }// si tengo una esquina izquierda
            else if (DetectionAddress("left").GetComponent<Street>().isCorner)
            {
                GetButton("right").SetActive(true);
            }
            else
            {
                GetButton("right").SetActive(true);
                GetButton("left").SetActive(true);
            }
        }
        private void DetectionBorderUpDown()
        {
            // HAY BORDE ARRIBA
            if (DetectionAddress("up").GetComponent<Street>().isBorder)
            {
                GetButton("down").SetActive(true);
            }// HAY BORDE ABAJO
            else if (DetectionAddress("down").GetComponent<Street>().isBorder)
            {
                GetButton("up").SetActive(true);
            }
            else
            {
                GetButton("up").SetActive(true);
                GetButton("down").SetActive(true);
            }
        }
        private void DetectionBorderRightLeft()
        {
            // HAY BORDE DERECHA
            if (DetectionAddress("right").GetComponent<Street>().isBorder)
            {
                GetButton("left").SetActive(true);
            }// HAY BORDE IZQUIERDA
            else if (DetectionAddress("left").GetComponent<Street>().isBorder)
            {
                GetButton("right").SetActive(true);
            }
            else
            {
                GetButton("right").SetActive(true);
                GetButton("left").SetActive(true);
            }
        }
        private void ActivateDownRightAndUpRight()
        {
            GetButton("downRight").SetActive(true);
            GetButton("upRight").SetActive(true);
        }
        private void ActivateDonwLeftAndUpLeft()
        {
            GetButton("downLeft").SetActive(true);
            GetButton("upLeft").SetActive(true);
        }
        private void ActivateRightDownAndLeftDown()
        {
            GetButton("rightDown").SetActive(true);
            GetButton("leftDown").SetActive(true);
        }
        private void ActivateRightUpAndLeftUp()
        {
            GetButton("rightUp").SetActive(true);
            GetButton("leftUp").SetActive(true);
        }

        public void DetectEdificeToMovePerson()
        {
            //EDIFICIO A LA DERECHA y BOSQUE A LA IZQUIERDA
            if (DetectionAddress("right").tag == "Edifice" && DetectionAddress("left").tag == "Border")
            {
                DetectionCornerUpDown();
                ActivateDownRightAndUpRight();
                return;
            }

            //EDIFICIO A LA IZQUIEDA y AGUA A LA DERECHA
            if (DetectionAddress("left").tag == "Edifice" && DetectionAddress("right").tag == "River")
            {
                DetectionCornerUpDown();
                ActivateDonwLeftAndUpLeft();
                return;
            }

            //  EDIFICIOS A LA DERECHA E IZQUIERDA
            if (DetectionAddress("right").tag == "Edifice" && DetectionAddress("left").tag == "Edifice")
            {
                DetectionBorderUpDown();
                ActivateDonwLeftAndUpLeft();
                ActivateDownRightAndUpRight();
                return;
            }

            // EDIFICIOS ARRIBA Y ABAJO
            if (DetectionAddress("up").tag == "Edifice" && DetectionAddress("down").tag == "Edifice")
            {
                DetectionBorderRightLeft();
                ActivateRightUpAndLeftUp();
                ActivateRightDownAndLeftDown();
                return;
            }

            //EDIFICIO ABAJO && BORDE ARRIBA
            if (DetectionAddress("down").tag == "Edifice" && DetectionAddress("up").tag == "Border")
            {
                DetectionCornerRightLeft();
                ActivateRightDownAndLeftDown();
                return;
            }

            //EDIFICIO ARRIBA y BORDE ABAJO
            if (DetectionAddress("up").tag == "Edifice" && DetectionAddress("down").tag == "Border")
            {
                DetectionCornerRightLeft();
                ActivateRightUpAndLeftUp();
                return;
            }
        }


        // detecta si tenemos algun edificio a los lados y activa el boton del edificio
        public void DetectEdificeToInspect()
        {
            Debug.Log("detect edifice to inspect");
            GameObject edifice;
            bool isInspected = false;
            bool isSelected = false;

            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);
                    isSelected = edifice.GetComponent<Edifice>().isSelected;
                    if (!edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice)
                    {
                        if (isSelected)
                        {
                            edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                            edifice.GetComponent<Edifice>().isSelected = false;
                            edifice.GetComponent<Edifice>().btn.SetActive(false);
                        }
                        else
                        {
                            edifice.GetComponent<Edifice>().btn.SetActive(true);
                            edifice.GetComponent<Edifice>().isSelected = true;
                            edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);
                            isInspected = true;
                        }
                    }
                }
            }
            if (!isInspected && !isSelected) UIManagerGame.sharedInstance.ShowPanelNotification(I18nMng.GetText("noEdificesToShow"));
        }

        public void HideInspect()
        {
            GameObject edifice;
            bool isSelected = false;
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);
                    isSelected = edifice.GetComponent<Edifice>().isSelected;
                    if (!edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice)
                    {
                        if (isSelected)
                        {
                            edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                            edifice.GetComponent<Edifice>().isSelected = false;
                            edifice.GetComponent<Edifice>().btn.SetActive(false);
                        }
                    }
                }
            }
        }

        public void HideAllButtonsInspect()
        {
            GameObject edifice;

            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);
                    edifice.GetComponent<Edifice>().btn.SetActive(false);
                    edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                }
            }
        }

        public void DetectEdificeTakeOutHabitant()
        {
            GameObject edifice;
            bool detected = false;
            bool isSelected = false;
            //ver en las 4 direcciones
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);
                    isSelected = edifice.GetComponent<Edifice>().isSelectedOutHabitant;
                    if (edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice) // el edificio fue inspeccionado 
                    {
                        int cont = 0;
                        for (int j = 0; j < 3; j++)
                        {
                            if (edifice.GetComponent<Edifice>().habitants[j].image.enabled) // si hay habitantes
                            {
                                if (isSelected)
                                {
                                    edifice.GetComponent<Edifice>().habitants[j].interactable = false;
                                    edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                                    edifice.GetComponent<Edifice>().isSelectedOutHabitant = false;
                                }
                                else
                                {
                                    edifice.GetComponent<Edifice>().isSelectedOutHabitant = true;
                                    edifice.GetComponent<Edifice>().habitants[j].interactable = true;
                                    edifice.GetComponent<SpriteRenderer>().color = new Color(.85f, .85f, .85f, 0.3f);
                                    detected = true;
                                }
                            }
                            else cont++;
                        }              
                        if(cont == 3 && isSelected )
                        {
                            detected = false;
                            isSelected = false;
                        }
                        edifice.GetComponent<Edifice>().idPositionEdifice = i;
                    }
                }
            }
            //activa panel de notificacion
            if (!detected && !isSelected) UIManagerGame.sharedInstance.ShowPanelNotification(I18nMng.GetText("noInhabitantsToShow"));
        }

        public void HideTakeOutHabitant()
        {
            GameObject edifice;
            bool isSelected = false;
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);
                    isSelected = edifice.GetComponent<Edifice>().isSelectedOutHabitant;
                    if (edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice) // el edificio fue inspeccionado 
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (edifice.GetComponent<Edifice>().habitants[j].image.enabled) // si hay habitantes
                            {
                                if (isSelected)
                                {
                                    edifice.GetComponent<Edifice>().habitants[j].interactable = false;
                                    edifice.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
                                    edifice.GetComponent<Edifice>().isSelectedOutHabitant = false;
                                }
                            }
                        }
                        //edifice.GetComponent<Edifice>().idPositionEdifice = i;
                    }
                }
            }
        }


        public void DetectPetInEdifice()
        {
            GameObject edifice;

            //ver en las 4 direcciones
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                if (GetNeighbor(adjacentDirections[i], mask).tag == "Edifice")
                {
                    edifice = GetNeighbor(adjacentDirections[i], mask);

                    if (edifice.GetComponent<Edifice>().isInspected && !edifice.GetComponent<Edifice>().BurnedEdifice) // el edificio fue inspeccionado 
                    {
                        //ScriptEfectos.DetectEdifice(edifice);
                        //si no hay mascota en el edificio activo las imagnes
                        edifice.GetComponent<Edifice>().idPositionEdifice = i;
                    }
                }
            }
        }

        #endregion

        public void DeactivateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetActive(false);
            }

            UIManagerGame.sharedInstance.ChangeCam();
            UIManagerGame.sharedInstance.HidePanelMinimap();
            UIManagerGame.sharedInstance.ActivateDeactivateCams(false, false, false);
        }

    }
}