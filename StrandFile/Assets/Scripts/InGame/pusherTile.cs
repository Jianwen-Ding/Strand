using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pusherTile : MonoBehaviour
{
    // Sates
    // 0- Down Idle
    // 1- Down Prep
    // 2- Rise
    // 3- Up Idle
    // 4- Up Prep
    // 5- Drop
    [SerializeField]
    int currentState = 0;
    [SerializeField]
    float[] stateTimes = new float[6];
    [SerializeField]
    float currentStateTime;
    [SerializeField]
    float pushoutForce;
    [SerializeField]
    float playerStun;
    [SerializeField]
    float enemyStun;
    [SerializeField]
    string downAdjust;
    [SerializeField]
    int downLayer;
    [SerializeField]
    string upAdjust;
    [SerializeField]
    int upLayer;
    List<GameObject> insideObjects = new List<GameObject>();
    // cache vars
    Animator objectAnimator;
    Collider2D getCollider;
    SpriteYLayering objectLayering;
    SpriteRenderer objectRenderer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null && !insideObjects.Contains(collision.gameObject))
        {
            Rigidbody2D potentialRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            flyingHeadScript flyingHeadScript = collision.gameObject.GetComponent<flyingHeadScript>();
            stalkerEnemy stalkerScript = collision.gameObject.GetComponent<stalkerEnemy>();
            if (potentialRigidbody != null && flyingHeadScript == null && stalkerScript == null)
            {
                insideObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (insideObjects.Contains(collision.gameObject))
        {
            insideObjects.Remove(collision.gameObject);
        }
    }

    private void push(GameObject pushGameObject)
    {
        if(pushGameObject != null)
        {
            Rigidbody2D rigid = pushGameObject.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                //-pushes character back
                float Angle = Mathf.Rad2Deg * Mathf.Atan2(gameObject.transform.position.y - pushGameObject.transform.position.y, gameObject.transform.position.x - pushGameObject.gameObject.transform.position.x);
                //pushes back
                float xPush = Mathf.Cos(Angle * Mathf.Deg2Rad) * pushoutForce;
                float yPush = Mathf.Sin(Angle * Mathf.Deg2Rad) * pushoutForce;
                rigid.velocity *= 0;
                rigid.AddForce(new Vector2(-xPush, -yPush), ForceMode2D.Impulse);
                PlayerMainScript player = pushGameObject.GetComponent<PlayerMainScript>();
                baseEnemy enemy = pushGameObject.GetComponent<baseEnemy>();
                if (player != null)
                {
                    player.lockMovement(playerStun);
                }
                if (enemy != null && enemy.GetType() != typeof(flyingHeadScript) && enemy.GetType() != typeof(stalkerEnemy))
                {
                    enemy.stunEnemy(enemyStun);
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = gameObject.GetComponent<SpriteRenderer>();
        objectAnimator = gameObject.GetComponent<Animator>();
        getCollider = gameObject.GetComponent<Collider2D>();
        objectLayering = gameObject.GetComponent<SpriteYLayering>();
        currentStateTime = stateTimes[0];
    }

    // Update is called once per frame
    void Update()
    {
        currentStateTime -= Time.deltaTime;
        if(currentStateTime <= 0)
        {
            currentState++;
            switch (currentState)
            {
                case 2:
                    for(int i = 0; i < insideObjects.Count; i++)
                    {
                        push(insideObjects[i]);
                    }
                    insideObjects.Clear();
                    gameObject.layer = upLayer;
                    break;
                case 3:
                    objectRenderer.sortingLayerName = upAdjust;
                    getCollider.isTrigger = false;
                    break;
                case 6:
                    objectRenderer.sortingLayerName = downAdjust;
                    getCollider.isTrigger = true;
                    currentState = 0;
                    gameObject.layer = downLayer;
                    break;
            }
            objectAnimator.SetInteger("Next State", currentState);
            currentStateTime = stateTimes[currentState];
        }
        if(currentState > 1)
        {
            for (int i = 0; i < insideObjects.Count; i++)
            {
                push(insideObjects[i]);
            }
            insideObjects.Clear();
        }
    }
}
