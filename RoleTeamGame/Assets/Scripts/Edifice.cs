#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
#endregion //Namespace


namespace Com.BrumaGames.Llamaradas
{
    #region FireState
    // Estados del fuego en los edificios
    public enum FireState
    {
        level0,
        level1,
        level2,
        level3,
        level4
    }
    #endregion //FireState

    public class Edifice : MonoBehaviour
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables

        public int id;
        public int idTypeOfEdifice;
        [SerializeField] Sprite spriteBurnedEdifice;
        [SerializeField] Sprite spriteTurnedEdifice;

        PhotonView pv;

        public GameObject btn;
        public Transform street2;
        public bool BurnedEdifice;

        [Space(10)]
        [Header("Level Fire")]
        public GameObject level1;
        public GameObject level2;
        public GameObject level3;
        public GameObject level4;

        [Space(10)]
        [Header("Habitants")]
        public Button[] habitants;
        public Sprite[] imageHabitants;

        public int contPopulation;
        public int contDisabledPerson;
        public int contPerson;
        public int contPet;

        [Space(10)]
        [Header("Fire")]
        public int maxFire;
        public int contFire;


        public bool isInspected = false;
        public bool isSelected = false;
        public bool isSelectedOutHabitant = false;
        public int idPositionEdifice = -1;

        //AUDIO
        private AudioSource edificeAudio;
        [Space(10)]
        [Header("Audio")]
        public AudioClip[] clips;
        SfxControl ScriptEfectos;


        //TEST
        [Space(10)]
        [Header("TEST")]
        public TextMeshProUGUI[] texts;

        #endregion // Variables

        // ########################################
        // Funciones MonoBehaviour 
        // ########################################

        #region MonoBehaviour

        public void Start()
        {

            edificeAudio = gameObject.GetComponent<AudioSource>();
            edificeAudio.pitch = 1.36f;
            ScriptEfectos = GameObject.Find("Sound/Efectos interaccion").GetComponent<SfxControl>();

            int disabledPerson = GameController.totalDisabledPerson;
            int person = GameController.totalPerson;
            int pet = GameController.totalPet;
            int population = disabledPerson + person + pet;
            idPositionEdifice = -1;

        }

        private void Update()
        {
            //TEXTO DE PRUEBA
            
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = "-";
            }

