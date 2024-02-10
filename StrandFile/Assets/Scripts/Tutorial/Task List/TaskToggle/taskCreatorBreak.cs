using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskCreatorBreak : taskCreator
{
    [SerializeField]
    GameObject givenObject;


    // Update is called once per frame
    void Update()
    {
        if (givenObject == null)
        {
            attemptSignalTaskComplete();
        }
    }
}
