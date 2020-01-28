using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.BrumaGames.Llamaradas
{
    public class Minimap : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("El player al que el minimapa hara el seguimiento")]
        public GameObject player;

        #endregion // Public Fields

        #region Private Fields

        PhotonView pv;
        [SerializeField] bool isFolliwing;

        #endregion // Private Fields

        #region MonoBehaviours CallBacks
        
        private void LateUpdate()
        {
            if (player == null)
                return;                

            if (pv.IsMine)
            {
                Vector3 newPosition = player.transform.position;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
                isFolliwing = true;
            }            
        }

        public void FindPlayer()
        {
            if (player == null && !isFolliwing)
            {                
                player = GameObject.FindGameObjectWithTag("Player"); 
                if(player != null)
                    pv = player.GetComponent<PhotonView>();
            }
        }
        #endregion  // MonoBehaviours CallBacks
    }
}

