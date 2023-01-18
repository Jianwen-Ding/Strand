using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    //--Variables for Modification--
    [SerializeField]
    private bool angleChange;
    [SerializeField]
    private float grabShrink = 1;
    //Slash/Use Variables
    [SerializeField]
    private int durability;
    [SerializeField]
    private int slashKnockBackStrength;
    [SerializeField]
    private int damage;
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
    //--Release intagibility, switches layer of the object when grabbed so it does not collide with player directly on release--
    [SerializeField]
    private float releaseIntagibilityTime;
    [SerializeField]
    private float releaseIntagibilityTimeLeft;
    #endregion
    //private functions
    public void push(float angle, float strength)
    {
        float xPush = Mathf.Cos(angle * Mathf.Deg2Rad) * strength;
        float yPush = Mathf.Sin(angle * Mathf.Deg2Rad) * strength;
        objectPhysics.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
    }
    //get/set functions
    public float getVelocityThreshold()
    {
        return velocityThreshold;
    }
    //public functions
    public virtual void grabbedEffect(GameObject grabbedBy)
    {
        objectCollider.isTrigger = true;
        gameObject.layer = 8;
        grabbedByObject = grabbedBy;
        grabbedByObjectScript = grabbedByObject.GetComponent<playerHand>();
        grabbedByObjectRender = grabbedByObject.GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = gameObject.transform.localScale * grabShrink;
        if(objectLayerScript != null)
        {
            objectLayerScript.enabled = false;
        }
        if (grabbedByObjectScript == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " has been grabbed by something without the required component -playerHand-");
        }
    }
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
    }
    public virtual void slashEnd()
    {
        slashCooldownTimeLeft = slashCooldown;
        objectRender.color = slashColor;
    }
    public virtual bool slashObject(GameObject slashedObject, float Angle)
    {
        bool hasHitObject = false;
        //Pushes back object
        Rigidbody2D slashedRigidbody2D = slashedObject.GetComponent<Rigidbody2D>();
        if(slashedRigidbody2D != null)
        {
            hasHitObject = true;
            float xPush = Mathf.Cos(Angle * Mathf.Deg2Rad) * slashKnockBackStrength;
            float yPush = Mathf.Sin(Angle * Mathf.Deg2Rad) * slashKnockBackStrength;
            slashedRigidbody2D.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            grabbedByObjectScript.stopAttemptSlash();
        }
        return hasHitObject;
    }
    public virtual void whileGrabbedEffect()
    {
        gameObject.transform.position = grabbedByObject.transform.position;
        objectRender.sortingOrder = grabbedByObjectRender.sortingOrder;
    }
    public virtual void throwEffect(float strength, float angle)
    {
        objectCollider.isTrigger = false;
        gameObject.transform.localScale = gameObject.transform.localScale / grabShrink;
        releaseIntagibilityTimeLeft = releaseIntagibilityTime;
        push(angle, strength);
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = true;

        }
    }
    public virtual void releasedEffect()
    {
        objectCollider.isTrigger = false;
        gameObject.transform.localScale = gameObject.transform.localScale / grabShrink;
        releaseIntagibilityTimeLeft = releaseIntagibilityTime;
        if (objectLayerScript != null)
        {
            objectLayerScript.enabled = true;

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        objectPhysics = gameObject.GetComponent<Rigidbody2D>();
        if (objectCollider == null)
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
    }

    // Update is called once per frame
    void Update()
    {
        if (slashTimeLeft >= 0)
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
        if (releaseIntagibilityTimeLeft >= 0)
        {
            releaseIntagibilityTimeLeft -= Time.deltaTime;
            if(releaseIntagibilityTimeLeft < 0)
            {
                gameObject.layer = originalLayer;
            }
        }
        if(grabbedByObjectScript != null)
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
