using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherToggleBreak : MonoBehaviour
{
    [SerializeField]
    GameObject givenObject;

    [SerializeField]
    tutorialPusherGather getGather;

    [SerializeField]
    baseEnemy deadOnCompletion;
    [SerializeField]
    bool lowers;

    // Update is called once per frame
    void Update()
    {
        if (givenObject == null)
        {
            if (lowers)
            {
                if (deadOnCompletion != null)
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
