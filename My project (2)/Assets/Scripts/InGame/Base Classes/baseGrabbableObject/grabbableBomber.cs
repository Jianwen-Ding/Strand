using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableBomber : grabbableScriptEnemy
{
    public override void throwEffect(float strength, float angle)
    {
        gameObject.GetComponent<bomberScript>().isDamaged(1000);
        base.throwEffect(strength, angle);
    }
}
