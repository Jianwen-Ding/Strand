using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomberScript : baseEnemy
{
    //BOMBER
    //Dealing with these enemies are high risk high reward
    //Very hard to grab and stun and explodes upon death
    //Rewards weakening at range before going in for the grab
    //Lunges consistently against 
    //-- ACTIVE STATE --
    //Casts projectiles in random directions that attempt to home on to the player in regular intervals
    //Moves in short bursts of speed, circles player constantly.
    //-- IDLE STATE --
    //When the player enters the grid that the flying head is also on, the flying head enters active state 
    //Animation states
    //0 - idle
    //1 - walking
    //2 - enraged run
    //3 - stunned
    //4 - hurt
    //5 - death

    //Audio States
    //3 - Heavy Step

    //Handles Pursuit States
    enum State
    {
        idle,
        walk,
        rage
    }
    State pursuitState = State.idle;
    [SerializeField]
    Vector2 playerAdjust;
    [SerializeField]
    Vector2 bomberAdjust;
    [SerializeField]
    float distanceCheck;
    [SerializeField]
    float distanceUntilDisconnect;

    //Handles Movement
    [SerializeField]
    float walkStepForce;
    [SerializeField]
    float daysPerWalkStepForceAdvance = 1000;
    [SerializeField]
    float timeUntilStepWalk;
    [SerializeField]
    float rageSteps;
    [SerializeField]
    int daysPerRageStepAdvance = 1000;
    float rageStepsLeft = 0;
    [SerializeField]
    float rageStepForce;
    [SerializeField]
    float timeUntilStepRage;
    float timeUntilStepLeft;

    //Handles Explosions
    [SerializeField]
    GameObject explosionPrefab;

    public override void Start()
    {
        base.Start();
        rageSteps = rageSteps + (int)Mathf.Floor(PlayerPrefs.GetInt("daysSpent", 0) / daysPerRageStepAdvance);
        walkStepForce = walkStepForce + (int)Mathf.Floor(PlayerPrefs.GetInt("daysSpent", 0) / daysPerWalkStepForceAdvance);
    }
    // Changes animation on damage
    public override void isDamaged(int damage)
    {
        base.isDamaged(damage);
        getObjectAnimator().SetInteger("EnemyState", 4);
    }

    // Destr
    public override void deathDestruct()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity.normalized);
        base.deathDestruct();
    }
    // decides what the bomber does frame to frame depending on state
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                //If searching for Player, not active
                if (pursuitState == State.idle)
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    timeUntilStepLeft = timeUntilStepRage;
                    getObjectRigidbody().velocity = Vector2.zero;
                    Vector2 diff = ((Vector2)getPlayerObject().transform.position + playerAdjust) - ((Vector2)gameObject.transform.position + bomberAdjust);
                    float angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    //Only allows default or player colliders to be hit
                    int layerMask = 1 << 0 | 1 << 6;
                    Vector2 directionVector = new Vector2(Mathf.Cos(angleTowardsPlayer), Mathf.Sin(angleTowardsPlayer));
                    RaycastHit2D checker = Physics2D.Raycast((Vector2)gameObject.transform.position + bomberAdjust, diff, distanceCheck, layerMask);
                    if (checker.collider == null)
                    {
                        Debug.DrawRay((Vector2)gameObject.transform.position + bomberAdjust, directionVector * distanceCheck, Color.green);
                    }
                    else
                    {
                        Debug.DrawLine((Vector2)gameObject.transform.position + bomberAdjust, checker.point, Color.cyan);
                    }
                    // print(checker.collider);
                    if (checker.collider != null && checker.collider.tag == "Player")
                    {
                        getObjectAnimator().SetInteger("EnemyState", 1);
                        pursuitState = State.walk;
                        timeUntilStepLeft = timeUntilStepWalk;
                    }
                }
                //In active pursuit
                else
                {
                    float xDiffrence = gameObject.transform.position.x - getPlayerObject().transform.position.x;
                    float yDiffrence = gameObject.transform.position.y - getPlayerObject().transform.position.y;
                    float angleTowardsPlayer = Mathf.Atan2(yDiffrence, xDiffrence) * Mathf.Rad2Deg;
                    float distance = Mathf.Sqrt(xDiffrence * xDiffrence + yDiffrence * yDiffrence);
                    timeUntilStepLeft -= Time.deltaTime;
                    // Burst movement
                    if (timeUntilStepLeft <= 0)
                    {
                        getRenderer().flipX = !(getPlayerObject().transform.position.x < gameObject.transform.position.x);
                        float circleAngle = 180 + angleTowardsPlayer;
                        Vector2 burstForce;
                        if (pursuitState == State.walk)
                        {
                            burstForce = new Vector2(Mathf.Cos(circleAngle * Mathf.Deg2Rad) * walkStepForce, Mathf.Sin(circleAngle * Mathf.Deg2Rad) * walkStepForce);
                            getObjectRigidbody().AddForce(burstForce, ForceMode2D.Impulse);
                            timeUntilStepLeft = timeUntilStepWalk;
                        }
                        if (pursuitState == State.rage)
                        {
                            burstForce = new Vector2(Mathf.Cos(circleAngle * Mathf.Deg2Rad) * rageStepForce, Mathf.Sin(circleAngle * Mathf.Deg2Rad) * rageStepForce);
                            getObjectRigidbody().AddForce(burstForce, ForceMode2D.Impulse);
                            timeUntilStepLeft = timeUntilStepRage;
                            rageStepsLeft--;
                            if(rageStepsLeft < 0)
                            {
                                pursuitState = State.walk;
                                timeUntilStepLeft = timeUntilStepWalk;
                            }
                        }
                        getCacheAudio().playSound(3, 0);
                    }
                    if (distance > distanceUntilDisconnect)
                    {
                        pursuitState = State.idle;
                    }
                    if (pursuitState == State.rage)
                    {
                        getObjectAnimator().SetInteger("EnemyState", 2);
                    }
                    if (pursuitState == State.walk)
                    {
                        getObjectAnimator().SetInteger("EnemyState", 1);
                    }
                }
                break;
            case "stunned":
                timeUntilStepLeft = timeUntilStepRage;
                rageStepsLeft = rageSteps;
                pursuitState = State.rage;
                if (getObjectAnimator().GetInteger("EnemyState") != 4)
                    getObjectAnimator().SetInteger("EnemyState", 3);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 5);
                break;
        }
    }
}
