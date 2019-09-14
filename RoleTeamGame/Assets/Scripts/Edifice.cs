#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion //Namespace

#region FireState
// Estados del fuego en los edificios
public enum FireState
{
    level0,
    level1,
    level2,
    level3
}
#endregion //FireState


public class Edifice : MonoBehaviour
{
    // ########################################
    // Variables
    // ########################################

    #region Variables
    public int id;
    public GameObject btn;

    public Transform street2;


    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    public Button[] habitants;
    public Sprite[] imageHabitants;

    public int contPopulation;
    public int contDisabledPerson;
    public int contPerson;
    public int contPet;

    public int maxFire;
    public int contFire;

    public bool isInspected = false;
    public int idPositionEdifice = -1;

    //AUDIO
    private AudioSource edificeAudio;
    public AudioClip[] clips;

    #endregion // Variables

    // ########################################
    // Funciones MonoBehaviour 
    // ########################################

    #region MonoBehaviour

    public void Start()
    {
        edificeAudio = gameObject.GetComponent<AudioSource>();

        
        int disabledPerson = GameController.sharedInstance.totalDisabledPerson;
        int person = GameController.sharedInstance.totalPerson;
        int pet = GameController.sharedInstance.totalPet;
        int population = disabledPerson + person + pet;
        idPositionEdifice = -1;
        //TODO: ANTES DE GENERAR EL NUMERO RANDOM PREGUNTE SI AUN QUEDAN PERSONAS DISPONIBLES

        // PREGUNTAR POR CADA HABITANTE
        if (population > 0)
        {

            int maxPopulationInEdifice = 0;

            if(population >= 3)
            {
                maxPopulationInEdifice = 3;
            }else if(population == 2)
            {
                maxPopulationInEdifice = 2;
            }
            else
            {
                maxPopulationInEdifice = 1;
            }

            //TODO: ver si hay edificios sin personas
            contPopulation = Random.Range(1, maxPopulationInEdifice + 1);

            if(contPopulation > 0)
            {
                int i = contPopulation;
                do
                {
                    int numRandom = Random.Range(1, 4);
                    // TODO: confirmar si hay del habitante que salio si no intentar sacar otro
                    if (numRandom == 1 && disabledPerson > 0 )
                    {
                        contDisabledPerson++;
                        GameController.sharedInstance.totalDisabledPerson--;
                        i--;
                    }

                    if (numRandom == 2 && person > 0)
                    {
                        contPerson++;
                        GameController.sharedInstance.totalPerson--;
                        i--;
                    }

                    if (numRandom == 3 && pet > 0)
                    {
                        contPet++;
                        GameController.sharedInstance.totalPet--;
                        i--;
                    }

                } while (i > 0);                              
            }
        }      
    }

    public void Update()
    {
        
        if (level1.activeSelf)
        {
            edificeAudio.clip = clips[0];
        }
        if (level2.activeSelf)
        {
            edificeAudio.clip = clips[1];
        }
        if (level3.activeSelf)
        {
            edificeAudio.clip = clips[2];
        }

        if (!edificeAudio.isPlaying)
        {
            edificeAudio.Play();
        }

        if(contFire == 1)
        {
            StartFireLevel1();
        }else if(contFire == 2)
        {
            StartFireLevel2();
        }else if(contFire == 3)
        {
            StartFireLevel3();
        }
    }

    #endregion // Monobehaviour


    // ########################################
    // Funciones IsSelected y Audio
    // ########################################

    #region IsSelected/Audio

    public void IsSelected()
    {
        //UIManagerGame.sharedInstance.ShowPanelInfo();
        //UIManagerGame.sharedInstance.panelInfo.GetComponent<PanelInfo>().FillInformation(gameObject);
        FillHabitant();
        PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller.UpdateNumberOfActions();
        controller.HideAllButtonsInspect();
        UIManagerGame.sharedInstance.ShowPanelUI();
        isInspected = true;
    }

    public void IsSelectedTakeOut(int id)
    {
        PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        GameObject habitant = null;
        
        if (habitants[id].GetComponent<ButtonHabitant>().idHabitant == 0)
        {
            habitant = GameController.sharedInstance.disabledPerson;
            
        }else if (habitants[id].GetComponent<ButtonHabitant>().idHabitant == 1)
        {
            habitant = GameController.sharedInstance.person;
        }
        else if(habitants[id].GetComponent<ButtonHabitant>().idHabitant == 2)
        {
            habitant = GameController.sharedInstance.pet;
        }

        if (idPositionEdifice == 0)
        {
            Instantiate(habitant, controller.street.transform.GetChild(0).transform);
        }
    }

    public void PlayClickSound()
    {
        edificeAudio.PlayOneShot(clips[3]);
    }
    

    void FillHabitant()
    {
        int disabledPerson = contDisabledPerson;
        int person = contPerson;
        int pet = contPet;

        for (int i = 0; i < 3; i++)
        {
            if(disabledPerson > 0)
            {
                habitants[i].image.enabled = true;
                habitants[i].image.sprite = imageHabitants[0];
                habitants[i].GetComponent<ButtonHabitant>().idHabitant = 0;
                disabledPerson--;
            }else if(person > 0)
            {
                habitants[i].image.enabled = true;
                habitants[i].image.sprite = imageHabitants[1];
                habitants[i].GetComponent<ButtonHabitant>().idHabitant = 1;
                person--;
            }else if(pet > 0)
            {
                habitants[i].image.enabled = true;
                habitants[i].image.sprite = imageHabitants[2];
                habitants[i].GetComponent<ButtonHabitant>().idHabitant = 2;
                pet--;
            }

           // habitants[i].preserveAspect = true;
        }
    }


    #endregion // Audio


    // ########################################
    // Funciones Estados del fuego
    // ########################################

    #region FireState

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


    void SetFireState(FireState newFireState)
    { 
    //TODO: cambiar el color de los sprite cuando se comience a quemar
        if(newFireState == FireState.level1)
        {
            level1.SetActive(true);
        }
        if (newFireState == FireState.level2)
        {
           level2.SetActive(true);
        }
        if (newFireState == FireState.level3)
        {
            level3.SetActive(true);
            contFire = 3;
        }
    }
    #endregion //FireState

    public GameObject GetNeighborEdifice(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction,30f,LayerMask.GetMask("Edifice"));
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }


}
