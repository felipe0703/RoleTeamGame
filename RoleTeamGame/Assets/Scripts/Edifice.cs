using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edifice : MonoBehaviour
{
    public int id;
    public GameObject btn;
    public GameObject level1;

    public int contPopulation;
    public int contDisabledPerson;
    public int contPerson;
    public int contPet;

    public int maxFire;

    //AUDIO
    private AudioSource edificeAudio;
    public AudioClip[] clips;

    public void Start()
    {
        edificeAudio = gameObject.GetComponent<AudioSource>();

        
        int disabledPerson = GameController.sharedInstance.totalDisabledPerson;
        int person = GameController.sharedInstance.totalPerson;
        int pet = GameController.sharedInstance.totalPet;
        int population = disabledPerson + person + pet;

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
        /*  Como no hay más nivel de fuego que el 1 no me puedo referir a los demas

        if (level2.activeSelf)
        {
            edificeAudio.clip = clips[1];
        }
        if (level3.activeSelf)
        {
            edificeAudio.clip = clips[2];
        }
        if (level4.activeSelf)
        {
            edificeAudio.clip = clips[2];

        }
        */

        if (!edificeAudio.isPlaying)
        {
            edificeAudio.Play();
        }
    }

    public void IsSelected()
    {
        UIManagerGame.sharedInstance.ShowPanelInfo();
        UIManagerGame.sharedInstance.panelInfo.GetComponent<PanelInfo>().FillInformation(gameObject);
        btn.SetActive(false);
    }

    public void FireStart()
    {
        level1.SetActive(true);
    }

}
 