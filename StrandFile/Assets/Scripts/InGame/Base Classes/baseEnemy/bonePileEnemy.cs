using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonePileEnemy : baseEnemy
{
    // BONE PILE
    // punishes players for rushing into new rooms
    // activates and deactivates based on whether the player is in room
    // periodically fires a volley of projectiles in a wide arc
    // stays in place and cannot be grabbed
    // Has three states
    // --SHOOTING--
    // fires volley of shots for a periord of time then goes back to activated state
    // --ACTIVATED--
    // prepares to enter shooting phase and disconnects with player upon player leaving a set radius
    // --DEACTIVATED--
    // waits for player to stumble upon the grid and enters activated state once that happens
    //
    //--ANIMATION STATES--
    // "isActive"- Bool
    // determines if enemy is activate
    //
    // "EnemyState"- Int
    // 0 - idle
    // 1 - shooting 
    // 2 - stun
    // 3 - hurt
    // 4 - death
    //
    //--AUDIO STATES--
    // 3 - shoot state
    #region vars
    enum State
    {
        Shooting,
        Active,
        Deactive
    }
    State currentState = State.Deactive;
    //--Shooting Vars--
    [SerializeField]
    float fireTime;
    float fireTimeLeft;
    [SerializeField]
    int volleyAmount;
    [SerializeField]
    float volleyAngleAdjust;
    [SerializeField]
    GameObject projectilePrefab;
    [SerializeField]
    float timeUntilStopShoot;
    float timeLeftUntilStopShoot;
    //--Activated Vars--
    [SerializeField]
    float timeUntilStartShoot;
    [SerializeField]
    float timeUntilStartShootDayAddOn;
    float timeLeftUntilStartShoot;
    [SerializeField]
    float distanceUntilDisconnect;
    //--Deactivated Vars--
    [SerializeField]
    gridOverallLoader gridInfoGet;
    int currentXGrid = -69;
    int currentYGrid = -69;
    #endregion

    public override void Start()
    {
        base.Start();
        gridInfoGet = Camera.main.gameObject.GetComponent<gridOverallLoader>();
        timeUntilStartShoot = timeUntilStartShoot + timeUntilStartShootDayAddOn / (1 + PlayerPrefs.GetInt("daysSpent", 0));
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

    //Goes into pursuit mode after being damaged or stunned
    public override void isDamaged(int damage)
    {
        base.isDamaged(damage);
        getObjectAnimator().SetInteger("EnemyState", 3);
    }

    void transitionState(State givenState)
    {
        timeLeftUntilStopShoot = timeUntilStopShoot;
        timeLeftUntilStartShoot = timeUntilStartShoot;
        fireTimeLeft = fireTime;
        if(givenState == State.Shooting)
        {
            getCacheAudio().playSound(3, 1);
        }
        else
        {
            getCacheAudio().muteSound(1); 
        }
        currentState = givenState;
    }

    void fireProjectile(float givenAngle) {
        float newAngle = (givenAngle + 180) % 360;
        GameObject createdProjectile = Instantiate(projectilePrefab);
        createdProjectile.gameObject.transform.position = transform.position;
        createdProjectile.GetComponent<blockableProjectile>().setAngle(newAngle);
    }

    // decides what the flying head does frame to frame depending on state
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                // Actively waiting to start shooting again
                if (currentState == State.Active)
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    getObjectAnimator().SetBool("isActive", true);
                    float xDiffrence = gameObject.transform.position.x - getPlayerObject().transform.position.x;
                    float yDiffrence = gameObject.transform.position.y - getPlayerObject().transform.position.y;
                    float angleTowardsPlayer = Mathf.Atan2(yDiffrence, xDiffrence) * Mathf.Rad2Deg;
                    float distance = Mathf.Sqrt(xDiffrence * xDiffrence + yDiffrence * yDiffrence);
                    // disconnects from player due to distance
                    if (distance > distanceUntilDisconnect && gridInfoGet.getPlayerPositionX() != currentXGrid && gridInfoGet.getPlayerPositionY() != currentYGrid)
                    {
                        transitionState(State.Deactive);
                    }
                    timeLeftUntilStartShoot -= Time.deltaTime;
                    // times out and begins shoot phase
                    if (timeLeftUntilStartShoot < 0)
                    {
                        transitionState(State.Shooting);
                    }
                }
                // If searching for Player, not active
                else if (currentState == State.Deactive)
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    getObjectAnimator().SetBool("isActive", false);
                    getObjectRigidbody().velocity = Vector2.zero;
                    if (gridInfoGet.getPlayerPositionX() == currentXGrid && gridInfoGet.getPlayerPositionY() == currentYGrid)
                    {
                        transitionState(State.Active);
                    }
                }
                // If actively firing out volleys of shots
                else if (currentState == State.Shooting)
                {
                    getObjectAnimator().SetInteger("EnemyState", 1);
                    getObjectAnimator().SetBool("isActive", true);
                    fireTimeLeft -= Time.deltaTime;
                    // times out and fires a single volley of shots
                    if(fireTimeLeft < 0)
                    {
                        float xDiffrence = gameObject.transform.position.x - getPlayerObject().transform.position.x;
                        float yDiffrence = gameObject.transform.position.y - getPlayerObject().transform.position.y;
                        float angleTowardsPlayer = Mathf.Atan2(yDiffrence, xDiffrence) * Mathf.Rad2Deg;
                        fireProjectile(angleTowardsPlayer);
                        for(int i = 1; i < volleyAmount; i++)
                        {
                            fireProjectile(angleTowardsPlayer - volleyAngleAdjust * i);
                            fireProjectile(angleTowardsPlayer + volleyAngleAdjust * i);
                        }
                        fireTimeLeft = fireTime;
                    }
                    timeLeftUntilStopShoot -= Time.deltaTime;
                    if(timeLeftUntilStopShoot < 0)
                    {
                        transitionState(State.Active);
                    }
                }
                break;
            case "stunned":
                transitionState(State.Active);
                if (getObjectAnimator().GetInteger("EnemyState") != 3)
                    getObjectAnimator().SetInteger("EnemyState", 2);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 4);
                break;
        }
    }
}
