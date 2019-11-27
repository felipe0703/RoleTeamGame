#region Namespace
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion //Namespace

namespace Com.BrumaGames.Llamaradas
{
    public class PanelInfo : MonoBehaviour
    {
        // ########################################
        // Variables
        // ########################################

        #region Variables

        public Image imageEdifice;
        public Image[] habitants;
        public Image[] fires;

        #endregion //Variables

        // ########################################
        // Fire Functions
        // ########################################

        #region FireFunction

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
                habitants[i].color = new Color(.85f, .85f, .85f, .3f);
            }

            for (int i = 0; i < habitants.Length; i++)
            {
                if (disabledPerson > 0)
                {
                    habitants[i].color = new Color(1f, 1f, 1f, 1f);
                    disabledPerson--;
                }
                else if (person > 0 && i > 2)
                {
                    habitants[i].color = new Color(1f, 1f, 1f, 1f);
                    person--;
                }
                else if (pet > 0 && i > 5)
                {
                    habitants[i].color = new Color(1f, 1f, 1f, 1f);
                    pet--;
                }
            }
        }


        void FillFire(GameObject edifice)
        {
            int contFire = edifice.GetComponent<Edifice>().contFire;

            for (int i = 0; i < fires.Length; i++)
            {
                fires[i].color = new Color(.85f, .85f, .85f, .3f);
            }

            for (int i = 0; i < fires.Length; i++)
            {
                if (contFire > 0)
                {
                    fires[i].color = new Color(1f, 1f, 1f, 1f);
                    contFire--;
                }
            }
        }
        #endregion //FireFunction
    }

}
