using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartButton : EnterMainMenuScript
{
    public override void activate()
    {
        base.activate();
        PlayerPrefs.DeleteAll();
    }
}
