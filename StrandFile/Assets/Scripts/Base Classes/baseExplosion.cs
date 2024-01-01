using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class baseExplosion : MonoBehaviour
{
    [SerializeField]
    float explosionTime;
    [SerializeField]
    int playerDamage;
    [SerializeField]
    float playerLock;
    [SerializeField]
    int enemyDamage;
    [SerializeField]
    float enemyLock;
    [SerializeField]
    float pushBack;
    List<GameObject> affectedObjects = new List<GameObject>();
    Collider2D explosionCollider;

    // Check if object has already been affected
    private bool checkIfAffected(GameObject checkObject)
    {
        return affectedObjects.Contains(checkObject);
    }

    // Plug in for collision
    private void collide(GameObject collideObject)
    {
        if (!checkIfAffected(collideObject))
        {
            affectedObjects.Add(collideObject);
            Rigidbody2D objectPhysics = collideObject.GetComponent<Rigidbody2D>();
            PlayerMainScript objectPlayerScript = collideObject.GetComponent<PlayerMainScript>();
            baseEnemy objectEnemyScript = collideObject.GetComponent<baseEnemy>();
            if(objectEnemyScript != null)
            {
                objectEnemyScript.isDamaged(enemyDamage);
                objectEnemyScript.stunEnemy(enemyLock);
            }
            if (objectPhysics != null)
            {
                Vector2 diff = ((Vector2)gameObject.transform.position - (Vector2)collideObject.transform.position);
                float angleTowardsObject = Mathf.Abs(Mathf.Rad2Deg * Mathf.Atan2(diff.y, diff.x) + 180) % 360;
                float xPush = Mathf.Cos(angleTowardsObject * Mathf.Deg2Rad) * pushBack;
                float yPush = Mathf.Sin(angleTowardsObject * Mathf.Deg2Rad) * pushBack;
                objectPhysics.velocity *= 0;
                objectPhysics.AddForce(new Vector2(xPush, yPush), ForceMode2D.Impulse);
            }
            if(objectPlayerScript != null)
            {
                objectPlayerScript.damagePlayer(playerDamage);
                objectPlayerScript.lockMovement(playerLock);
            }
        }
    }

    // Check if collision enter
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colObject = collision.gameObject;
        collide(colObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject colObject = collision.gameObject;
        collide(colObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        explosionCollider = gameObject.GetComponent<Collider2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        explosionTime -= Time.deltaTime;
        if(explosionTime < 0)
        {
            explosionCollider.enabled = false;
            this.enabled = false;
        }
    }
}
