using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieEnemy : baseEnemy
{
    //THE ZOMBIE
    //The most basic enemy, not a major threat
    //Walks towards players upon line of sight and does not stop walking in a straight line until player gets a certain distance away
    //Damages on contact and has no other special abilities
    //Zombies only have walking and not walking state, 0 and 1 animation states
    //Zombies specific variables
    [SerializeField]
    private bool inPursuit;
    [SerializeField]
    private Vector2 playerAdjust;
    [SerializeField]
    private Vector2 zombieAdjust;
    [SerializeField]
    private float distanceCheck;
    [SerializeField]
    private float distanceUntilDisconnect;
    [SerializeField]
    private float walkSpeed;
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                Vector2 diff;
                float angleTowardsPlayer;
                //Checking if player is within line of sight
                if (!inPursuit)
                {
                    getObjectAnimator().SetInteger("EnemyState", 0);
                    diff = ((Vector2)getPlayerObject().transform.position + playerAdjust) - ((Vector2)gameObject.transform.position + zombieAdjust);
                    angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    //Only allows default or player colliders to be hit
                    int layerMask = 1 << 0 | 1 << 6;
                    Vector2 directionVector = new Vector2(Mathf.Cos(angleTowardsPlayer), Mathf.Sin(angleTowardsPlayer));
                    RaycastHit2D checker = Physics2D.Raycast((Vector2)gameObject.transform.position + zombieAdjust, diff, distanceCheck, layerMask);
                    if (checker.collider == null)
                    {
                        Debug.DrawRay((Vector2)gameObject.transform.position + zombieAdjust, directionVector * distanceCheck, Color.green);
                    }
                    else
                    {
                        Debug.DrawLine((Vector2)gameObject.transform.position + zombieAdjust, checker.point, Color.cyan);
                    }
                    print(checker.collider);
                    if(checker.collider != null && checker.collider.tag == "Player")
                    {
                        inPursuit = true;
                    }
                }
                //Is in active pursuit
                else
                {
                    getObjectAnimator().SetInteger("EnemyState", 1);
                    diff = ((Vector2)getPlayerObject().transform.position) - ((Vector2)gameObject.transform.position);
                    angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    Vector2 walkVelocity = new Vector2(walkSpeed * Mathf.Cos(angleTowardsPlayer),walkSpeed * Mathf.Sin(angleTowardsPlayer));
                    getObjectRigidbody().velocity = walkVelocity;
                    float distance = Mathf.Sqrt(diff.x*diff.x + diff.y+diff.y);
                    //Player went too far away
                    if(distance > distanceUntilDisconnect)
                    {
                        inPursuit = false;
                    }
                    //Flips enemy on x axis upon the player crossing enemy
                    getRenderer().flipX = !(getPlayerObject().transform.position.x < gameObject.transform.position.x);
                }
                break;
            case "stunned":
                getObjectAnimator().SetInteger("EnemyState", 0);
                break;
            case "death":
                getObjectAnimator().SetInteger("EnemyState", 0);
                break;
        }
    }
}
