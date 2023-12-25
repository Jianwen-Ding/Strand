using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stalkerEnemy : baseEnemy
{
    //STALKER
    //Endgame collapse enemy that only comes at night
    //Causes all tiles it passes over to become stalker infested
    //Completely bypasses all walls
    //Moves at a constant rate towards the player, does not ever despawn
    //Can't be hurt

    //Animation state
    //0 - move

    [SerializeField]
    float movementSpeed;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        onContact(collision.gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            singleGridLoadToggle getGrid = collision.GetComponent<singleGridLoadToggle>();
            getGrid.setStalkerInfected(true);
        }
    }
    public override void stateUpdate(string insertedState)
    {
        switch (insertedState)
        {
            case "default":
                Vector2 diff;
                float angleTowardsPlayer;
                getObjectAnimator().SetInteger("EnemyState", 0);
                diff = ((Vector2)getPlayerObject().transform.position) - ((Vector2)gameObject.transform.position);
                angleTowardsPlayer = Mathf.Atan2(diff.y, diff.x);
                Vector2 walkVelocity = new Vector2(movementSpeed * Mathf.Cos(angleTowardsPlayer), movementSpeed * Mathf.Sin(angleTowardsPlayer));
                getObjectRigidbody().velocity = walkVelocity;
                //Flips enemy on x axis upon the player crossing enemy
                getRenderer().flipX = (getPlayerObject().transform.position.x < gameObject.transform.position.x);
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
