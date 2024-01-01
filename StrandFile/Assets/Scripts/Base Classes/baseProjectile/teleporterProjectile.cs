using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UIElements;

public class teleporterProjectile : baseProjectile
{
    Animator objectAnimator;
    List<GameObject> objectsInHit = new List<GameObject>();
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (objectsInHit.Contains(collidedObject))
        {
            objectsInHit.Remove(collidedObject);
        }
    }

    // What happens when projectile hits an object whether through isTrigger or onCollision
    public override void collide(GameObject collideObject)
    {
        if (!objectsInHit.Contains(collideObject))
        {
            objectsInHit.Add(collideObject);
        }
    }

    public bool getIfIsColliding()
    {
        return objectsInHit.Count != 0;
    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        objectAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (getIfIsColliding())
        {
            objectAnimator.SetBool("IsColliding", true);
        }
        else
        {
            objectAnimator.SetBool("IsColliding", false);
        }
    }
}
