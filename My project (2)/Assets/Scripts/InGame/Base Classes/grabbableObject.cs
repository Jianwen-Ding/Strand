using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --THIS SCRIPT SERVES AS THE BASE CLASS FOR ALL GRABBABLE OBJECTS--
public class grabbableObject : MonoBehaviour
{
    #region variables
    //Setup variables
    private Collider2D objectCollider;
    private SpriteRenderer objectRender;
    private SpriteYLayering objectLayerScript;
    private Rigidbody2D objectPhysics;
    private GameObject grabbedByObject;
    private playerHand grabbedByObjectScript;
    private SpriteRenderer grabbedByObjectRender;
    private int originalLayer;
    private Color originialColor;
    //localScale
    private Vector3 originalScale; 
    private float originalDrag;
    [SerializeField]
    private float grabShrink = 0.5f;
    //Grab Variables
    [SerializeField]
    private bool hasBeenGrabbed;
    //Slash/Use Variables
    [SerializeField]
    private int durability = 3;
    [SerializeField]
    private int slashKnockBackStrength = 30;
    [SerializeField]
    private float slashStunTime;
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private float slashTime;
    [SerializeField]
    private float slashTimeLeft;
    [SerializeField]
    private float slashCooldown;
    [SerializeField]
    private float slashCooldownTimeLeft;
    [SerializeField]
    private Color slashColor = Color.gray;
    [SerializeField]
    private Color fullVelSlashColor = Color.red;
    [SerializeField]
    private float velocityThreshold;
    List<GameObject> objectsHit = new List<GameObject>();
    //--thrown state, occurs when thrown
    [SerializeField]
    private float throwVelocityToHitThreshold;
    [SerializeField]
    private int throwDamage;
    [SerializeField]
    private float throwStunTme;
    [SerializeField]
    private int throwKnockback;
    [SerializeField]
    private float thrownStateTime;
    [SerializeField]
    private float thrownStateTimeLeft;
    [SerializeField]
    private float thrownDrag;
    //other
    [SerializeField]
    private float releaseStateTime;
    #endregion
    //private functions
    public void push(float angle, float strength)
    {
        float xPush = Mathf.Cos(angle * Mathf.Deg2Rad) * strength;
        float yPush = Mathf.Sin(angle * Mathf.Deg2Rad) * strength;
        objectPhysics.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
    }
    //--public functions--
    //get/set functions
    public float getThrowVelocityThreshold()
    {
        return throwVelocityToHitThreshold;
    }
    public float getThrowStunTime()
    {
        return throwStunTme;
    }
    public float getThrowKnockback()
    {
        return throwKnockback;
    }
    public float getThrownStateTimeLeft()
    {
        return thrownStateTimeLeft;
    }
    public int getThrowDamage()
    {
        return throwDamage;
    }
    public float getVelocityThreshold()
    {
        return velocityThreshold;
    }
    public playerHand getGrabbedByObjectScript()
    {
        return grabbedByObjectScript;
    }
    public int getDurability()
    {
        return durability;
    }
    public void setDurability(int set)
    {
        durability = set;
    }
    public Rigidbody2D getObjectPhysics()
    {
        return objectPhysics;
    }
    public float getThrownStateTime()
    {
        return thrownStateTime;
    }
    public float getReleaseStateTime()
    {
        return releaseStateTime;
    }
    //grabbed
    public virtual bool grabbedEffect(GameObject grabbedBy)
    {
        objectCollider.isTrigger = true;
        gameObject.layer = 8;
        grabbedByObject = grabbedBy;
        grabbedByObjectScript = grabbedByObject.GetComponent<playerHand>();
        grabbedByObjectRender = grabbedByObject.GetComponent<SpriteRenderer>();
        hasBeenGrabbed = true;
        gameObject.transform.localScale = gameObject.transform.localScale * grabShrink;
        if(objectLayerScript != null)
        {
            objectLayerScript.enabled = false;
        }
        if (grabbedByObjectScript == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " has been grabbed by something without the required component -playerHand-");
        }
        return true;
    }
    public virtual void whileGrabbedEffect()
    {
        gameObject.transform.position = grabbedByObject.transform.position;
        objectRender.sortingOrder = grabbedByObjectRender.sortingOrder;
    }
    //slashing/using
    public virtual bool startSlashEffect()
    {
        if (slashCooldownTimeLeft < 0)
        {
            objectRender.color = slashColor;
            slashTimeLeft = slashTime;
            return true;
        }
        else
        {
            return false;
        }
    }
    public virtual void whileSlashingEffect()
    {
        if (grabbedByObjectScript.getAngleVelocity() > velocityThreshold)
        {
            objectRender.color = fullVelSlashColor;
        }
        else
        {
            objectRender.color = slashColor;
        }
        gameObject.transform.position = grabbedByObject.transform.position;
        objectRender.sortingOrder = grabbedByObjectRender.sortingOrder;
    }
    public virtual void slashEnd()
    {
        slashCooldownTimeLeft = slashCooldown;
        objectRender.color = originialColor;
        objectsHit.Clear();
    }
    public virtual bool slashObject(GameObject slashedObject, Vector3 slashFromLocation)
    {
        bool hasHitObject = false;
        if(grabbedByObjectScript.getAngleVelocity() > velocityThreshold && objectsHit.IndexOf(slashedObject) == -1)
        {
            objectsHit.Add(slashedObject);
            //Pushes back object
            Rigidbody2D slashedRigidbody2D = slashedObject.GetComponent<Rigidbody2D>();
            baseEnemy slashedEnemyScript = slashedObject.GetComponent<baseEnemy>();
            if (slashedRigidbody2D != null)
            {
                float Angle = Mathf.Rad2Deg * Mathf.Atan2(slashedObject.transform.position.y - slashFromLocation.y, slashedObject.transform.position.x - slashFromLocation.x);
                hasHitObject = true;
                float xPush = Mathf.Cos(Angle * Mathf.Deg2Rad) * slashKnockBackStrength;
                float yPush = Mathf.Sin(Angle * Mathf.Deg2Rad) * slashKnockBackStrength;
                slashedRigidbody2D.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            }
            if(slashedEnemyScript != null)
            {
                hasHitObject = true;
                slashedEnemyScript.isDamaged(damage);
                slashedEnemyScript.stunEnemy(slashStunTime); 
            }
            if (hasHitObject)
            {
                durability -= 1;
                if(durability < 0)
                {
                    durabilityGone();
                }
            }
        }
        return hasHitObject;
    }
    public virtual void durabilityGone()
    {
        grabbedByObjectScript.stopAttemptSlash();
        grabbedByObjectScript.releaseObject();
        Destroy(gameObject);
    }
    //thrown
    public virtual void throwEffect(float strength, float angle)
    {
        hasBeenGrabbed = false;
        objectPhysics.drag = 0;
        objectRender.color = originialColor;
        objectCollider.isTrigger = false;
        gameObject.transform.localScale = originalScale;
        thrownStateTimeLeft = thrownStateTime;
        push(angle, strength);
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = true;

        }
    }
    public virtual void whileThrownEffect()
    {
    }
    public virtual void throwHitEffect(Collision2D collisionInput)
    {
        thrownEnd();
    }
    public virtual void thrownEnd()
    {
        objectPhysics.drag = originalDrag;
        gameObject.layer = originalLayer;
    }
    //released, independent of thrown
    public virtual void releasedEffect()
    {
        objectCollider.isTrigger = false;
        gameObject.transform.localScale = originalScale;
        thrownStateTimeLeft = releaseStateTime;
        hasBeenGrabbed = false;
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = true;

        }
    }
    //On collide
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(thrownStateTimeLeft >= 0)
        {
            throwHitEffect(collision);
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        objectPhysics = gameObject.GetComponent<Rigidbody2D>();
        if (objectPhysics == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " does not have a 2D rigidbody");
        }
        originalLayer = gameObject.layer;
        objectCollider = gameObject.GetComponent<Collider2D>();
        if(objectCollider == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " does not have a 2D collider");
        }
        objectRender = gameObject.GetComponent<SpriteRenderer>();
        if (objectRender == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " does not have a sprite renderer");
        }
        objectLayerScript = gameObject.GetComponent<SpriteYLayering>();
        originialColor = objectRender.color;
        originalDrag = objectPhysics.drag;
        originalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (grabbedByObjectScript != null && hasBeenGrabbed && slashTimeLeft >= 0)
        {
            slashTimeLeft -= Time.deltaTime;
            if(slashTimeLeft < 0)
            {
                grabbedByObjectScript.stopAttemptSlash();
            }
        }
        if (slashCooldownTimeLeft >= 0)
        {
            slashCooldownTimeLeft -= Time.deltaTime;
        }
        if (thrownStateTimeLeft >= 0)
        {
            whileThrownEffect();
            thrownStateTimeLeft -= Time.deltaTime;
            if(thrownStateTimeLeft < 0)
            {
                thrownEnd();
            }
        }
        if(grabbedByObjectScript != null && hasBeenGrabbed)
        {
            if (grabbedByObjectScript.getGrabState() == "grabbed")
            {
                whileGrabbedEffect();
            }
            if (grabbedByObjectScript.getGrabState() == "slashing")
            {
                whileSlashingEffect();
            }
        }
    }
}
