using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskCreatorGrab : taskCreator
{
    [SerializeField]
    grabbableObject getToggleObject;

    // Update is called once per frame
    void Update()
    {
        if (getToggleObject.getHasBeenGrabbed())
        {
            attemptSignalTaskComplete();
        }
    }
}
