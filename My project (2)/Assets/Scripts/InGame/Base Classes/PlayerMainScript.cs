using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    [SerializeField]
    private Animator objectAnimator;
    [SerializeField]
    private SpriteRenderer objectRenderer;
    [SerializeField]
    private HealthUI healthProjector;
    //Drag in image using serialize field
    [SerializeField]
    private Image transitionImage;
    //health
    [SerializeField]
    private int health;
    [SerializeField]
    private int healthMax;
    [SerializeField]
    private GameObject healthParticlePrefab;
    //death
    [SerializeField]
    private bool isDead;
    [SerializeField]
    private string deathSceneName;
    [SerializeField]
    private float transitionSpeed;
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
    //--slash variables--
    [SerializeField]
    private float angleChangeVelocity;
    [SerializeField]
    private float lastAngle;
    [SerializeField]
    private Queue<float> lastTimeDeltas = new Queue<float>();
    [SerializeField]
    private Queue<float> lastAngleChanges = new Queue<float>();
    [SerializeField]
    private float timeAngleVelocityNormalization;
    //--misc-- 
    [SerializeField]
    private bool hasLeftClicked;
    [SerializeField]
    private bool hasRightClicked;
    [SerializeField]
    private bool movementLocked;
    [SerializeField]
    private float movementLockedTimeLeft;
    #endregion
    //public functions
    public playerHand getHandScript()
    {
        return objectHandScript;
    }
    public void damagePlayer(int damagePlayer)
    {
        if (!isDead)
        {
            health -= damagePlayer;
            if (health <= 0)
            {
                objectAnimator.SetBool("IsDead", true);
                objectAnimator.SetBool("IsMoving", false);
                isDead = true;
            }
            else
            {
                objectAnimator.SetTrigger("HasBeenHurt");
            }
            healthProjector.updateHealthIconList(health);
        }
    }
    public void healPlayer(int damageHealed)
    {
        if (!isDead)
        {
            if(damageHealed > 0)
            {
                Instantiate(healthParticlePrefab, transform.position, Quaternion.identity.normalized);
            }
            health += damageHealed;
            if (health > healthMax)
            {
                health = healthMax;
            }
            healthProjector.updateHealthIconList(health);
        }
    }
    public float getAngleVelocity()
    {
        return angleChangeVelocity;
    }
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
        objectRigid.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
    }
    //private functions

    // Start is called before the first frame update
    void Start()
    {
        objectRigid = gameObject.GetComponent<Rigidbody2D>();
        objectCollider = gameObject.GetComponent<Collider2D>();
        objectHandScript = objectHand.GetComponent<playerHand>();
        objectAnimator = gameObject.GetComponent<Animator>();
        objectRenderer = gameObject.GetComponent<SpriteRenderer>();
        healthProjector = GameObject.FindGameObjectWithTag("HeartUI").GetComponent<HealthUI>();
        healthProjector.setUpHealthIcons(healthMax, health);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (isGrabbingTimeLeft >= 0)
            {
                isGrabbingTimeLeft -= Time.deltaTime;
            }
            else
            {
                //Determines angle between player and mouse
                angleFace = Mathf.Rad2Deg * Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - gameObject.transform.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - gameObject.transform.position.x);
                lastAngleChanges.Enqueue(angleFace - lastAngle);
                lastTimeDeltas.Enqueue(Time.deltaTime);
                lastAngle = angleFace;
                float totalTimeDelta = 0;
                float totalAngleChange = 0;
                Queue<float> tempTimeDeltas = new Queue<float>(lastTimeDeltas);
                Queue<float> tempAngleChanges = new Queue<float>(lastAngleChanges);
                while (tempTimeDeltas.Count > 0)
                {
                    totalTimeDelta += tempTimeDeltas.Dequeue();
                    totalAngleChange += tempAngleChanges.Dequeue();
                }
                angleChangeVelocity = Mathf.Abs(totalAngleChange) / totalTimeDelta;
                if (totalTimeDelta > timeAngleVelocityNormalization)
                {
                    lastTimeDeltas.Dequeue();
                    lastAngleChanges.Dequeue();
                }
            }
            if (grabTimeLeft >= 0)
            {
                grabTimeLeft -= Time.deltaTime;
                if (grabTimeLeft < 0)
                {
                    endGrab();
                }
            }
            //Movement
            bool hasMoved = false;
            if (!movementLocked)
            {
                //Uses rigid body, requires high drag to make it feel responsive
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    if (objectRigid.velocity.x >= -walkSpeed)
                    {
                        objectRigid.velocity = new Vector3(-walkSpeed, objectRigid.velocity.y);
                    }
                    //Faces left
                    objectRenderer.flipX = true;
                    hasMoved = true;
                }
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    if (objectRigid.velocity.x <= walkSpeed)
                    {
                        objectRigid.velocity = new Vector3(walkSpeed, objectRigid.velocity.y);
                    }
                    //Faces right
                    objectRenderer.flipX = false;
                    hasMoved = true;
                }
                if (Input.GetAxisRaw("Verticle") < 0)
                {
                    if (objectRigid.velocity.y <= walkSpeed)
                    {
                        objectRigid.velocity = new Vector3(objectRigid.velocity.x, walkSpeed);
                    }
                    hasMoved = true;
                }
                if (Input.GetAxisRaw("Verticle") > 0)
                {
                    if (objectRigid.velocity.y >= -walkSpeed)
                    {
                        objectRigid.velocity = new Vector3(objectRigid.velocity.x, -walkSpeed);
                    }
                    hasMoved = true;
                }
                if (Input.GetAxisRaw("Drop Object") > 0)
                {
                    if (objectHandScript.getGrabState() == "grabbed")
                    {
                        objectHandScript.releaseObject();
                    }
                    hasMoved = true;
                }
                //This makes sure only one click from the mouse gets registered
                if (Input.GetMouseButtonDown(0))
                {
                    if (!hasLeftClicked)
                    {
                        hasLeftClicked = true;
                        //Slash/ Use
                        if (objectHandScript.getGrabState() == "grabbed")
                        {
                            objectHandScript.attemptSlash();
                        }
                        //Stand Grab
                        else if (objectHandScript.getGrabState() == "none" && grabTimeLeft < 0)
                        {
                            grabTimeLeft = standGrabTimeCooldown;
                            lockMovement(0.04f);
                            isGrabbingTimeLeft = standGrabbingTime;
                            objectHandScript.attemptGrab();
                        }
                    }
                }
                else
                {
                    hasLeftClicked = false;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    objectHandScript.stopAttemptSlash();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    if (!hasRightClicked)
                    {
                        hasRightClicked = true;
                        //Throw
                        if (objectHandScript.getGrabState() == "grabbed")
                        {
                            objectHandScript.attemptThrow(throwStrength, angleFace);
                        }
                        //Lunge Grab
                        else if (objectHandScript.getGrabState() == "none" && grabTimeLeft < 0)
                        {
                            //facing right
                            if (angleFace < 90 && angleFace > -90)
                            {
                                objectRenderer.flipX = false;
                            }
                            //facing left
                            else
                            {
                                objectRenderer.flipX = true;
                            }
                            push(angleFace, lungeStrength);
                            grabTimeLeft = lungeGrabTimeCooldown;
                            lockMovement(0.08f);
                            objectCollider.sharedMaterial.bounciness = grabBounce;
                            isGrabbingTimeLeft = lungeGrabbingTime;
                            objectHandScript.attemptGrab();
                            objectAnimator.SetTrigger("HasDashed");
                        }
                    }
                }
                else
                {
                    hasRightClicked = false;
                }
            }
            else
            {
                movementLockedTimeLeft -= Time.deltaTime;
                if (movementLockedTimeLeft < 0)
                {
                    movementLocked = false;
                }
            }
            objectAnimator.SetBool("IsMoving", hasMoved);
        }
        else
        {
            transitionImage.gameObject.SetActive(true);
            Time.timeScale = (float)0.1;
            if (transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed >= 1)
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);
                SceneManager.LoadScene(deathSceneName);
            }
            else
            {
                transitionImage.color = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, transitionImage.color.a + Time.fixedDeltaTime * transitionSpeed);
            }
        }
    }
}
