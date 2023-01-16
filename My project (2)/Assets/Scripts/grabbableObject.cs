using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableObject : MonoBehaviour
{
    #region variables
    //Setup variables
    private Collider2D objectCollider;
    private SpriteRenderer objectRender;
    private Rigidbody2D objectPhysics;
    private GameObject grabbedByObject;
    private playerHand grabbedByObjectScript;
    private SpriteRenderer grabbedByObjectRender;
    private int originalLayer;
    //Variables for Modification
    [SerializeField]
    private int durability;
    [SerializeField]
    private bool angleChange;
    [SerializeField]
    private float grabShrink = 1;
    //Release intagibility, switches layer of the object when grabbed so it does not collide with player directly on release
    [SerializeField]
    private float releaseIntagibilityTime;
    [SerializeField]
    private float releaseIntagibilityTimeLeft;
    //State Variables
    [SerializeField]
    private bool hasBeenGrabbed;
    #endregion
    //public functions
    public bool getHasBeenGrabbed()
    {
        return hasBeenGrabbed;
    }
    public virtual void grabbedEffect(GameObject grabbedBy)
    {
        objectCollider.isTrigger = true;
        gameObject.layer = 8;
        hasBeenGrabbed = true;
        grabbedByObject = grabbedBy;
        grabbedByObjectScript = grabbedByObject.GetComponent<playerHand>();
        grabbedByObjectRender = grabbedByObject.GetComponent<SpriteRenderer>();
        gameObject.transform.localScale = gameObject.transform.localScale * grabShrink;
        if (grabbedByObjectScript == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " has been grabbed by something without the required component -playerHand-");
        }
    }
    public virtual void slashEffect(GameObject slashedObject)
    {

    }
    public virtual void whileGrabbedEffect()
    {
        gameObject.transform.position = grabbedByObject.transform.position;
        objectRender.sortingOrder = grabbedByObjectRender.sortingOrder;
    }
    public virtual void throwEffect(float strength, float angle)
    {
        objectCollider.isTrigger = false;
        hasBeenGrabbed = false;
        gameObject.transform.localScale = gameObject.transform.localScale / grabShrink;
        releaseIntagibilityTimeLeft = releaseIntagibilityTime;
       
    }
    public virtual void releasedEffect()
    {
        objectCollider.isTrigger = false;
        hasBeenGrabbed = false;
        gameObject.transform.localScale = gameObject.transform.localScale / grabShrink;
        releaseIntagibilityTimeLeft = releaseIntagibilityTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        objectPhysics = gameObject.GetComponent<Rigidbody2D>();
        if (objectCollider == null)
        {
            print("ERROR- grabbable object " + gameObject.name + " does not have a 2D collider");
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
    }

    // Update is called once per frame
    void Update()
    {
        if (releaseIntagibilityTimeLeft >= 0)
        {
            releaseIntagibilityTimeLeft -= Time.deltaTime;
            if(releaseIntagibilityTimeLeft < 0)
            {
                gameObject.layer = originalLayer;
            }
        }
        if (hasBeenGrabbed)
        {
            whileGrabbedEffect();
        }
    }
}
