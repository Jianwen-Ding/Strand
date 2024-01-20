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
    bool lowers;

    // Update is called once per frame
    void Update()
    {
        if (givenObject == null)
        {
            if (lowers)
            {
                getGather.allLower();
            }
            else
            {
                getGather.allRise();
            }
        }
    }
}
