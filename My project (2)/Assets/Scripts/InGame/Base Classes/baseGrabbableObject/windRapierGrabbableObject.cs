using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windRapierGrabbableObject : grabbableObject
{
    [SerializeField]
    float windBurst;
    [SerializeField]
    float playerLock;
    [SerializeField]
    float blowBack;
    [SerializeField]
    float blowbackLock;
    AudioSource objectAudio;
    public override void Start()
    {
        base.Start();
        objectAudio = gameObject.GetComponent<AudioSource>();
    }
    public override bool startSlashEffect()
    {
        bool returnResult = base.startSlashEffect();
        if (returnResult)
        {
            objectAudio.Play();
            getGrabbedByPlayerScript().lockMovement(playerLock);
            getGrabbedByPlayerScript().push(getGrabbedByPlayerScript().getAngleFace(), windBurst);
        }
        return returnResult;
    }

    public override bool slashObject(GameObject slashedObject, Vector3 slashFromLocation)
    {
        bool returnResult = base.slashObject(slashedObject, slashFromLocation);
        if (returnResult)
        {
            getGrabbedByPlayerScript().lockMovement(blowbackLock);
            getGrabbedByPlayerScript().push(getGrabbedByPlayerScript().getAngleFace() + 180, blowBack);
        }
        return returnResult;
    }
}
