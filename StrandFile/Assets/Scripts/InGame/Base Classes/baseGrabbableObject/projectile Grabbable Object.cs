using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileGrabbableObject : grabbableObject
{
    AudioSource objectAudio;
    [SerializeField]
    GameObject projectilePrefab;
    // Start is called before the first frame update
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
            GameObject currentProjectile = Instantiate(projectilePrefab, getGrabbedByPlayerScript().gameObject.transform.position, Quaternion.identity.normalized);
            baseProjectile projectileScript = currentProjectile.GetComponent<baseProjectile>();
            projectileScript.setAngle(getGrabbedByPlayerScript().getAngleFace());
            durabilityDamage();
        }
        return returnResult;
    }
}
