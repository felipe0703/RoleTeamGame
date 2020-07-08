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
                    controller.speed = 5f;

                    if (goTarget1)
                    {                     
                        gameObject.GetComponent<MovementIA>().CallSetTargetWhereToMove(controller.target.gameObject);
                        goTarget1 = false;
                        goTarget2 = true;
                    }
                    float distance = Vector2.Distance(controller.target.transform.position, gameObject.transform.position);

                    if(goTarget2 && distance < dist)
                    {
                        gameObject.GetComponent<MovementIA>().CallSetTargetWhereToMove(controller.target2.gameObject);
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
            //Movimiento del player
            UIManagerGame.sharedInstance.ShowPanelMove();            
            UIManagerGame.sharedInstance.CallDetectEdificeToMove();
            disabledPersonMove = true;
           
        }

        void DisabledPersonMove()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            gameObject.GetComponent<MovementIA>().CallSetTargetWhereToMove(player);
            //gameObject.transform.SetParent(player.transform);
            player.GetComponent<PlayerController>().disabledPersonArrivedAtDestination = false;
        }
    }
}

