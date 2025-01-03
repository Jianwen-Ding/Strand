using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableScriptEnemy : grabbableObject
{
    //--VARIENT OF GRABBABLE OBJECT THAT NEEDS TO BE ATTACHED WITH BASE ENEMY
    #region variables
    private baseEnemy enemyScript;
    [SerializeField]
    private float timeUntilRelease;
    [SerializeField]
    private float timeUntilReleaseLeft;
    #endregion
    public override bool slashObject(GameObject slashedObject, Vector3 slashFromLocation)
    {
        bool hasSlashed = base.slashObject(slashedObject, slashFromLocation);
        if (hasSlashed)
        {
            enemyScript.isDamaged(1);
        }
        return hasSlashed;
    }
    public override void throwEffect(float strength, float angle)
    {
        base.throwEffect(strength, angle);
        enemyScript.stunEnemy(getThrownStateTime());
        timeUntilReleaseLeft = timeUntilRelease;
    }
    public override void thrownEnd()
    {
        base.thrownEnd();
        if (enemyScript.getCacheBars() != null)
        {
            enemyScript.getCacheBars().showBars();
        }
    }
    public override void releasedEffect()
    {
        base.releasedEffect();
        if (enemyScript.getCacheBars() != null)
        {
            enemyScript.getCacheBars().showBars();
        }
        enemyScript.stunEnemy(getReleaseStateTime());
        timeUntilReleaseLeft = timeUntilRelease;
    }
    public override bool grabbedEffect(GameObject grabbedBy)
    {
        playerHand grabbedByHandScript = grabbedBy.gameObject.GetComponent<playerHand>();
        //succesful grab
        if (enemyScript.getGrabArmor() <= 0)
        {
            if (enemyScript.getCacheBars() != null)
            {
                enemyScript.getCacheBars().hideBars();
            }
            base.grabbedEffect(grabbedBy);
            enemyScript.stunEnemy(timeUntilRelease);
            enemyScript.setGrabArmor(enemyScript.getDefaultGrabArmor());
            enemyScript.getCacheBars().updateGrabArmor(enemyScript.getGrabArmor());
            return true;
        }
        //failed grab
        else
        {
            enemyScript.loseGrabArmor(1);
            enemyScript.getCacheBars().updateGrabArmor(enemyScript.getGrabArmor());
            enemyScript.stunEnemy(enemyScript.getStunOnPush());
            //-pushes both characters back
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(gameObject.transform.position.y - grabbedByHandScript.getObjectPlayerScript().transform.position.y, gameObject.transform.position.x - grabbedByHandScript.getObjectPlayerScript().transform.position.x );
            //pushes enemy back
            getObjectPhysics().velocity *= 0;
            push(Angle, enemyScript.getFailedGrabPushBackEnemy());
            //pushes player back
            grabbedByHandScript.getPlayerObjectPhysics().velocity *= 0;
            grabbedByHandScript.getObjectPlayerScript().push(Angle + 180, enemyScript.getFailedGrabPushBackPlayer());
            return false;
        }
    }
    public override void whileGrabbedEffect()
    {
        base.whileGrabbedEffect();
        if (timeUntilReleaseLeft >= 0)
        {
            timeUntilReleaseLeft -= Time.deltaTime;
            if (timeUntilReleaseLeft < 0)
            {
                getGrabbedByObjectScript().stopAttemptSlash();
                getGrabbedByObjectScript().releaseObject();
                timeUntilReleaseLeft = timeUntilRelease;
            }
        }
    }

    public override bool getSignifiesSlash()
    {
        return base.getSignifiesSlash();
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        setDurability(99);
        timeUntilReleaseLeft = timeUntilRelease;
        enemyScript = gameObject.GetComponent<baseEnemy>();
        if(enemyScript == null)
        {
            print("The grabbableScripEnemy script attached to " + gameObject.name + "requires the script baseChildren, or any children scripts");
        }
    }
    // Update is called every fram
    public override void Update()
    {
        base.Update();
    }
}
