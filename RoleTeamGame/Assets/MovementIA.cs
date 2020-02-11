using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


namespace Com.BrumaGames.Llamaradas
{
    public class MovementIA : MonoBehaviour
    {

        public Transform target;

        public float speed = 200f;
        public float nextWaypointDistance = 3f;

        Path path;// ruta actual
        int currentWaypoint = 0;
        bool reachedEndPath = false;

        //animacion
        private Animator animator;
        //private Vector2 animDir = Vector2.zero;
        //private Vector2 animDir2 = Vector2.zero;

        [SerializeField]
        Seeker seeker;
        [SerializeField]
        Rigidbody2D rb;


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

            if (path == null)
            {
                //Debug.Log("no encuentro la ruta");
                return;
            }

            //si los puntos necesarios para llegar son mayor a los que faltan, es porque llegué
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndPath = true;
                StopAnim();
                return;
            }
            else
            {
                //Debug.Log("No he Llegado");
                reachedEndPath = false;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            animator.SetBool("isMoving", true);
            animator.SetFloat("MoveX", force.x);
            animator.SetFloat("MoveY", force.y);
            //Debug.Log("x = "+force.x+", y = "+force.y);
            

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance)
                currentWaypoint++;
        }

        // se llama cuando se completa la ruta
        void OnPathComplete(Path p)
        {
            //Debug.Log("complete la ruta");
            if (!p.error)
            {
                //Debug.Log("dentro del if");
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

        public void SetTargetWhereToMove(GameObject target)
        {
            this.target = target.transform;
            PlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            controller.UpdateNumberOfActions();
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