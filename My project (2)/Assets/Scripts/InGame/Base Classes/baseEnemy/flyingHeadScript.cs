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
    //If too close to player begins to move in oppisite direction
    //When at a right distance from player attempt to orbit the player
    //When too far away from player return to idle state
    //-- IDLE STATE --
    //When the player enters the grid that the flying head is also on, the flying head enters active state 
    //Animation states
    //0 - patrol
    //1 - hunt
    //2 - stun
    //3 - hurt
    //4 - death
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
    float angleModifyCircle;
    [SerializeField]
    float timeUntilMove;
    [SerializeField]
    float timeLeftUntilMove;
    [SerializeField]
    float minDistanceFromPlayer;
    //Shoot
    [SerializeField]
    float timeUntilFire;
    [SerializeField]
    float timeLeftUntilFire;
    [SerializeField]
    float angleThreshold;
    [SerializeField]
    GameObject projectilePrefab;
    //--Idle State--
    [SerializeField]
    gridOverallLoader gridInfoGet;
    [SerializeField]
    int currentXGrid;
    [SerializeField]
    int currentYGrid;
    #endregion

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
        gridInfoGet = Camera.main.gameObject.GetComponent<gridOverallLoader>();
    }
    //Goes into pursuit mode after being damaged or stunned
    public override void isDamaged(int damage)
    {
        base.isDamaged(damage);
        getObjectAnimator().SetInteger("EnemyState", 3);
    }
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                //Actively attacking enemies
                if (isActive)
                {
                    float xDiffrence = gameObject.transform.position.x - getPlayerObject().transform.position.x;
                    float yDiffrence = gameObject.transform.position.y - getPlayerObject().transform.position.y;
                    float angleTowardsPlayer = Mathf.Atan2(yDiffrence, xDiffrence) * Mathf.Rad2Deg;
                    float distance = Mathf.Sqrt(xDiffrence * xDiffrence + xDiffrence);
                    timeLeftUntilFire -= Time.deltaTime;
                    timeLeftUntilMove -= Time.deltaTime;
                    // Burst movement
                    if(timeLeftUntilMove <= 0)
                    {
                        float circleAngle = angleModifyCircle + angleTowardsPlayer;
                        Vector2 burstForce = new Vector2(Mathf.Cos(circleAngle * Mathf.Deg2Rad), Mathf.Sin(circleAngle * Mathf.Deg2Rad));
                        getObjectRigidbody().AddForce(burstForce, ForceMode2D.Impulse);
                        timeLeftUntilMove = timeUntilMove;
                    }
                    //Fire projectile
                    if(timeLeftUntilFire <= 0)
                    {
                        float randomizedAngle = Random.Range(angleTowardsPlayer - angleThreshold, angleTowardsPlayer + angleThreshold);
                        GameObject createdProjectile = Instantiate(projectilePrefab);
                        timeLeftUntilFire = timeUntilFire;
                    }
                    if (distance > maxDistanceUntilDisconnect)
                    {
                        isActive = false;
                    }
                }
                //If searching for Player, not active
                else
                {
                    if(gridInfoGet.getPlayerPositionX() == currentXGrid && gridInfoGet.getPlayerPositionY() == currentYGrid)
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
