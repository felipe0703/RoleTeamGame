using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSceneLine : MonoBehaviour
{

    public Transform from;
    public Transform[] target;
    public Color color;

    private void OnDrawGizmos()
    {
        if (from != null )
        {
            //  FROM
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(from.position, 0.2f);

            // TARGETS
            Gizmos.color = new Color(color.r, color.g, color.b);
            for (int i = 0; i < target.Length; i++)
            {
                Gizmos.DrawLine(from.position, target[i].position);
                Gizmos.DrawSphere(target[i].position, 0.3f);
            }
            

        }    
    }
    
}
