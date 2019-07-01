using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSceneLine : MonoBehaviour
{

    public Transform from;
    public Transform[] toRight;
    public Transform[] toUp;
    public Transform[] toLeft;
    public Transform[] toDown;
    public Transform target;
    public Transform target2;

    private void OnDrawGizmos()
    {
        if (from != null && toRight != null && toUp != null && toLeft != null && toDown != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(from.position, 0.2f);
            //  RIGHT
            for (int i = 0; i < toRight.Length; i++)
            {
                Gizmos.DrawLine(from.position, toRight[i].position);                
                Gizmos.DrawSphere(toRight[i].position, 0.2f);
            }


            // UP
            for (int i = 0; i < toUp.Length; i++)
            {
                Gizmos.DrawLine(from.position, toUp[i].position);
                Gizmos.DrawSphere(toUp[i].position, 0.2f);
            }


            // LEFT
            for (int i = 0; i < toLeft.Length; i++)
            {
                Gizmos.DrawLine(from.position, toLeft[i].position);
                Gizmos.DrawSphere(toLeft[i].position, 0.2f);
            }


            // DOWN
            for (int i = 0; i < toDown.Length; i++)
            {
                Gizmos.DrawLine(from.position, toDown[i].position);
                Gizmos.DrawSphere(toDown[i].position, 0.2f);
            }
            

            // TARGET
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(target.position, 0.3f);

            // TARGET
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(target2.position, 0.3f);
        }    
    }
    
}
