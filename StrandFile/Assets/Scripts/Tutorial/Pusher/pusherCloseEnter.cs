using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherCloseEnter : MonoBehaviour
{
    [SerializeField]
    tutorialPusherGather getGather;

    [SerializeField]
    bool lowers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMainScript>() != null)
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
