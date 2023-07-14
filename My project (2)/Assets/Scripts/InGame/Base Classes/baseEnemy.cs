using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --THIS SCRIPT SERVES AS THE BASE CLASS FOR ALL ENEMIES--

public class baseEnemy : MonoBehaviour
{
    #region
    //Setup Variables
    private Rigidbody2D enemyRigid;
    private SpriteRenderer enemyRender;
    private Color originalColor;
    //-State-
    //default
    //stunned
    [SerializeField]
    private string enemyState;
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
    #endregion
    //private functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMainScript colliderPlayerScript = collision.gameObject.GetComponent<PlayerMainScript>();
        grabbableObject colliderGrabbableScript = collision.gameObject.GetComponent<grabbableObject>();
        //if enemy collides with player
        if (colliderPlayerScript != null)
        {
            colliderPlayerScript.damagePlayer(1);
            colliderPlayerScript.lockMovement(playerTouchMovementLockTime);
            //-pushes both characters back
            float Angle = Mathf.Rad2Deg * Mathf.Atan2(gameObject.transform.position.y - collision.gameObject.transform.position.y, gameObject.transform.position.x - collision.gameObject.transform.position.x);
            //pushes enemy back
            float xPush = Mathf.Cos(Angle * Mathf.Deg2Rad) * playerTouchPushbackOnEnemy;
            float yPush = Mathf.Sin(Angle * Mathf.Deg2Rad) * playerTouchPushbackOnEnemy;
            enemyRigid.velocity *= 0;
            enemyRigid.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            Rigidbody2D collisionRigid = collision.gameObject.GetComponent<Rigidbody2D>();
            collisionRigid.velocity *= 0;
            collisionRigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
        }
        //if enemy collides with thrown/fast flying grabbable object
        else if(colliderGrabbableScript != null)
        {
            print(colliderGrabbableScript.getVelocityThreshold());
            //Checking the magnitude of the velocity
            float colliderVelocitySum = Mathf.Sqrt(colliderGrabbableScript.getObjectPhysics().velocity.y * colliderGrabbableScript.getObjectPhysics().velocity.y + colliderGrabbableScript.getObjectPhysics().velocity.x * colliderGrabbableScript.getObjectPhysics().velocity.x);
            if(colliderVelocitySum >= colliderGrabbableScript.getThrowVelocityThreshold())
            {
                isDamaged(colliderGrabbableScript.getThrowDamage());
                stunEnemy(colliderGrabbableScript.getThrowStunTime());
                float colliderAngle = Mathf.Atan2(colliderGrabbableScript.getObjectPhysics().velocity.y, colliderGrabbableScript.getObjectPhysics().velocity.x);
                float xPush = Mathf.Cos(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                float yPush = Mathf.Sin(colliderAngle) * colliderGrabbableScript.getThrowKnockback();
                enemyRigid.velocity *= 0;
                enemyRigid.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
                Rigidbody2D collisionRigid = collision.gameObject.GetComponent<Rigidbody2D>();
                collisionRigid.velocity *= 0;
                collisionRigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
            }
        }
    }
    //public function
    //get/set functions
    public int getGrabArmor()
    {
        return grabArmor;
    }
    public void setGrabArmor(int setArmor)
    {
        grabArmor = setArmor;
    }
    public void loseGrabArmor(int lostArmor)
    {
        grabArmor -= lostArmor;
    }
    public int getDefaultGrabArmor()
    {
        return defaultGrabArmor;
    }
    public float getFailedGrabPushBackEnemy()
    {
        return failedGrabPushBackEnemy;
    }
    public float getFailedGrabPushBackPlayer()
    {
        return failedGrabPushBackPlayer;
    }
    public void stunEnemy(float time)
    {
        timeStunnedLeft = time;
        enemyRender.color = stunColor;
    }
    public void destunEnemy()
    {
        timeStunnedLeft = -1;
        enemyState = "default";
        enemyRender.color = originalColor;
    }
    //health
    public virtual void isDamaged(int damage)
    {
        grabArmor -= damage;
        health -= damage;
    }
    //updates depends on state
    public virtual void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                
                break; 
            case "stunned":

                break;
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
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
        stateUpdate(enemyState);
    }
}
