using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPusherTile : pusherTile
{
    [SerializeField]
    bool lowered;
    public void lowerPusher()
    {
        if (!lowered)
        {
            lowered = true;
            switchState(5);
        }
    }
    public void raisePusher()
    {
        if (lowered)
        {
            lowered = false;
            switchState(2);
        }
    }
    public override void Awake()
    {
        base.Awake();
        if (lowered)
        {
            desolidify();
            switchState(0);
        }
        else
        {
            solidify();
            switchState(3);
        }
    }
    public override void Update()
    {
        if(getCurrentState() != 0 && getCurrentState() != 3)
        {
            base.Update();
        }
        switch (getCurrentState())
        {
            case 1:
                switchState(2);
                break;
            case 4:
                switchState(5);
                break;
        }
    }
}
