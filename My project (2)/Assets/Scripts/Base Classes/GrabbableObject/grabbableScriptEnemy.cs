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
    public override void grabbedEffect(GameObject grabbedBy)
    {
        playerHand grabbedByHandScript = grabbedBy.gameObject.GetComponent<playerHand>();
        if (enemyScript.getGrabArmor() <= 0)
        {
            base.grabbedEffect(grabbedBy);
            enemyScript.setGrabArmor(enemyScript.getDefaultGrabArmor());
        }
        else
        {
            enemyScript.loseGrabArmor(1);
            grabbedByHandScript.releaseObject();
            //-pushes both characters back
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(gameObject.transform.position.y - grabbedByHandScript.getObjectPlayerScript().transform.position.y, gameObject.transform.position.x - grabbedByHandScript.getObjectPlayerScript().transform.position.x );
            //pushes enemy back
            getObjectPhysics().velocity *= 0;
            push(Angle, enemyScript.getFailedGrabPushBackEnemy());
            //pushes player back
            grabbedByHandScript.getPlayerObjectPhysics().velocity *= 0;
            grabbedByHandScript.getObjectPlayerScript().push(Angle + 180, enemyScript.getFailedGrabPushBackPlayer());

        }
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
