using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.BrumaGames.Llamaradas
{
    public class MovementPet : MonoBehaviour
    {
        public LayerMask mask;

        //Detectar los edificios adyacentes
        private Vector2[] adjacentDirections = new Vector2[]
        {
                Vector2.up,
                Vector2.right,
                Vector2.down,
                Vector2.left
        };

        private void OnEnable()
        {
            GameObject target = GetBorder(mask);
            this.gameObject.GetComponent<MovementIA>().SetTargetWhereToMove(target);
        }

        private GameObject GetBorder(LayerMask mask)
        {

            float distance = 1000f;
            Vector2 direction = Vector2.zero;

            //distancia mas corta al borde
            for (int i = 0; i < adjacentDirections.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, adjacentDirections[i], 1000f, mask);

                if (hit.collider != null)
                {
                    if (hit.distance < distance)
                    {
                        distance = hit.distance;
                        direction = adjacentDirections[i];
                    }
                }
            }

            RaycastHit2D hit2 = Physics2D.Raycast(this.transform.position, direction, 1000f, mask);

            if (hit2.collider != null)
            {
                //Debug.Log(hit.collider.name);
                return hit2.collider.gameObject;
            }
            else
            {
                //Debug.Log("no encontre nada");
                return null;
            }
        }
    }
}