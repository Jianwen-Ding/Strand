using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPusherGather : MonoBehaviour
{
    [SerializeField]
    ArrayList pusherInGather = new ArrayList();
    [SerializeField]
    bool defaultLower;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialPusherTile attemptGather = collision.gameObject.GetComponent<tutorialPusherTile>();
        if (attemptGather != null && !pusherInGather.Contains(attemptGather))
        {
            pusherInGather.Add(attemptGather);
            if (defaultLower)
            {
                attemptGather.lowerPusher();
            }
            else
            {
                attemptGather.raisePusher();
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        tutorialPusherTile attemptGather = collision.gameObject.GetComponent<tutorialPusherTile>();
        if (attemptGather != null && !pusherInGather.Contains(attemptGather))
        {
            pusherInGather.Add(attemptGather);
            if (defaultLower)
            {
                attemptGather.lowerPusher();
            }
            else
            {
                attemptGather.raisePusher();
            }
        }
    }

    public void allRise()
    {
        foreach(tutorialPusherTile pusher in pusherInGather)
        {
            pusher.raisePusher();
        }
    }
    public void allLower()
    {
        foreach (tutorialPusherTile pusher in pusherInGather)
        {
            pusher.lowerPusher();
        }
    }
}
