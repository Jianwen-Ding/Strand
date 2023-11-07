using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --THIS SCRIPT SERVES AS THE BASE CLASS FOR ALL ENEMIES--

public class baseEnemy : MonoBehaviour
{
    #region
    //Cache Variables
    private Rigidbody2D enemyRigid;
    private SpriteRenderer enemyRender;
    private Color originalColor;
    private GameObject playerObject;
    private enemyAudio cacheAudio;
    //Parameter of state named "EnemyState"
    //1 - Default
    //2 - Stunned
    //3 - Death
    private Animator objectAnimator;
    //-State-
    //default
    //stunned
    //death
    [SerializeField]
    private string enemyState;
    [SerializeField]
    private float timeStunOnDamage;
    [SerializeField]
    private float timeStunnedLeft;
    [SerializeField]
    private Color stunColor;
    //-Health-
    [SerializeField]
    private int health;
    [SerializeField]
    private float playerTouchPushbackOnPlayer;
    [SerializeField]
    private float playerTouchPushbackOnEnemy;
    [SerializeField]
    private float stunOnPush;
    [SerializeField]
    float playerTouchMovementLockTime;
    //-GRAB ARMOR-
    [SerializeField]
    float failedGrabPushBackEnemy;
    [SerializeField]
    float failedGrabPushBackPlayer;
    //grabArmor determines if an enemy can be grabbed or not
    [SerializeField]
    private int grabArmor;
    //max grab armor, will default to after being succesfully grabbed
    [SerializeField]
    private int defaultGrabArmor;
    //Death variables
    [SerializeField]
    private bool hasDied;
    [SerializeField]
    private float timeUntilDestruct;
    [SerializeField]
    private float timeLeftUntilDestruct;
    [SerializeField]
    private GameObject destructionResidue;
    #endregion
    //private functions
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        onContact(collision);
    }
    //Deals with contact
    public virtual void onContact(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        PlayerMainScript colliderPlayerScript = collisionObject.GetComponent<PlayerMainScript>();
        grabbableObject colliderGrabbableScript = collisionObject.GetComponent<grabbableObject>();
        if (colliderGrabbableScript != null)
        {
            if(!(colliderGrabbableScript != null && colliderGrabbableScript.getThrownDamageTimeLeft() >= 0))
            {
                print("wow");
            }
            print("Result: " + (colliderGrabbableScript != null && colliderGrabbableScript.getThrownDamageTimeLeft() >= 0));
            print("Time Left: " + colliderGrabbableScript.getThrownDamageTimeLeft());
            print("Velocity Magnitude: " + Mathf.Sqrt(colliderGrabbableScript.getObjectPhysics().velocity.y * colliderGrabbableScript.getObjectPhysics().velocity.y + colliderGrabbableScript.getObjectPhysics().velocity.x * colliderGrabbableScript.getObjectPhysics().velocity.x));
        }
        //if enemy collides with player
        if (colliderPlayerScript != null)
        {
            colliderPlayerScript.damagePlayer(1);
            colliderPlayerScript.lockMovement(playerTouchMovementLockTime);
            stunEnemy(stunOnPush);
            //-pushes both characters back
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(gameObject.transform.position.y - collisionObject.transform.position.y, gameObject.transform.position.x - collisionObject.gameObject.transform.position.x);
            //pushes enemy back
            float xPush = Mathf.Cos(Angle * Mathf.Deg2Rad) * playerTouchPushbackOnEnemy;
            float yPush = Mathf.Sin(Angle * Mathf.Deg2Rad) * playerTouchPushbackOnEnemy;
            enemyRigid.velocity *= 0;
            enemyRigid.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            Rigidbody2D collisionRigid = collisionObject.gameObject.GetComponent<Rigidbody2D>();
            collisionRigid.velocity *= 0;
            collisionRigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
        }
        
        //if enemy collides with thrown/fast flying grabbable object
        else if (colliderGrabbableScript != null && colliderGrabbableScript.getThrownDamageTimeLeft() >= 0)
        {
            print("wow");
            //print(colliderGrabbableScript.getVelocityThreshold());
            //Checking the magnitude of the velocity
            float colliderVelocitySum = Mathf.Sqrt(colliderGrabbableScript.getObjectPhysics().velocity.y * colliderGrabbableScript.getObjectPhysics().velocity.y + colliderGrabbableScript.getObjectPhysics().velocity.x * colliderGrabbableScript.getObjectPhysics().velocity.x);
            if (colliderVelocitySum >= colliderGrabbableScript.getThrowVelocityThreshold())
            {       
                colliderGrabbableScript.throwHitEffect(collision);
                isDamaged(colliderGrabbableScript.getThrowDamage());
                float colliderAngle = Mathf.Atan2(colliderGrabbableScript.getObjectPhysics().velocity.y, colliderGrabbableScript.getObjectPhysics().velocity.x);
                float xPush = Mathf.Cos(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                float yPush = Mathf.Sin(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                enemyRigid.velocity *= 0;
                enemyRigid.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
                Rigidbody2D collisionRigid = collisionObject.gameObject.GetComponent<Rigidbody2D>();
                collisionRigid.velocity *= 0;
                collisionRigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
            }
        }

    }

    //public function
    //get/set functions
    public virtual int getGrabArmor()
    {
        return grabArmor;
    }
    public virtual void setGrabArmor(int setArmor)
    {
        grabArmor = setArmor;
    }
    public virtual void loseGrabArmor(int lostArmor)
    {
        grabArmor -= lostArmor;
    }
    public virtual int getDefaultGrabArmor()
    {
        return defaultGrabArmor;
    }
    public virtual float getFailedGrabPushBackEnemy()
    {
        return failedGrabPushBackEnemy;
    }
    public virtual float getFailedGrabPushBackPlayer()
    {
        return failedGrabPushBackPlayer;
    }
    public virtual float getStunOnPush()
    {
        return stunOnPush;
    }
    public virtual string getEnemyState()
    {
        return enemyState;
    }
    public virtual void setEnemyState(string setString)
    {
        enemyState = setString;
    }
    public virtual GameObject getPlayerObject()
    {
        return playerObject;
    }
    public virtual Animator getObjectAnimator()
    {
        return objectAnimator;
    }
    public virtual Rigidbody2D getObjectRigidbody()
    {
        return enemyRigid;
    }
    public virtual SpriteRenderer getRenderer()
    {
        return enemyRender;
    }
    //Helper functions
    public virtual void stunEnemy(float time)
    {
        cacheAudio.playSound(1, 0);
        enemyState = "stunned";
        timeStunnedLeft = time;
        enemyRender.color = stunColor;
    }
    public virtual void destunEnemy()
    {
        timeStunnedLeft = -1;
        enemyState = "default";
        enemyRender.color = originalColor;
    }
    public virtual void onDeath()
    {
        hasDied = true;
        enemyState = "death";
    }
    public virtual void onFailedGrab()
    {
    }
    //health
    public virtual void isDamaged(int damage)
    {
        stunEnemy(timeStunOnDamage);
        grabArmor -= damage;
        health -= damage;
        cacheAudio.playSound(0,0);
        if(health <= 0)
        {
            cacheAudio.playSound(2, 0);
            onDeath();
        }
    }
    //updates depends on state
    public virtual void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                objectAnimator.SetInteger("EnemyState", 1);
                break; 
            case "stunned":
                objectAnimator.SetInteger("EnemyState", 2);
                break;
            case "death":
                objectAnimator.SetInteger("EnemyState", 3);
                break;
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        cacheAudio = gameObject.GetComponent<enemyAudio>();
        objectAnimator = gameObject.GetComponent<Animator>();
        timeLeftUntilDestruct = timeUntilDestruct;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyRigid = gameObject.GetComponent<Rigidbody2D>();
        enemyRender = gameObject.GetComponent<SpriteRenderer>();
        originalColor = enemyRender.color;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(timeStunnedLeft >= 0)
        {
            timeStunnedLeft -= Time.deltaTime;
            if(timeStunnedLeft < 0)
            {
                destunEnemy();
            }
        }
        if (hasDied)
        {
            timeLeftUntilDestruct -= Time.deltaTime;
            if(timeLeftUntilDestruct <= 0)
            {
                Instantiate(destructionResidue, transform.position, Quaternion.identity.normalized);
                Destroy(gameObject);
            }
        }
        stateUpdate(enemyState);
    }
}
