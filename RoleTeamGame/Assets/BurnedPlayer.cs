using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnedPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
        }
        else
        {
            Debug.Log("no hay player enter");
        }

        if (collision.gameObject.name == "Player(clone)")
        {
            Debug.Log("hay player enter name clone");
        }

        if (collision.gameObject.name == "Player")
        {
            Debug.Log("hay player enter name ");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("hay player stay");
        }
        else
        {
            Debug.Log("no hay player stay");
        }
    }
}
