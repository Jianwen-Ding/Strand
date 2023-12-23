using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class storageEnemy : baseEnemy
{
    //STORAGE ENEMY
    //Does no damage,
    //Only meant to drop a useful object
    //Animation State
    //0 - Default
    //1 - Open


    //Deals with contact
    public override void onContact(GameObject collisionObject)
    {
        grabbableObject colliderGrabbableScript = collisionObject.GetComponent<grabbableObject>();

        //if enemy collides with thrown/fast flying grabbable object
        if (colliderGrabbableScript != null && colliderGrabbableScript.getThrownDamageTimeLeft() >= 0)
        {
            //print(colliderGrabbableScript.getVelocityThreshold());
            //Checking the magnitude of the velocity
            float colliderVelocitySum = Mathf.Sqrt(colliderGrabbableScript.getObjectPhysics().velocity.y * colliderGrabbableScript.getObjectPhysics().velocity.y + colliderGrabbableScript.getObjectPhysics().velocity.x * colliderGrabbableScript.getObjectPhysics().velocity.x);
            if (colliderVelocitySum >= colliderGrabbableScript.getThrowVelocityThreshold())
            {
                colliderGrabbableScript.throwHitEffect();
                isDamaged(colliderGrabbableScript.getThrowDamage());
                float colliderAngle = Mathf.Atan2(colliderGrabbableScript.getObjectPhysics().velocity.y, colliderGrabbableScript.getObjectPhysics().velocity.x);
                float xPush = Mathf.Cos(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                float yPush = Mathf.Sin(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                getObjectRigidbody().velocity *= 0;
                getObjectRigidbody().AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
                Rigidbody2D collisionRigid = collisionObject.gameObject.GetComponent<Rigidbody2D>();
                collisionRigid.velocity *= 0;
                collisionRigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
            }
        }

    }

    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                getObjectAnimator().SetInteger("EnemyState", 1);
                break;
            case "stunned":
                if (getObjectAnimator().GetInteger("EnemyState") != 3)
                    getObjectAnimator().SetInteger("EnemyState", 2);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 3);
                break;
        }
    }

}
