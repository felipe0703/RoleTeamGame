using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDetectBurnedPlayer : MonoBehaviour
{
    public GameObject colliderNorth;
    public GameObject colliderEast;
    public GameObject colliderSouth;
    public GameObject colliderWest;

    public void ActivateBurnedPlayerDetector(int direction)
    {
        Debug.Log("me active");
        switch (direction)
        {
            case 0:
                colliderNorth.SetActive(true);
                break;
            case 1:
                colliderEast.SetActive(true);
                break;
            case 2:
                colliderSouth.SetActive(true);
                break;
            case 3:
                colliderWest.SetActive(true);
                break;
            default:
                break;
        }
    }
}
