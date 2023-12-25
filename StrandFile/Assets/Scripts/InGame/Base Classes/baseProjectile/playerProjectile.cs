using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

public class playerProjectile : baseProjectile
{
    // What happens when projectile hits an object whether through isTrigger or onCollision
    public override void collide(GameObject collideObject)
    {
        baseEnemy enemy = collideObject.GetComponent<baseEnemy>();
        Rigidbody2D collidePhysics = collideObject.GetComponent<Rigidbody2D>();
        if (enemy != null)
        {
            enemy.isDamaged(getPlayerDamage());
            enemy.stunEnemy(getPlayerLock());
            float xPush = Mathf.Cos(getAngle() * Mathf.Deg2Rad) * getPlayerPushBack();
            float yPush = Mathf.Sin(getAngle() * Mathf.Deg2Rad) * getPlayerPushBack();
            collidePhysics.velocity *= 0;
            collidePhysics.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        if(collideObject.tag == "Untagged")
        {
            Destroy(gameObject);
        }
    }
}
