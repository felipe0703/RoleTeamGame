using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class ActivatingDectectionZone : MonoBehaviour
    {
        public GameObject detectionZone;
        
        public void ActivateZone()
        {
            Debug.Log("activando detector");
            detectionZone.SetActive(true);
        }
        public void DeactivateZone()
        {
            detectionZone.GetComponent<DetectionZone>().detetedHabitants = false;
            detectionZone.SetActive(false);
        }
    }
}
