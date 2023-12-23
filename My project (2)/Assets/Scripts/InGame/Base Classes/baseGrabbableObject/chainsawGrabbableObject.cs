using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class chainsawGrabbableObject : grabbableObject
{
    [SerializeField]
    float refreshTime;
    float refreshTimeLeft = 0;
    [SerializeField]
    int durabilityUntilDisplay;
    [SerializeField]
    AudioClip chainsawStart;
    [SerializeField]
    AudioClip chainsawEnd;
    AudioSource objectAudio;
    [SerializeField]
    float blowBack;
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
            objectAudio.clip = chainsawStart;
            objectAudio.Play();
        }
        return returnResult;
    }
    public override void slashEnd()
    {
        objectAudio.clip = chainsawEnd;
        objectAudio.Play();
        base.slashEnd();
    }
    public override void Update()
    {
        refreshTimeLeft -= Time.deltaTime;
        if(refreshTimeLeft <= 0)
        {
            refreshTimeLeft = refreshTime;
            getObjectsHit().Clear();
        }
        base.Update();
    }
    public override void durabilityDamage()
    {
        setDurability(getDurability() - 1);
        if (getSignifiesDamage() && getDurability() <= durabilityUntilDisplay)
        {
            getObjectAnimator().SetBool("IsDamaged", true);
        }
        if (getDurability() < 0)
        {
            durabilityGone();
        }
    }
    public override bool slashObject(GameObject slashedObject, Vector3 slashFromLocation)
    {
        bool returnResult = base.slashObject(slashedObject, slashFromLocation);
        if (returnResult)
        {
            getGrabbedByPlayerScript().push(getGrabbedByPlayerScript().getAngleFace() + 180, blowBack);
        }
        return returnResult;
    }
}
