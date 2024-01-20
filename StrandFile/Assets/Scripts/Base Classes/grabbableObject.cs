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
    private Animator objectAnimator;
    private GameObject grabbedByObject;
    private playerHand grabbedByObjectScript;
    private SpriteRenderer grabbedByObjectRender;
    private int originalLayer;
    private Color originialColor;
    //localScale
    private float originalAngle;
    private Vector3 originalScale; 
    private float originalDrag;
    [SerializeField]
    private float grabShrink = 0.5f;
    //Grab Variables
    private bool hasBeenGrabbed;
    [SerializeField]
    private bool willPointOutwards;
    [SerializeField]
    private float adjustAngleCheckY;
    //Slash/Use Variables
    [SerializeField]
    private bool isActivelySlashing;
    [SerializeField]
    private bool signifiesSlash;
    [SerializeField]
    private bool signifiesDamage;
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
    //The time the thrown object goes for a set velocity
    [SerializeField]
    private float thrownStateTime;
    private float thrownStateTimeLeft;
    //Time that a thrown object can hit for damage
    [SerializeField]
    private float thrownDamageTime;
    [SerializeField]
    private float thrownDamageTimeLeft;
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
    public bool getHasBeenGrabbed()
    {
        return hasBeenGrabbed;
    }
    public bool getIsActivelySlashing()
    {
        return isActivelySlashing;
    }
    public virtual bool getSignifiesSlash()
    {
        return signifiesSlash;
    }
    public bool getSignifiesDamage()
    {
        return signifiesDamage;
    }
    public Animator getObjectAnimator()
    {
        return objectAnimator;
    }
    public List<GameObject> getObjectsHit()
    {
        return objectsHit;
    }
    public float getThrowVelocityThreshold()
    {
        return throwVelocityToHitThreshold;
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
    public PlayerMainScript getGrabbedByPlayerScript()
    {
        return grabbedByObjectScript.getObjectPlayerScript();
    }
    public Rigidbody2D getGrabbedByPlayerPhysics()
    {
        return grabbedByObjectScript.getPlayerObjectPhysics();
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
    public float getThrownDamageTimeLeft()
    {
        return thrownDamageTimeLeft;
    }
    public Collider2D getCollider()
    {
        return objectCollider;
    }
    //grabbed
    public virtual bool grabbedEffect(GameObject grabbedBy)
    {
        objectCollider.enabled = false;
        gameObject.layer = 8;
        grabbedByObject = grabbedBy;
        grabbedByObjectScript = grabbedByObject.GetComponent<playerHand>();
        grabbedByObjectRender = grabbedByObject.GetComponent<SpriteRenderer>();
        originalAngle = transform.rotation.z;
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
        if (willPointOutwards)
        {
            float xDiff = gameObject.transform.position.x - (grabbedByObjectScript.getObjectPlayerScript().transform.position.x + adjustAngleCheckY);
            float yDiff = gameObject.transform.position.y - (grabbedByObjectScript.getObjectPlayerScript().transform.position.y + adjustAngleCheckY);
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(yDiff, xDiff);
            transform.localRotation = Quaternion.Euler(new Vector3(0,0, Angle - 90));
        }
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
        isActivelySlashing = grabbedByObjectScript.getAngleVelocity() > velocityThreshold;
        if (isActivelySlashing)
        {
            objectRender.color = fullVelSlashColor;
        }
        else
        {
            objectRender.color = slashColor;
        }
        gameObject.transform.position = grabbedByObject.transform.position;
        objectRender.sortingOrder = grabbedByObjectRender.sortingOrder;
        if (willPointOutwards)
        {
            float xDiff = gameObject.transform.position.x - (grabbedByObjectScript.getObjectPlayerScript().transform.position.x + adjustAngleCheckY);
            float yDiff = gameObject.transform.position.y - (grabbedByObjectScript.getObjectPlayerScript().transform.position.y + adjustAngleCheckY);
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(yDiff, xDiff);
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Angle - 90));
        }
    }
    public virtual void slashEnd()
    {
        slashCooldownTimeLeft = slashCooldown;
        if(objectRender != null)
        {
            objectRender.color = originialColor;
            objectsHit.Clear();
        }
    }
    public virtual bool slashObject(GameObject slashedObject, Vector3 slashFromLocation)
    {
        bool hasHitObject = false;
        if(grabbedByObjectScript.getAngleVelocity() > velocityThreshold && objectsHit.IndexOf(slashedObject) == -1 && slashedObject != null)
        {
            objectsHit.Add(slashedObject);
            //Pushes back object
            Rigidbody2D slashedRigidbody2D = slashedObject.GetComponent<Rigidbody2D>();
            baseEnemy slashedEnemyScript = slashedObject.GetComponent<baseEnemy>();
            if (slashedRigidbody2D != null)
            {
                float Angle = Mathf.Rad2Deg * Mathf.Atan2(slashedObject.transform.position.y - slashFromLocation.y, slashedObject.transform.position.x - slashFromLocation.x);
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
                durabilityDamage();
            }
        }
        return hasHitObject;
    }
    public virtual void durabilityDamage()
    {
        durability -= 1;
        if(signifiesDamage && durability == 0)
        {
            objectAnimator.SetBool("IsDamaged", true);
        }
        if (durability < 0)
        {
            durabilityGone();
        }
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
        objectCollider.enabled = true;
        gameObject.transform.localScale = originalScale;
        thrownStateTimeLeft = thrownStateTime;
        thrownDamageTimeLeft = thrownDamageTime;
        push(angle, strength);
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = false;

        }
    }
    public virtual void whileThrownEffect()
    {
    }
    public virtual void throwHitEffect()
    {
        thrownDamageTimeLeft = -1;
        thrownEnd();
    }

    public virtual void thrownEnd()
    {
        thrownStateTimeLeft = -1;  
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, originalAngle));
        objectPhysics.drag = originalDrag;
        gameObject.layer = originalLayer;
    }
    //released, independent of thrown
    public virtual void releasedEffect()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, originalAngle));
        objectCollider.enabled = true;
        gameObject.transform.localScale = originalScale;
        thrownStateTimeLeft = releaseStateTime;
        hasBeenGrabbed = false;
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = false;

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
        if (signifiesDamage)
        {
            objectAnimator = gameObject.GetComponent<Animator>();
        }
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
        if( thrownDamageTimeLeft >= 0)
        {
            thrownDamageTimeLeft -= Time.deltaTime;
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
