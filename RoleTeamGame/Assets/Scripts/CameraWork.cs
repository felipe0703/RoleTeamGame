using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

namespace Com.BrumaGames.Llamaradas
{
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields

        // cached transform of the target
        //Transform cameraTransform;
        public GameObject cam;
        // maintain a flag internally to reconnect if target is lost or camera is switched
        bool isFollowing;

        private PhotonView pv;

        #endregion

        private void Start()
        {
            pv = GetComponent<PhotonView>();

            if (cam == null && !isFollowing)
            {
                OnStartFollowing();
            }
        }

        public void OnStartFollowing()
        {
            if (pv.IsMine)
            {
                cam = GameObject.Find("CM vcam1");
                CinemachineVirtualCamera vCam = cam.GetComponent<CinemachineVirtualCamera>();
                vCam.Follow = this.gameObject.transform;
                isFollowing = true;
            }
        }
    }
}

