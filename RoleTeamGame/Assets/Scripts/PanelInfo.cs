using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelInfo : MonoBehaviour
{
    public Image imageEdifice;
    public Image[] habitants;
    public GameObject prefabDisabledPerson;
    public GameObject prefabPerson;
    public GameObject prefabPet;


    public void FillInformation(GameObject edifice)
    {
        imageEdifice.sprite = edifice.GetComponent<SpriteRenderer>().sprite;
        FillHabitant(edifice);
        FillFire(edifice);
    }


    void FillHabitant(GameObject edifice)
    {
        int disabledPerson = edifice.GetComponent<Edifice>().contDisabledPerson;
        int person = edifice.GetComponent<Edifice>().contPerson;
        int pet = edifice.GetComponent<Edifice>().contPet;

        for (int i = 0; i < habitants.Length; i++)
        {
            habitants[i].GetComponent<Image>().enabled = false;
        }

        for (int i = 0; i < habitants.Length; i++)
        {
            if (disabledPerson > 0)
            {
                //TODO: ver si me conviene tener los prefabs en el gamecontroller
                habitants[i].GetComponent<Image>().enabled = true;
                habitants[i].sprite = prefabDisabledPerson.GetComponent<Image>().sprite;
                disabledPerson--;
            }
            else if (person > 0 && i > 2)
            {
                habitants[i].sprite = prefabPerson.GetComponent<Image>().sprite;
                habitants[i].GetComponent<Image>().enabled = true;
                person--;
            }
            else if (pet > 0 && i > 5)
            {
                habitants[i].sprite = prefabPet.GetComponent<Image>().sprite;
                habitants[i].GetComponent<Image>().enabled = true;
                pet--;
            }
        }
    }


    void FillFire(GameObject edifice)
    {

    }
}
