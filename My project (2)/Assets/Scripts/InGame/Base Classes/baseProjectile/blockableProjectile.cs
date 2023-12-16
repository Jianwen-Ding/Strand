using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blockableProjectile : baseProjectile
{
    [SerializeField]
    float pushObjects;
    // What happens when projectile hits an object whether through isTrigger or onCollision
    public override void collide(GameObject collideObject)
    {
        base.collide(collideObject);
        if(collideObject.layer == 0)
        {
            Rigidbody2D collidePhysics = collideObject.GetComponent<Rigidbody2D>();
            if (collidePhysics != null)
            {
                float xPush = Mathf.Cos(getAngle() * Mathf.Deg2Rad) * pushObjects;
                float yPush = Mathf.Sin(getAngle() * Mathf.Deg2Rad) * pushObjects;
                collidePhysics.velocity *= 0;
                collidePhysics.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            }
            Destroy(gameObject);
        }
    }
}
