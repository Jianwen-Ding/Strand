using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables
    //establishing variables
    private Rigidbody2D objectRigid;
    private Collider2D objectCollider;
    [SerializeField]
    private GameObject objectHand;
    [SerializeField]
    private playerHand objectHandScript;
    private float health;
    //the angle that player faces towards the mouse from the right horizontal
    [SerializeField]
    private float angleFace;
    //--Speed of general movement--
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float lungeStrength;
    //--Grab variables--
    [SerializeField]
    private float grabBounce;
    //Checks if player is in process of grabbing
    [SerializeField]
    private float isGrabbingTimeLeft;
    [SerializeField]
    private float lungeGrabbingTime;
    [SerializeField]
    private float standGrabbingTime;
    //Checks if grab can be used
    [SerializeField]
    private float lungeGrabTimeCooldown;
    [SerializeField]
    private float standGrabTimeCooldown;
    [SerializeField]
    private float grabTimeLeft;
    //--throw variables--
    [SerializeField]
    private float throwStrength;
    //--misc-- 
    [SerializeField]
    private bool movementLocked;
    [SerializeField]
    private float movementLockedTimeLeft;
    #endregion
    //public functions
    public void lockMovement(float time)
    {
        movementLocked = true;
        movementLockedTimeLeft = time;
    }
    public void unlockMovement()
    {
        movementLocked = false;
    }
    public float getIsGrabbingTimeLeft()
    {
        return isGrabbingTimeLeft;
    }
    public void setIsGrabbingTimeLeft(float setTime)
    {
        isGrabbingTimeLeft = setTime;
    }
    public void endGrab()
    {
        objectCollider.sharedMaterial.bounciness = 0f;
        objectHandScript.stopGrabAttempt();
    }
    public float getAngleFace()
    {
        return angleFace;
    }
    public void push(float angle, float strength)
    {
        float xPush = Mathf.Cos(angle * Mathf.Deg2Rad) * strength;
        float yPush = Mathf.Sin(angle * Mathf.Deg2Rad) * strength;
        objectRigid.AddForce(new Vector2(xPush, yPush));
    }
    //private functions

    // Start is called before the first frame update
    void Start()
    {
        objectRigid = gameObject.GetComponent<Rigidbody2D>();
        objectCollider = gameObject.GetComponent<Collider2D>();
        objectHandScript = objectHand.GetComponent<playerHand>();
    }
    // Update is called once per frame
    void Update()
    {
        if(isGrabbingTimeLeft >= 0)
        {
            isGrabbingTimeLeft -= Time.deltaTime;
        }
        else
        {
            //Determines angle between player and mouse
            angleFace = Mathf.Rad2Deg * Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x);
        }
        if(grabTimeLeft >= 0)
        {
            grabTimeLeft -= Time.deltaTime;
            if(grabTimeLeft < 0)
            {
                endGrab();
            }
        }
        //Movement
        if (!movementLocked)
        {
            //Uses rigid body, requires high drag to make it feel responsive
            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                if(objectRigid.velocity.x >= -walkSpeed)
                {
                    objectRigid.velocity = new Vector3(-walkSpeed, objectRigid.velocity.y);
                }
            }
            if (Input.GetAxisRaw("Horizontal") > 0)
            {
                if (objectRigid.velocity.x <= walkSpeed)
                {
                    objectRigid.velocity = new Vector3(walkSpeed, objectRigid.velocity.y);
                }
            }
            if (Input.GetAxisRaw("Verticle") < 0)
            {
                if (objectRigid.velocity.y <= walkSpeed)
                {
                    objectRigid.velocity = new Vector3(objectRigid.velocity.x, walkSpeed);
                }
            }
            if (Input.GetAxisRaw("Verticle") > 0)
            {
                if (objectRigid.velocity.y >= -walkSpeed)
                {
                    objectRigid.velocity = new Vector3(objectRigid.velocity.x, -walkSpeed);
                }
            }
            //Grabbing mechanic
            if(objectHandScript.getGrabState() == "none")
            {
                //Lunge Grab
                if (Input.GetMouseButtonDown(0) && grabTimeLeft < 0)
                {
                    push(angleFace, lungeStrength);
                    grabTimeLeft = lungeGrabTimeCooldown;
                    lockMovement(0.08f);
                    objectCollider.sharedMaterial.bounciness = grabBounce;
                    isGrabbingTimeLeft = lungeGrabbingTime;
                    objectHandScript.attemptGrab();
                }
                //Standing Granb
                if (Input.GetMouseButtonDown(1) && grabTimeLeft < 0)
                {
                    grabTimeLeft = standGrabTimeCooldown;
                    lockMovement(0.04f);
                    isGrabbingTimeLeft = standGrabbingTime;
                    objectHandScript.attemptGrab();
                }
            }
            if (objectHandScript.getGrabState() == "grabbed")
            {
                //Slash/Use
                if (Input.GetMouseButtonDown(0) && grabTimeLeft < 0)
                {
                    objectHandScript.attemptThrow(throwStrength, angleFace);
                }
                //Throw
                if (Input.GetMouseButtonDown(1) && grabTimeLeft < 0)
                {
                    objectHandScript.attemptThrow(throwStrength, angleFace);
                }
            }
        }
        else
        {
            movementLockedTimeLeft -= Time.deltaTime;
            if(movementLockedTimeLeft < 0)
            {
                movementLocked = false;
            }
        }
    }
}
