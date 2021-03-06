﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;
using System;

namespace Com.BrumaGames.Llamaradas
{
    public class MovementIA : MonoBehaviour
    {

        public Transform target;

        public bool personMove = false;
        public float speed = 200f;
        public float nextWaypointDistance = 3f;

        Path path;// ruta actual
        int currentWaypoint = 0;
        public bool reachedEndPath = false;

        //animacion
        private Animator animator;
        //private Vector2 animDir = Vector2.zero;
        //private Vector2 animDir2 = Vector2.zero;

        [SerializeField]
        Seeker seeker;
        [SerializeField]
        Rigidbody2D rb;
        PhotonView pv;


        // Start is called before the first frame update
        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            
            //InvokeRepeating("UpdatePath", 0f,0.5f);
            //UpdatePath();
        }


        void FixedUpdate()
        {

            if (path == null) return;

            //si los puntos necesarios para llegar son mayor a los que faltan, es porque llegué
            if (currentWaypoint >= path.vectorPath.Count)
            {
                //Debug.Log("lleuge");
               // Debug.Log("distancia: " + currentWaypoint + "pathcoun: " + path.vectorPath.Count);
                reachedEndPath = true;
                StopAnim();
                return;
            }
            else
            {
                //Debug.Log("no lleuge distancia: " + currentWaypoint + "pathcoun: " + path.vectorPath.Count);
                //Debug.Log("No he Llegado");
                //reachedEndPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            animator.SetBool("isMoving", true);
            animator.SetFloat("MoveX", force.x);
            animator.SetFloat("MoveY", force.y);           

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) currentWaypoint++;
        }

        // se llama cuando se completa la ruta
        void OnPathComplete(Path p)
        {
           // Debug.Log("complete la ruta");
            if (!p.error)
            {
               // Debug.Log("dentro del if");
                path = p;
                currentWaypoint = 0;
            }
        }

        // calcular la ruta
        void UpdatePath()
        {
            if (seeker == null)
            {
                seeker = GetComponent<Seeker>();
                rb = GetComponent<Rigidbody2D>();
            }
            //se ha calculado la ruta?
            if (seeker.IsDone())
            {
                //Debug.Log("calculando la ruta");
                //comenzar a calcular la ruta(inicio, fin, callback
                seeker.StartPath(rb.position, target.position, OnPathComplete);
            }

        }


        public void CallSetTargetWhereToMove(GameObject target)
        {
            Vector3 position = target.transform.position;
            byte[] data = new byte[sizeof(float) * 3];
            Buffer.BlockCopy(BitConverter.GetBytes(position.x), 0, data, 0 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(position.y), 0, data, 1 * sizeof(float), sizeof(float));
            Buffer.BlockCopy(BitConverter.GetBytes(position.z), 0, data, 2 * sizeof(float), sizeof(float));

            pv = GetComponent<PhotonView>();
            pv.RPC("SetTargetWhereToMovePun", RpcTarget.AllViaServer,data);
            if (personMove)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                PlayerController controller = player.GetComponent<PlayerController>();
                controller.UpdateActions();
            } 
        }

        [PunRPC]
        void SetTargetWhereToMovePun(byte[] data)
        {
            byte[] position = data;
            Vector3 vect = Vector3.zero;
            vect.x = BitConverter.ToSingle(position, 0 * sizeof(float));
            vect.y = BitConverter.ToSingle(position, 1 * sizeof(float));
            vect.z = BitConverter.ToSingle(position, 2 * sizeof(float));

            GameObject emptyGO = new GameObject();
            emptyGO.transform.position = vect;
            this.target = emptyGO.transform;
            reachedEndPath = false;
            UpdatePath();
        }

        public void SetTargetWhereToMove(GameObject target)
        {
            this.target = target.transform;
            reachedEndPath = false;
            UpdatePath();
        }

        private void StopAnim()
        {
            animator.SetBool("isMoving", false);
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", 0);
        }

    }
}