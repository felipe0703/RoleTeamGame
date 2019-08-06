using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystemManager : MonoBehaviour
{
    public static TurnSystemManager sharedInstance;
    public int turn = 0;
    public int turnLimit = 3;

    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(turn > 3)
        {
            turn = 1;
        }
        Debug.Log(turn);
    }
}
