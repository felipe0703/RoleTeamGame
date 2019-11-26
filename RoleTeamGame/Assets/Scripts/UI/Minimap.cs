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
        bool isFolliwing;
        #endregion // Private Fields

        #region MonoBehaviours CallBacks
        private void Start()
        {
            if(player == null && !isFolliwing)
            {

                player = GameObject.Find("Player");
                pv = player.GetComponent<PhotonView>();
            }
            else
            {
                Debug.Log("no lo encontre");
            }
            
        }

        private void LateUpdate()
        {
            if (pv.IsMine)
            {
                Vector3 newPosition = player.transform.position;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
                isFolliwing = true;
            }            
        }
        #endregion  // MonoBehaviours CallBacks
    }
}

