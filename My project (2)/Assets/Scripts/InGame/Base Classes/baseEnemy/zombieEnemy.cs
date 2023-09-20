using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieEnemy : baseEnemy
{
    //THE ZOMBIE
    //The most basic enemy,
    //Walks towards players upon line of sight and does not stop walking in a straight line until player gets a certain distance away
    //Damages on contact and has no other special abilities
    //Zombies specific variables
    [SerializeField]
    private bool inPursuit;
    [SerializeField]
    private Vector2 playerAdjust;
    [SerializeField]
    private Vector2 zombieAdjust;
    [SerializeField]
    private float distanceCheckPlayer;
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
                if (!inPursuit)
                {
                    diff = ((Vector2)getPlayerObject().transform.position + playerAdjust) - ((Vector2)gameObject.transform.position + zombieAdjust);
                    angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    //Checks if enemy has line of sight with the play character
                    int layerMask = 1 << 0 | 1 << 6;
                    Vector2 directionVector = new Vector2(Mathf.Cos(angleTowardsPlayer), Mathf.Sin(angleTowardsPlayer));
                    RaycastHit2D checker = Physics2D.Raycast((Vector2)gameObject.transform.position + zombieAdjust, diff, distanceCheck, layerMask);
                    if (checker.collider == null)
                    {
                        Debug.DrawRay(gameObject.transform.position, directionVector * distanceCheck, Color.green);
                    }
                    else
                    {
                        Debug.DrawLine(gameObject.transform.position, checker.point, Color.cyan);
                    }
                    print(checker.collider);
                    if(checker.collider != null && checker.collider.tag == "Player")
                    {
                        inPursuit = true;
                    }
                }
                else
                {
                    diff = ((Vector2)getPlayerObject().transform.position) - ((Vector2)gameObject.transform.position);
                    angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                    Vector2 walkVelocity = new Vector2(walkSpeed * Mathf.Cos(angleTowardsPlayer),walkSpeed * Mathf.Sin(angleTowardsPlayer));
                    getObjectRigidbody().velocity = walkVelocity;
                    float distance = Mathf.Sqrt(diff.x*diff.x + diff.y+diff.y);
                    if(distance > distanceUntilDisconnect)
                    {
                        inPursuit = false;
                    }
                }
                break;
            case "stunned":
                break;
            case "death":
                break;
        }
    }
}
