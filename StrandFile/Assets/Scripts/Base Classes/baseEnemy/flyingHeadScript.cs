using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingHeadScript : baseEnemy
{
    //FLYING HEAD
    //Forces players to chase down these enemies
    //Either in idle or active state
    //Completely bypasses all walls
    //-- ACTIVE STATE --
    //Casts projectiles in random directions that attempt to home on to the player in regular intervals
    //Moves in short bursts of speed, circles player constantly.
    //-- IDLE STATE --
    //When the player enters the grid that the flying head is also on, the flying head enters active state 
    //Animation states
    //0 - patrol
    //1 - hunt
    //2 - stun
    //3 - hurt
    //4 - death


    //Audio States
    //3 - flap
    //4 - fire
    //5 - is held
    #region vars
    //--Active State--
    [SerializeField]
    bool isActive;
    [SerializeField]
    float maxDistanceUntilDisconnect;
    //Burst/Movement
    [SerializeField]
    float burstMovement;
    [SerializeField]
    float daysPerBurstForceAdvance = 1000;
    [SerializeField]
    float angleModifyCircle;
    [SerializeField]
    float timeUntilMove;
    float timeLeftUntilMove;
    //Shoot
    [SerializeField]
    float timeUntilFire;
    [SerializeField]
    float timeUntilFireDayAddOn;
    float timeLeftUntilFire;
    [SerializeField]
    float angleThreshold;
    [SerializeField]
    GameObject projectilePrefab;
    //--Idle State--
    [SerializeField]
    gridOverallLoader gridInfoGet;
    int currentXGrid;
    int currentYGrid;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onContact(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            singleGridLoadToggle getGrid = collision.GetComponent<singleGridLoadToggle>();
            currentXGrid = getGrid.getGridPositionX();
            currentYGrid = getGrid.getGridPositionY();
        }
    }
    public override void Start()
    {
        base.Start();
        timeUntilFire = timeUntilFire + timeUntilFireDayAddOn / (1 + PlayerPrefs.GetInt("daysSpent", 0));
        burstMovement = burstMovement + PlayerPrefs.GetInt("daysSpent", 0) / daysPerBurstForceAdvance;
        gridInfoGet = Camera.main.gameObject.GetComponent<gridOverallLoader>();
    }
    //Goes into pursuit mode after being damaged or stunned
    public override void isDamaged(int damage)
    {
        base.isDamaged(damage);
        getObjectAnimator().SetInteger("EnemyState", 3);
        isActive = true;    
    }

    // decides what the flying head does frame to frame depending on state
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                //Actively attacking enemies
                if (isActive)
                {
                    getObjectAnimator().SetInteger("EnemyState", 1);
                    float xDiffrence = gameObject.transform.position.x - getPlayerObject().transform.position.x;
                    float yDiffrence = gameObject.transform.position.y - getPlayerObject().transform.position.y;
                    float angleTowardsPlayer = Mathf.Atan2(yDiffrence, xDiffrence) * Mathf.Rad2Deg;
                    float distance = Mathf.Sqrt(xDiffrence * xDiffrence + yDiffrence * yDiffrence);
                    timeLeftUntilFire -= Time.deltaTime;
                    timeLeftUntilMove -= Time.deltaTime;
                    // Burst movement
                    if(timeLeftUntilMove <= 0)
                    {
                        float circleAngle = angleModifyCircle + angleTowardsPlayer;
                        Vector2 burstForce = new Vector2(Mathf.Cos(circleAngle * Mathf.Deg2Rad) * burstMovement, Mathf.Sin(circleAngle * Mathf.Deg2Rad) * burstMovement);
                        getObjectRigidbody().AddForce(burstForce, ForceMode2D.Impulse);
                        timeLeftUntilMove = timeUntilMove;
                        getCacheAudio().playSound(3, 0);
                    }
                    //Fire projectile
                    if(timeLeftUntilFire <= 0)
                    {
                        float randomizedAngle = Mathf.Abs(Random.Range(angleTowardsPlayer - angleThreshold, angleTowardsPlayer + angleThreshold) + 180) % 360;
                        GameObject createdProjectile = Instantiate(projectilePrefab);
                        createdProjectile.gameObject.transform.position = transform.position;
                        createdProjectile.GetComponent<homingProjectile>().setAngle(randomizedAngle);
                        timeLeftUntilFire = timeUntilFire;
                        getCacheAudio().playSound(4, 0);
                    }
                    if (distance > maxDistanceUntilDisconnect)
                    {
                        timeLeftUntilMove = timeUntilMove;
                        timeLeftUntilFire = timeUntilFire;
                        isActive = false;
                    }
                }
                //If searching for Player, not active
                else
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    getObjectRigidbody().velocity = Vector2.zero;
                    if (gridInfoGet.getPlayerPositionX() == currentXGrid && gridInfoGet.getPlayerPositionY() == currentYGrid)
                    {

                        isActive = true;
                    }
                }
                break;
            case "stunned":
                if (getObjectAnimator().GetInteger("EnemyState") != 3)
                    getObjectAnimator().SetInteger("EnemyState", 2);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 4);
                break;
        }
    }
}
