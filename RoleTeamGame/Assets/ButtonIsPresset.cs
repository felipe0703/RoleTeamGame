using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class ButtonIsPresset : MonoBehaviour
    {

        public string action = "";

        private void Update()
        {
            if (action == "add")
            {
                if (GameController.sharedInstance.buttonAddFire && !GameController.sharedInstance.buttonSubtractFire)
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);                
                else
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
            else if(action == "subtract")
            {
                if (GameController.sharedInstance.buttonSubtractFire && !GameController.sharedInstance.buttonAddFire)
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1.2f, 1.2f, 1.2f);
                else
                    gameObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
    }
}

