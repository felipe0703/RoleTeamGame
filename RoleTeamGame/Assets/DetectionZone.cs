using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class DetectionZone : MonoBehaviour
    {
        public List<GameObject> habitansDetected = new List<GameObject>();
        public bool isPerson = false;

        private void OnEnable()
        {
            for (int i = 0; i < habitansDetected.Count; i++)
            {
                habitansDetected.RemoveAt(i);
            }
        }

        private void OnDisable()
        {
            Debug.Log("me desactivaron");
            foreach (var habitant in habitansDetected)
            {
                habitant.GetComponent<HabitantController>().DeactivateHabitantButtont();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DisabledPerson") && !isPerson)
            {
                habitansDetected.Add(collision.gameObject);
                collision.gameObject.GetComponent<HabitantController>().ActivateHabitantButtont();
            }

            if (collision.CompareTag("Person") && isPerson)
            {
                habitansDetected.Add(collision.gameObject);
                collision.gameObject.GetComponent<HabitantController>().ActivateHabitantButtont();
            }
        }


    }
}
