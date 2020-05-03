using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class MovementDisabledPerson : MonoBehaviour
    {
        bool disabledPersonMove = false;
        bool goTarget1 = false;
        bool goTarget2 = false;
        public float dist = 0.6f;

        private void Update()
        {
            if (disabledPersonMove)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                PlayerController controller = player.GetComponent<PlayerController>();

                if (controller.disabledPersonArrivedAtDestination)
                {
                    //gameObject.transform.SetParent(null);
                    
                    controller.speed = 5f;

                    if (goTarget1)
                    {                     
                        gameObject.GetComponent<MovementIA>().SetTargetWhereToMove(controller.target.gameObject);
                        goTarget1 = false;
                        goTarget2 = true;
                    }
                    float distance = Vector2.Distance(controller.target.transform.position, gameObject.transform.position);
                    Debug.Log("distance: " + distance);

                    if(goTarget2 && distance < dist)
                    {
                        Debug.Log("4");
                        gameObject.GetComponent<MovementIA>().SetTargetWhereToMove(controller.target2.gameObject);
                        goTarget2 = false;
                        disabledPersonMove = false;
                        // controller.speed = 10f;
                    }
                }
            }
        }
        public void DetectPlayer()
        {
            DisabledPersonMove();
            goTarget1 = true;
            Debug.Log("2");
            //Movimiento del player
            UIManagerGame.sharedInstance.ShowPanelMove();            
            UIManagerGame.sharedInstance.CallDetectEdificeToMove();
            disabledPersonMove = true;
           
        }

        void DisabledPersonMove()
        {
            Debug.Log("1");
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            gameObject.GetComponent<MovementIA>().SetTargetWhereToMove(player);
            //gameObject.transform.SetParent(player.transform);
            player.GetComponent<PlayerController>().disabledPersonArrivedAtDestination = false;
        }
    }
}

