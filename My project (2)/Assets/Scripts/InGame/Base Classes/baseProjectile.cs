using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseProjectile : MonoBehaviour
{
    // Cache variables
    Rigidbody2D objectPhysics;

    // Set variables
    [SerializeField]
    float travelSpeed;
    [SerializeField]
    float angle;
    [SerializeField]
    int playerDamage;
    [SerializeField]
    float playerPushBack;
    [SerializeField]
    float playerLock;

    // Get/set Function
    public float getAngle()
    {
        return angle;
    }

    public void setAngle(float setAngle)
    {
        angle = setAngle;
    }

    // Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collide(collision.gameObject);
    }

    // What happens when projectile hits an object whether through isTrigger or onCollision
    public virtual void collide(GameObject collideObject)
    {
        PlayerMainScript player = collideObject.GetComponent<PlayerMainScript>();
        Rigidbody2D collidePhysics = collideObject.GetComponent<Rigidbody2D>();
        if(player != null)
        {
            player.damagePlayer(playerDamage);
            player.lockMovement(playerLock);
            float xPush = Mathf.Cos(angle * Mathf.Deg2Rad) * playerPushBack;
            float yPush = Mathf.Sin(angle * Mathf.Deg2Rad) * playerPushBack;
            collidePhysics.velocity *= 0;
            collidePhysics.AddForce(new Vector2(xPush,  yPush), ForceMode2D.Impulse);
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        objectPhysics = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        float xVel = Mathf.Cos(angle * Mathf.Deg2Rad) * travelSpeed;
        float yVel = Mathf.Sin(angle * Mathf.Deg2Rad) * travelSpeed;
        objectPhysics.velocity = new Vector2(xVel ,yVel );
        transform.rotation = Quaternion.Euler(0, 0, angle + 180);
    }
}
