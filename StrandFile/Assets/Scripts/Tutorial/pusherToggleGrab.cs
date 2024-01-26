using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherToggleGrab : MonoBehaviour
{
    [SerializeField]
    tutorialPusherGather getGather;

    [SerializeField]
    grabbableObject getToggleObject;

    [SerializeField]
    baseEnemy deadOnCompletion;
    [SerializeField]
    bool lowers;

    // Update is called once per frame
    void Update()
    {
        if (getToggleObject.getHasBeenGrabbed())
        {
            if (lowers)
            {
                if(deadOnCompletion != null)
                {
                    deadOnCompletion.isDamaged(100000);
                }
                getGather.allLower();
            }
            else
            {
                getGather.allRise();
            }
        }
    }
}
