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
            GameObject target = GetBorder(adjacentDirections[3], mask);
            this.gameObject.GetComponent<MovementIA>().SetTargetWhereToMove(target);
        }

        private GameObject GetBorder(Vector2 direction, LayerMask mask)
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 1000f, mask);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                return hit.collider.gameObject;
            }
            else
            {
                Debug.Log("no encontre nada");
                return null;
            }
        }
    }
}