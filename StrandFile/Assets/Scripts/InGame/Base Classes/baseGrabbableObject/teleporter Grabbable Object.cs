using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;

public class teleporterGrabbableObject : grabbableObject
{
    AudioSource objectAudio;
    [SerializeField]
    GameObject projectilePrefab;
    GameObject currentProjectile;
    teleporterProjectile projectileScript;
    [SerializeField]
    GameObject residuePrefab;
    [SerializeField]
    bool hasSentProjectile = false;
    [SerializeField]
    float timeUntilForceTeleport;
    [SerializeField]
    float timeUntilForceTeleportLeft;

    public override bool startSlashEffect()
    {
        bool returnResult = base.startSlashEffect();
        if (returnResult)
        {
            if (!hasSentProjectile)
            {
                timeUntilForceTeleportLeft = timeUntilForceTeleport;
                hasSentProjectile = true;
                currentProjectile = Instantiate(projectilePrefab, getGrabbedByPlayerScript().gameObject.transform.position, Quaternion.identity.normalized);
                projectileScript = currentProjectile.GetComponent<teleporterProjectile>();
                projectileScript.setAngle(getGrabbedByPlayerScript().getAngleFace());
            }
            else
            {
                attemptTeleport();
            }
        }
        return returnResult;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        objectAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (hasSentProjectile)
        {
            timeUntilForceTeleportLeft -= Time.deltaTime;
            if (timeUntilForceTeleportLeft <= 0)
            {
                if (!attemptTeleport())
                {
                    hasSentProjectile = false;
                    timeUntilForceTeleportLeft = timeUntilForceTeleport;
                    Destroy(currentProjectile);
                }
            }
        }
        base.Update();
    }

    // Attempts to teleport
    public virtual bool attemptTeleport()
    {
        if (hasSentProjectile && !projectileScript.getIfIsColliding())
        {
            objectAudio.Play();
            durabilityDamage();
            hasSentProjectile = false;
            Instantiate(residuePrefab, getGrabbedByPlayerScript().gameObject.transform.position, Quaternion.identity.normalized);
            Instantiate(residuePrefab, currentProjectile.transform.position, Quaternion.identity.normalized);
            getGrabbedByPlayerScript().gameObject.transform.position = currentProjectile.transform.position;
            Destroy(currentProjectile);
            return true;
        }
        return false;
    }
}
