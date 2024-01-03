using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogEnemy : baseEnemy
{
    //THE DOG
    //Patrols area and persitantly hunts down the player upon sight
    //Should be difficult to deal with upon sighting, countered by going out of sight lines.
    //Has two states, patrol and hunt
    //Patrol: go left and upon hitting object reverse direction. Upon player entering line of sight (depends on what direction the dog is facing) go into hunt.
    //Hunt: run swiftly towards player, upon player leaving sightlines, run towards last location player was seen. If player still is not in sightlines go back into patrol.
    //Dog has multiple animation states
    //0 - patrol
    //1 - hunt
    //2 - stun
    //3 - hurt
    //4 - death
    //If not hunting, the dog patrols
    [SerializeField]
    private bool inHunt;
    //For adjusting raycast targets.
    [SerializeField]
    private Vector2 playerAdjust;
    [SerializeField]
    private Vector2 zombieAdjust;
    //--Patrol Variables--
    //Distance dog raycast checks for players away
    [SerializeField]
    private float distanceCheck;
    //Distance dog raycast checks for barriers to change direction
    [SerializeField]
    private float barrierCheckDist;
    [SerializeField]
    private float barrierVerticleOffSet;
    //Speed dog walks back and forth on patrol
    [SerializeField]
    private float patrolSpeed;
    //Distance dog checks ahead for collisions
    [SerializeField]
    private bool facingLeft;
    //--Hunt Variables--
    [SerializeField]
    private bool playerInSights;
    //The location that the enemy will run towards upon leaving sightlines
    [SerializeField]
    private Vector2 playerLastSceenLoc;
    //The object that repersents the last seen location
    [SerializeField]
    private GameObject scentMarkerPrefab;
    [SerializeField]
    private float huntAccelerate;
    [SerializeField]
    float daysPerHuntAccelerateAdvance = 1000;
    //The cap on speed
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    float daysPerMaxSpeedAdvance = 1000;
    //When enemy is accelerating in oppisite direction as velocity it gains more acceleration
    [SerializeField]
    private float deAccelerateRatio;
    //Upon player leaving sightline, the dog has a certain amount of time to find the player again before returning to patrol state
    [SerializeField]
    private float timeUntiDisconnect;
    [SerializeField]
    private float timeUntilDisconnectLeft;

    public override void Start()
    {
        base.Start();
        huntAccelerate = huntAccelerate + PlayerPrefs.GetInt("daysSpent", 0) / daysPerHuntAccelerateAdvance;
        maxSpeed = maxSpeed + PlayerPrefs.GetInt("daysSpent", 0) / daysPerMaxSpeedAdvance;
    }

    public void facingLeftSet(bool set)
    {
        facingLeft = set;
    }

    //Goes into pursuit mode after being damaged or stunned
    public override void isDamaged(int damage)
    {
        if(gameObject != null)
        {
            base.isDamaged(damage);
            getObjectAnimator().SetInteger("EnemyState", 3);
            playerLastSceenLoc = (Vector2)getPlayerObject().transform.position;
            inHunt = true;
        }
    }

    public override void stunEnemy(float time)
    {
        base.stunEnemy(time);
        playerLastSceenLoc = (Vector2)getPlayerObject().transform.position;
        inHunt = true;
        playerInSights = true;
    }

    //Checks if player is in sightline
    public virtual bool checkSightLine(bool debugging)
    {
        Vector2 diff = ((Vector2)getPlayerObject().transform.position + playerAdjust) - ((Vector2)gameObject.transform.position + zombieAdjust);
        float angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
        //Only allows default or player colliders to be hit
        int layerMask = 1 << 0 | 1 << 6;
        Vector2 directionVector = new Vector2(Mathf.Cos(angleTowardsPlayer), Mathf.Sin(angleTowardsPlayer));
        RaycastHit2D checker = Physics2D.Raycast((Vector2)gameObject.transform.position + zombieAdjust, diff, distanceCheck, layerMask);
        if (debugging)
        {
            if (checker.collider == null)
            {
                Debug.DrawRay((Vector2)gameObject.transform.position + zombieAdjust, directionVector * distanceCheck, Color.green);
            }
            else
            {
                Debug.DrawLine((Vector2)gameObject.transform.position + zombieAdjust, checker.point, Color.cyan);
            }
            //print(checker.collider);
        }
        return (checker.collider != null && checker.collider.tag == "Player");
    }

    //Checks for nearby stationary objects 
    public virtual bool barrierCheckRayCast(Vector2 offset, Vector2 direction)
    {
        int layerMask = 1 << 0 | 1 << 6;
        RaycastHit2D checker = Physics2D.Raycast((Vector2)gameObject.transform.position + offset, direction, barrierCheckDist, layerMask);
        return checker.collider != null && checker.collider.GetComponent<Rigidbody2D>() == null;
    }

    //
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                //Zombies only have walking and not walking state, 0 and 1
                Vector2 diff;
                float angleTowardsPlayer;
              
                if (!inHunt)
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    //Checking if player is within line of sight, does not bother if player is on side dog is not looking
                    if ((getPlayerObject().transform.position.x < gameObject.transform.position.x) == facingLeft && checkSightLine(true))
                    {
                        playerInSights = true;
                        inHunt = true;
                    }
                    Vector2 patrolVelocity;
                    if (facingLeft)
                    {
                        patrolVelocity = new Vector2(-patrolSpeed , 0);
                    }
                    else
                    {
                        patrolVelocity = new Vector2(patrolSpeed, 0);
                    }
                    getObjectRigidbody().velocity = patrolVelocity;
                    getRenderer().flipX = facingLeft;
                    //bool leftTopCheck = barrierCheckRayCast(zombieAdjust + new Vector2(0, barrierVerticleOffSet), Vector2.left);
                    //bool leftMidCheck = barrierCheckRayCast(zombieAdjust, Vector2.left);
                    //bool leftBotCheck = barrierCheckRayCast(zombieAdjust - new Vector2(0, barrierVerticleOffSet), Vector2.left);
                    //if (leftTopCheck || leftMidCheck || leftBotCheck)
                    //{
                    //    facingLeft = false;
                    //}
                    //bool rightTopCheck = barrierCheckRayCast(zombieAdjust + new Vector2(0, barrierVerticleOffSet), Vector2.right);
                    //bool rightMidCheck = barrierCheckRayCast(zombieAdjust, Vector2.right);
                    //bool rightBotCheck = barrierCheckRayCast(zombieAdjust - new Vector2(0, barrierVerticleOffSet), Vector2.right);
                    //if (rightTopCheck || rightMidCheck || rightBotCheck)
                    //{
                    //    facingLeft = true;
                    //}
                }
                //Is in active hunt
                else
                {

                    if (playerInSights)
                    {
                        diff = ((Vector2)getPlayerObject().transform.position - (Vector2)gameObject.transform.position);
                        //If player falls out of sightline
                        if (!checkSightLine(true))
                        {
                            playerInSights = false;
                            timeUntilDisconnectLeft = timeUntiDisconnect;
                        }
                        playerLastSceenLoc = (Vector2)getPlayerObject().transform.position;
                    }
                    else {
                        diff = playerLastSceenLoc - (Vector2)gameObject.transform.position;
                        //If player falls into sightline again
                        if (checkSightLine(true)) {
                            playerInSights = true;
                        }
                        timeUntilDisconnectLeft -= Time.deltaTime;
                        if(timeUntilDisconnectLeft < 0)
                        {
                            inHunt = false;
                        }
                    }

                    getObjectAnimator().SetInteger("EnemyState", 1);
                    angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    Vector2 accelerationVector = new Vector2(Mathf.Cos(angleTowardsPlayer) * huntAccelerate, Mathf.Sin(angleTowardsPlayer) * huntAccelerate);
                    Vector2 gotVelocity = getObjectRigidbody().velocity;
                    //Increases rate of deacceleration
                    if((accelerationVector.x >= 0) == (gotVelocity.x >= 0))
                    {
                        accelerationVector = new Vector2(accelerationVector.x * deAccelerateRatio, accelerationVector.y);
                    }
                    else if((accelerationVector.y >= 0) == (gotVelocity.y >= 0))
                    {
                        accelerationVector = new Vector2(accelerationVector.x, accelerationVector.y * deAccelerateRatio);
                    }
                    getObjectRigidbody().AddForce(accelerationVector * Time.deltaTime, ForceMode2D.Impulse);
                    //Caps player speed at point
                    float velocityAmplitude = Mathf.Sqrt(gotVelocity.x * gotVelocity.x + gotVelocity.y + gotVelocity.y);
                    if(velocityAmplitude > maxSpeed)
                    {
                        float angleVel = Mathf.Atan2(gotVelocity.y, gotVelocity.x);
                        getObjectRigidbody().velocity = new Vector2(Mathf.Cos(angleVel) * maxSpeed, Mathf.Sin(angleVel) * maxSpeed);
                    }
                    float distance = Mathf.Sqrt(diff.x * diff.x + diff.y + diff.y);
                    //Flips enemy on x axis upon the player crossing enemy
                    getRenderer().flipX = (getPlayerObject().transform.position.x < gameObject.transform.position.x);
                }
                break;
            case "stunned":
                if(getObjectAnimator().GetInteger("EnemyState") != 3)
                    getObjectAnimator().SetInteger("EnemyState", 2);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 4);
                break;
        }
    }
}
