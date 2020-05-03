using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class ActivatingDectectionZone : MonoBehaviour
    {
        public GameObject detectionZonePerson;
        public GameObject detectionZoneDisabledPerson;
        
        public void ActivateZone()
        {
            Debug.Log("activando detector");
            detectionZonePerson.SetActive(true);
            detectionZoneDisabledPerson.SetActive(true);
        }
        public void DeactivateZone()
        {
            //detectionZonePerson.GetComponent<DetectionZone>().detetedHabitants = false;
            detectionZonePerson.SetActive(false);

            //detectionZoneDisabledPerson.GetComponent<DetectionZone>().detetedHabitants = false;
            detectionZoneDisabledPerson.SetActive(false);
        }
    }
}
