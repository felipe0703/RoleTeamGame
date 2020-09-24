using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BrumaGames.Llamaradas
{
    public class HabitantController : MonoBehaviour
    {
        public bool detected;
        public GameObject btnSelect;
        public GameObject survivedEffect;
        public bool survived;
        public int idHabitant;
        int scoreHabitant = 0;
        SfxControl ScriptEfectosSonido;

        private void Start()
        {
            ScriptEfectosSonido = GameObject.Find("Sound/Efectos interaccion").GetComponent<SfxControl>();
        }

        private void Update()
        {
            if (detected) Debug.Log("me detectaron");
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Street")) survived = collision.GetComponent<Street>().isBorder;

            if (survived)
            {
                if (idHabitant == 0) { scoreHabitant = 3; ScriptEfectosSonido.PlayPoint3Sound(); }
                else if (idHabitant == 1) { scoreHabitant = 2; ScriptEfectosSonido.PlayPoint2Sound(); }

                PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                controller.UpdateScoreSaved(scoreHabitant);
                gameObject.GetComponent<MovementIA>().speed = 50f;
                StartCoroutine(DestroyHabitant());
            }
        }

        IEnumerator DestroyHabitant()
        {
            Instantiate(survivedEffect, transform.position, transform.rotation);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
    }
}
