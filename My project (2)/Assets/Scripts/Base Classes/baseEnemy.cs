using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// --THIS SCRIPT SERVES AS THE BASE CLASS FOR ALL ENEMIES--

public class baseEnemy : MonoBehaviour
{
    #region
    //Setup Variables
    private Rigidbody2D enemyRigid;
    //State
    [SerializeField]
    private string enemyState;
    //Health
    [SerializeField]
    private int health;
    [SerializeField]
    private float playerTouchPushbackOnPlayer;
    [SerializeField]
    private float playerTouchPushbackOnEnemy;
    [SerializeField]
    float playerTouchMovementLockTime;
    //--GRAB ARMOR--
    [SerializeField]
    float failedGrabPushBackEnemy;
    [SerializeField]
    float failedGrabPushBackPlayer;
    //grabArmor determines if an enemy can be grabbed or not
    [SerializeField]
    int grabArmor;
    //max grab armor, will default to after being succesfully grabbed
    [SerializeField]
    int defaultGrabArmor;
    #endregion
    //private functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMainScript colliderPlayerScript = collision.gameObject.GetComponent<PlayerMainScript>();
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
    //health
    public virtual void isDamaged(int damage)
    {
        grabArmor -= damage;
        health -= damage;
    }
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        enemyRigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }
}
