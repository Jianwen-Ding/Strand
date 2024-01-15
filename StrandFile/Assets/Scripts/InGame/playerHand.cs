using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHand : MonoBehaviour
{
    #region variables
    //Cache Vars
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private PlayerMainScript objectPlayerScript;
    [SerializeField]
    private SpriteRenderer objectPlayerRenderScript;
    [SerializeField]
    private Rigidbody2D objectPlayerPhysicsScript;
    [SerializeField]
    private SpriteRenderer objectRenderScript;
    [SerializeField]
    private Sprite defaultHandSprite;
    [SerializeField]
    private Sprite grabHandSprite;
    [SerializeField]
    // Animation states
    // 0 - nothing happening
    // 1 - is grabbing
    // 2 - is idlely slashing
    // 3 - is actively slashing
    private Animator objectHitboxAnim;
    private SpriteRenderer objectHitboxRend;
    //Orbit variables
    [SerializeField]
    private float yAddition;
    [SerializeField]
    private float yRange;
    [SerializeField]
    private float xRange;
    //Grab variables
    [SerializeField]
    private List<GameObject> objectsInRange;
    //states
    //slashing
    //grabbing
    //grabbed
    //none
    [SerializeField]
    private string grabState;
    [SerializeField]
    private GameObject objectGrabbed;
    [SerializeField]
    private grabbableObject grabbedScript;
    [SerializeField]
    private float slashVelocityCutoff;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        objectHitboxAnim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        objectHitboxRend = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        playerObject = gameObject.transform.parent.gameObject;
        objectPlayerScript = playerObject.GetComponent<PlayerMainScript>();
        objectPlayerRenderScript = playerObject.GetComponent<SpriteRenderer>();
        objectPlayerPhysicsScript = playerObject.GetComponent<Rigidbody2D>();
        objectRenderScript = gameObject.GetComponent<SpriteRenderer>();
    }
    //private functions
    private void grabObject(GameObject insertObject)
    {
        if(grabState != "grabbed")
        {
            grabbedScript = insertObject.GetComponent<grabbableObject>();
            if (grabbedScript != null && grabbedScript.grabbedEffect(gameObject))
            {
                objectPlayerScript.endGrab();
                grabState = "grabbed";
                objectRenderScript.enabled = false;
                objectGrabbed = insertObject;
            }
            else if(grabbedScript == null)
            {
                print("ERROR- object grabbed by " + gameObject.name + " does not have require -grabbableObject- script");
            }
        }
    }
    //get/set functions
    public GameObject getGrabbedObject()
    {
        return objectGrabbed;
    }
    public float getAngleVelocity()
    {
        return objectPlayerScript.getAngleVelocity();
    }
    public string getGrabState()
    {
        return grabState;
    }
    public GameObject getPlayerObject()
    {
        return playerObject;
    }
    public PlayerMainScript getObjectPlayerScript()
    {
        return objectPlayerScript;
    }
    public Rigidbody2D getPlayerObjectPhysics()
    {
        return objectPlayerPhysicsScript;
    }
    //public functions
    public void releaseObject()
    {
        if (grabState == "grabbed")
        {
            if (grabState == "slashing" && grabbedScript != null)
            {
                grabbedScript.slashEnd();
            }
            objectRenderScript.enabled = true;
            grabState = "none";
            if(grabbedScript != null)
            {
                grabbedScript.releasedEffect();
            }
            objectGrabbed = null;
            grabbedScript = null;
        }

    }
    public void attemptGrab()
    {
        if(grabState != "grabbed")
        {
            grabState = "grabbing";
            for (int i = 0; i < objectsInRange.Count; i++)
            {
                if (objectsInRange[i]  != null && objectsInRange[i].GetComponent<grabbableObject>() != null)
                {
                    grabObject(objectsInRange[i]);
                    break;
                }
            }
        }
        
    }
    public void stopGrabAttempt()
    {
        if(grabState == "grabbing"){
            grabState = "none";
        }
    }
    public void attemptSlash()
    {
        bool isPossibleToSlash = grabbedScript.startSlashEffect();
        if (grabState == "grabbed" && isPossibleToSlash)
        {
            grabState = "slashing";
        }
    }
    public void stopAttemptSlash()
    {
        if(grabState == "slashing")
        {
            grabbedScript.slashEnd();
            grabState = "grabbed";
        }
       
        
    }
    public void attemptThrow(float strength, float angle)
    {
        if (grabbedScript != null)
        {
            if (grabState == "slashing")
            {
                grabbedScript.slashEnd();
            }
            objectRenderScript.enabled = true;
            grabState = "none";
            grabbedScript.throwEffect(strength, angle);
            objectGrabbed = null;
            grabbedScript = null;
        }
        else
        {
            objectGrabbed = null;
            grabbedScript = null;
            print("ERROR- object being ungrabbed by " + gameObject.name + " does not have require -grabbableObject- script");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsInRange.Add(collision.gameObject);
        if(grabState == "grabbing")
        {
            if (collision.gameObject.GetComponent<grabbableObject>() != null)
            {
                grabObject(collision.gameObject);
            }
        }
        if (grabState == "slashing")
        {
            grabbedScript.slashObject(collision.gameObject, playerObject.transform.position);

        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (grabState == "slashing")
        {
            grabbedScript.slashObject(collision.gameObject, playerObject.transform.position);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsInRange.Remove(collision.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        objectHitboxRend.sortingOrder = objectRenderScript.sortingOrder - 1;
        // Switches hitbox animations
        switch (grabState)
        {
            case "grabbing":
                objectHitboxAnim.SetInteger("hitboxState", 1);
                break;
            case "grabbed":
                objectHitboxAnim.SetInteger("hitboxState", 0);
                break;
            case "slashing":
                if (grabbedScript.getSignifiesSlash())
                {
                    if (grabbedScript.getIsActivelySlashing())
                    {
                        objectHitboxAnim.SetInteger("hitboxState", 3);
                    }
                    else
                    {
                        objectHitboxAnim.SetInteger("hitboxState", 2);
                    }
                }
                else
                {
                    objectHitboxAnim.SetInteger("hitboxState", 0);
                }
                break;
            case "none":
                objectHitboxAnim.SetInteger("hitboxState", 0);
                break;
        }
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, objectPlayerScript.getAngleFace() - 90));
        if (grabState == "grabbing")
        {
            objectRenderScript.sprite = grabHandSprite;
        }
        else if(grabState == "grabbed" && objectGrabbed == null)
        {
            releaseObject();
        }
        else
        {
            objectRenderScript.sprite = defaultHandSprite;
        }
        if(objectPlayerScript.getAngleFace() >= 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder -  15;
        }
        if (objectPlayerScript.getAngleFace() < 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder + 15;
        }
        //uses polar equation for ellispes in order to create effect
        float radius = (xRange*yRange)/Mathf.Sqrt(Mathf.Pow(yRange * Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2) + Mathf.Pow(xRange * Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2));
        gameObject.transform.position = new Vector3(Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.x , Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.y + yAddition);
    }
}
