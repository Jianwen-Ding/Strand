using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainScript : MonoBehaviour
{
    #region Variables
    //establishing variables
    private Rigidbody2D objectRigid;
    private Collider2D objectCollider;
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
    private float lungeGrabTime;
    //Checks if grab can be used
    [SerializeField]
    private float grabTimeCooldown;
    [SerializeField]
    private float grabTimeLeft;
    //misc 
    [SerializeField]
    private bool movementLocked;
    [SerializeField]
    private float movementLockedTimeLeft;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        objectRigid = gameObject.GetComponent<Rigidbody2D>();
        objectCollider = gameObject.GetComponent<Collider2D>();
    }
    //Locks movement of the character for a set periord of time
    public void lockMovement(float time)
    {
        movementLocked = true;
        movementLockedTimeLeft = time;
    }
    public void unlock()
    {
        movementLocked = false;
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
    // Update is called once per frame
    void Update()
    {
        if(isGrabbingTimeLeft >= 0)
        {
            isGrabbingTimeLeft -= Time.deltaTime;
        }
        if(grabTimeLeft >= 0)
        {
            grabTimeLeft -= Time.deltaTime;
            if(grabTimeLeft < 0)
            {
                objectCollider.sharedMaterial.bounciness = 0f;
            }
        }
        //Determines angle between player and mouse
        angleFace = Mathf.Rad2Deg * Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x);
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
            //Lunge Grab Mechanic
            if (Input.GetMouseButtonDown(0) && grabTimeLeft < 0)
            {
                push(angleFace, lungeStrength);
                grabTimeLeft = grabTimeCooldown;
                lockMovement(0.08f);
                objectCollider.sharedMaterial.bounciness = grabBounce;
                isGrabbingTimeLeft = lungeGrabTime;
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
