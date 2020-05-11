using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class HabitantController : MonoBehaviour
    {
        public bool detected;
        public GameObject btnSelect;


        private void Update()
        {
            if (detected)
                Debug.Log("me detectaron");
        }

        public void ActivateHabitantButtont()
        {
            btnSelect.SetActive(true);
        }

        public void DeactivateHabitantButtont()
        {
            btnSelect.SetActive(false);
            UIManagerGame.sharedInstance.HidePanelMinimap();
            UIManagerGame.sharedInstance.ChangeCamMinimapTransform(this.transform);
            
        }

    }
}
