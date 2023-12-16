using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableFlyingHead : grabbableScriptEnemy
{
    [SerializeField]
    float pullForce;
    public override void whileGrabbedEffect()
    {
        base.whileGrabbedEffect();
        float angle = getGrabbedByPlayerScript().getAngleFace();
        getGrabbedByPlayerScript().push(angle, pullForce * Time.deltaTime);
    }
    public override void whileThrownEffect()
    {
        base.whileThrownEffect();
        getCollider().isTrigger = false;
    }
    public override void thrownEnd()
    {
        base.thrownEnd();
        getCollider().isTrigger = true;
    }
}