            if (PhotonNetwork.IsMasterClient)
            {
                texts[0].text = GameController.sharedInstance.populationInEdifices[id, 0].ToString();
                texts[1].text = GameController.sharedInstance.populationInEdifices[id, 1].ToString();
                texts[2].text = GameController.sharedInstance.populationInEdifices[id, 2].ToString();
            }
            else
            {
                texts[0].text = GameController.sharedInstance.populationInEdificesClient[id, 0].ToString();
                texts[1].text = GameController.sharedInstance.populationInEdificesClient[id, 1].ToString();
                texts[2].text = GameController.sharedInstance.populationInEdificesClient[id, 2].ToString();
            }
            
        }

        #endregion // Monobehaviour

        
        // ########################################
        // Funciones IsSelected y Audio
        // ########################################

        #region IsSelected/Audio

        
        public void IsSelected()
        {
            pv = GetComponent<PhotonView>();
            pv.RPC("SyncUpSpriteAndHabitant", RpcTarget.AllBufferedViaServer );
            isSelected = false;
            
            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            controller.UpdateActions();
            
            //controller.HideAllButtonsInspect();
            controller.GetComponent<DetectEdifice>().HideAllButtonsInspect();
        }

        [PunRPC]
        void SyncUpSpriteAndHabitant()
        {
            ChangeSpriteTurned();
            isInspected = true;
            FillHabitant();
        }

        //TODO: EVALUAR SI ES CONVENIENTE TENER ESTA FUNCIONALIDAD EN ESTE SCRIPT O EN EL GAMECONTROLLER
        public void IsSelectedTakeOut(int id)
        {
            if (!BurnedEdifice) //si el edificio no esta quemado
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                PlayerController controller = player.GetComponent<PlayerController>();
                GameObject habitant = null;
                isSelectedOutHabitant = false;
                player.GetComponent<DetectEdifice>().HideTakeOutHabitant();

                // detecto que tipo de habitante seleccione
                if (habitants[id].GetComponent<ButtonHabitant>().idHabitant == 0)
                {
                    habitant = GameController.sharedInstance.disabledPerson;
                }
                else if (habitants[id].GetComponent<ButtonHabitant>().idHabitant == 1)
                {
                    habitant = GameController.sharedInstance.person;
                }
                else if (habitants[id].GetComponent<ButtonHabitant>().idHabitant == 2)
                {
                    Debug.Log("detecte un perro");
                    habitant = GameController.sharedInstance.pet;
                    int scoreHabitant = 1;
                    controller.UpdateScoreSaved(scoreHabitant);
                }

                pv = GetComponent<PhotonView>();
                pv.RPC("UpdatePopulationInEdifice", RpcTarget.AllBuffered, this.id, habitants[id].GetComponent<ButtonHabitant>().idHabitant, id);

                gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);//vuelve el edificio a su color original
                HabitantsNotInteractable();

                //SPAWNEAR HABITANTES EN LAS CALLES
                // tamaño de un arreglo de booleanos que determina si la calle esta ocupada
                int sizePosition = controller.street.GetComponent<HabitantsInTheStreet>().positions.Length;

                // recorre el arreglo de booleanos de las posiciones de las calles
                // si encuentra una calle vacia posiciona ahí al habitante del edificio
                // si es un edificio sobre el jugador posiciona al habitante arriba en la calle
                // si es un edificio bajo el jugador posiciona al habitante abajo en la calle
                for (int i = 0; i < sizePosition; i++)
                {
                    if (!controller.street.GetComponent<HabitantsInTheStreet>().positions[i])
                    {
                        if ((idPositionEdifice == 0 || idPositionEdifice == 3) && i < 3)
                        {
                            //Instantiate(habitant, controller.street.transform.GetChild(i).transform);
                            PhotonNetwork.Instantiate(habitant.name, controller.street.transform.GetChild(i).position, Quaternion.identity);
                            controller.street.GetComponent<HabitantsInTheStreet>().positions[i] = true;
                            ScriptEfectos.PlayDoorFx(id);
                            if (habitants[id].GetComponent<ButtonHabitant>().idHabitant != 2) controller.UpdateActions();
                            return;
                        }
                        if ((idPositionEdifice == 1 || idPositionEdifice == 2) && i > 2)
                        {
                            //Instantiate(habitant, controller.street.transform.GetChild(i).transform);
                            PhotonNetwork.Instantiate(habitant.name, controller.street.transform.GetChild(i).position, Quaternion.identity);
                            controller.street.GetComponent<HabitantsInTheStreet>().positions[i] = true;
                            ScriptEfectos.PlayDoorFx(id);
                            if (habitants[id].GetComponent<ButtonHabitant>().idHabitant != 2) controller.UpdateActions();
                            return;
                        }

                    }
                }
            }            
        }

        void HabitantsNotInteractable()
        {
            for (int i = 0; i < 3; i++)
            {
                habitants[i].interactable = false;
            }
        }

        [PunRPC]
        void UpdatePopulationInEdifice(int idEdifice, int idHabitant, int id)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.Log("cantidad: " + GameController.sharedInstance.populationInEdifices[idEdifice, idHabitant]);

                GameController.sharedInstance.populationInEdifices[idEdifice, idHabitant]--;
                //Debug.Log("idEdifice: " + idEdifice + " IdHabitant: " + idHabitant + " habitants: " +GameController.sharedInstance.populationInEdifices[idEdifice, idHabitant]);
            }
            else
            {
                //Debug.Log("cantidad: " + GameController.sharedInstance.populationInEdificesClient[idEdifice, idHabitant]);

                GameController.sharedInstance.populationInEdificesClient[idEdifice, idHabitant]--;
                //Debug.Log("idEdifice: " + idEdifice + " IdHabitant: " + idHabitant + " habitants: " + GameController.sharedInstance.populationInEdificesClient[idEdifice, idHabitant]);

            }
            habitants[id].image.enabled = false;
            habitants[id].gameObject.SetActive(false);

        }


        public void PlayClickSound()
        {
            edificeAudio.PlayOneShot(clips[0]);
        }

        // rellena los sprite de los habitantes en el edificio
        void FillHabitant()
        {
            //Debug.Log("fill habitant");
            int disabledPerson = 0;
            int person = 0;
            int pet = 0;
            //TODO: buscar los contadores de cada edificio y asignarlos
            if (PhotonNetwork.IsMasterClient)
            {
                //Debug.Log("boton fill");
                disabledPerson = GameController.sharedInstance.populationInEdifices[id, 0];
                person = GameController.sharedInstance.populationInEdifices[id, 1];
                pet = GameController.sharedInstance.populationInEdifices[id, 2];
            }
            else
            {
                //Debug.Log("no soy el master");
                disabledPerson = GameController.sharedInstance.populationInEdificesClient[id, 0];
                person = GameController.sharedInstance.populationInEdificesClient[id, 1];
                pet = GameController.sharedInstance.populationInEdificesClient[id, 2];
            }
           
            for (int i = 0; i < 3; i++)
            {
                if (disabledPerson > 0)
                {
                    habitants[i].image.enabled = true;
                    habitants[i].image.sprite = imageHabitants[0];
                    habitants[i].GetComponent<ButtonHabitant>().idHabitant = 0;
                    disabledPerson--;
                }
                else if (person > 0)
                {
                    habitants[i].image.enabled = true;
                    habitants[i].image.sprite = imageHabitants[1];
                    habitants[i].GetComponent<ButtonHabitant>().idHabitant = 1;
                    person--;
                }
                else if (pet > 0)
                {
                    habitants[i].image.enabled = true;
                    habitants[i].image.sprite = imageHabitants[2];
                    habitants[i].GetComponent<ButtonHabitant>().idHabitant = 2;
                    pet--;
                    habitants[i].interactable = true;


                    //buscar calle disponible, identifica en que dirección está el edificio
                    PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                    controller.GetComponent<DetectEdifice>().DetectPetInEdifice();
                    //saco al perro
                    IsSelectedTakeOut(i);
                   // Debug.Log("hay un perro, posicion: " + i);
                }

                // habitants[i].preserveAspect = true;
            }
        }
        

        #endregion // Audio

        // ########################################
        // Funciones Estados del fuego
        // ########################################

        #region FireState

        public void StartFireLevel0()
        {
            SetFireState(FireState.level0);
        }
        public void StartFireLevel1()
        {
            SetFireState(FireState.level1);
        }
        public void StartFireLevel2()
        {
            SetFireState(FireState.level2);
        }
        public void StartFireLevel3()
        {
            SetFireState(FireState.level3);
        }
        public void StartFireLevel4()
        {
            SetFireState(FireState.level4);
        }


        void SetFireState(FireState newFireState)
        {
            //TODO: cambiar el color de los sprite cuando se comience a quemar

            if (newFireState == FireState.level0)
            {
                //CallStartFireNeighbor();
                level1.SetActive(false);
                level2.SetActive(false);
                level3.SetActive(false);
                level4.SetActive(false);
                contFire = 0;
            }

            if (newFireState == FireState.level1)
            {
                //CallStartFireNeighbor();
                level1.SetActive(true);
                level2.SetActive(false);
                level3.SetActive(false);
                level4.SetActive(false);
                contFire = 1;
            }

            if (newFireState == FireState.level2)
            {
                level1.SetActive(false);
                level2.SetActive(true);
                level3.SetActive(false);
                level4.SetActive(false);
                contFire = 2;
                //CallFireLevel2();
            }

            if (newFireState == FireState.level3)
            {
                contFire = 3;
                level1.SetActive(false);
                level2.SetActive(false);
                level3.SetActive(true);
                level4.SetActive(false);
                //CallFireLevel3();
            }

            if (newFireState == FireState.level4)
            {
                contFire = 4;
                level1.SetActive(false);
                level2.SetActive(false);
                level3.SetActive(false);
                level4.SetActive(true);
                //CallFireLevel4();
            }
        }
        #endregion //FireState

        #region GetNeighborEdifice

        public GameObject GetNeighborEdifice(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 20f, LayerMask.GetMask("Edifice"));
            if (hit.collider != null)
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region FireStart
        //fuego en el primer edificio, donde comienza el incendio
        public void CallFireStart()
        {
            
            pv = GetComponent<PhotonView>();
            pv.RPC("FireStartInEdifice", RpcTarget.AllBuffered);
        }

        [PunRPC]
        void FireStartInEdifice()
        {
            //bool  next = false;
            //encontrar el boarmanager
            /* do
             {
                 Debug.Log("no lo encuentro");
                 if (BoardManager.sharedInstance != null)
                 {
                     next = true;
                     Debug.Log("en el while");
                 }
                 else
                     Debug.Log("no seta");
             } while (!next);
             */
            
            ChangeSprite();
            BurnedEdifice = true;

            if (GameObject.Find("BoardManager(Clone)") == null)
                Debug.Log("no hay boardmanager");
            else
                Debug.Log("encontre el boarmanager");

            BoardManager.sharedInstance.setCont = true;

            if (idTypeOfEdifice == 1)
            {
                StartFireLevel3();
            }
            else if (idTypeOfEdifice == 2)
            {
                StartFireLevel4();
            }
            else
            {
                StartFireLevel2();
            }
        }
        #endregion

        #region FireStartNeighbor

        // inicia el fuego en los edificios vecinos
        public void CallStartFireNeighbor(int viewId)
        {
            pv = this.GetComponent<PhotonView>();
            if( pv.ViewID == viewId && contFire == 1 && PhotonNetwork.IsMasterClient)
            {
                pv.RPC("StartFireNeighbor", RpcTarget.AllViaServer, viewId);
            }            
        }
        
        [PunRPC]
        void StartFireNeighbor(int viewId)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            level3.SetActive(false);
            level4.SetActive(false);
        }
        #endregion

        #region FireLevel
        
        public void CallFireLevel()
        {
            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (contFire == maxFire)
            {
                controller.UpdateScoreDead(TotalScoreDeadHabitant());
                ChangeSprite();
                BurnedEdifice = true;
                GameController.sharedInstance.UpdateTotalBurnedEdifice();
            }

            pv = GetComponent<PhotonView>();
            Debug.Log("ejecutar el level fuego");
            pv.RPC("FireLevel", RpcTarget.AllBuffered);
        }

        [PunRPC]
        void FireLevel()
        {
            Debug.Log("level fuego");
            if (contFire == 0)
            {
                StartFireLevel0();
            }
            else if (contFire == 1)
            {
                StartFireLevel1();
            }
            else if (contFire == 2)
            {
                StartFireLevel2();
            }
            else if (contFire == 3)
            {
                StartFireLevel3();
            }
            else if (contFire == 4)
            {
                StartFireLevel4();
            }
        }

        [PunRPC]
        public void CallFireLevelAll(bool addFire)
        {

            if (contFire >= 4)
                return;

            if (addFire && contFire >= 0)
                contFire++;
            else if (contFire > 0)
                contFire--;
            else
                return;
            


            Debug.Log("contador fire: " + contFire);
            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (contFire == maxFire)
            {
                controller.UpdateScoreDead(TotalScoreDeadHabitant());
                ChangeSprite();
                BurnedEdifice = true;
                GameController.sharedInstance.UpdateTotalBurnedEdifice();
            }

            pv = GetComponent<PhotonView>();
            pv.RPC("FireLevel", RpcTarget.AllBuffered);
        }

        public void SelectEdificeForIncreaseFire()
        {
            pv = GetComponent<PhotonView>();
            pv.RPC("CallFireLevelAll", RpcTarget.AllViaServer, true);

        }

        public void SelectEdificeForDecreaseFire()
        {
            pv = GetComponent<PhotonView>();
            pv.RPC("CallFireLevelAll", RpcTarget.AllViaServer, false);
        }

        #endregion

        public void ActivateDetectBurnedPlayer(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 13f, LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                //return hit.collider.gameObject;
                Debug.Log("detecte un jugador " + this.gameObject.name);
                hit.collider.gameObject.GetComponent<PlayerController>().PlayerLosesTurn();
            }
            else
            {
                Debug.Log("no hay jugador " + this.gameObject.name);
                //return null;
            }
        }
           
        void ChangeSprite()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteBurnedEdifice;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

            for (int i = 0; i < habitants.Length; i++)
            {
                habitants[i].gameObject.SetActive(false);
            }
        }

        void ChangeSpriteTurned()
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = spriteTurnedEdifice;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }

        int TotalScoreDeadHabitant()
        {
            int disabledPerson;
            int person;
            int pet;

            if (PhotonNetwork.IsMasterClient)
            {
                disabledPerson = GameController.sharedInstance.populationInEdifices[id, 0];
                person = GameController.sharedInstance.populationInEdifices[id, 1];
                pet = GameController.sharedInstance.populationInEdifices[id, 2];
            }
            else
            {
                disabledPerson = GameController.sharedInstance.populationInEdificesClient[id, 0];
                person = GameController.sharedInstance.populationInEdificesClient[id, 1];
                pet = GameController.sharedInstance.populationInEdificesClient[id, 2];
            }            

            int score = disabledPerson + person + pet;

            return score;
        }
    }
    
}
