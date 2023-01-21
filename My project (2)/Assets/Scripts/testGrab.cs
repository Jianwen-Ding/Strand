using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGrab : grabbableObject
{
    public override void grabbedEffect(GameObject grabbedBy)
    {
        base.grabbedEffect(grabbedBy);
        print("test");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
