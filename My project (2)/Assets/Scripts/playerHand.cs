using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHand : MonoBehaviour
{
    #region variables
    //Setup Objects
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private PlayerMainScript objectPlayerScript;
    [SerializeField]
    private SpriteRenderer objectPlayerRenderScript;
    [SerializeField]
    private SpriteRenderer objectRenderScript;
    //Orbit variables
    [SerializeField]
    private float yAddition;
    [SerializeField]
    private float yRange;
    [SerializeField]
    private float xRange;
    //Grab variables
    [SerializeField]
    private List<GameObject> objectsInRange;
    //states
    //slashing
    //grabbing
    //grabbed
    //none
    [SerializeField]
    private string grabState;
    [SerializeField]
    private GameObject objectGrabbed;
    [SerializeField]
    private grabbableObject grabbedScript;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        playerObject = gameObject.transform.parent.gameObject;
        objectPlayerScript = playerObject.GetComponent<PlayerMainScript>();
        objectPlayerRenderScript = playerObject.GetComponent<SpriteRenderer>();
        objectRenderScript = gameObject.GetComponent<SpriteRenderer>();
    }
    //private functions
    private void grabObject(GameObject insertObject)
    {
        grabbedScript = insertObject.GetComponent<grabbableObject>();
        if(grabbedScript != null)
        {
            grabState = "grabbed";
            objectRenderScript.enabled = false;
            objectGrabbed = insertObject;
            objectPlayerScript.endGrab();
            grabbedScript.grabbedEffect(gameObject);
        }
        else
        {
            print("ERROR- object grabbed by " + gameObject.name + " does not have require -grabbableObject- script");
        }
    }
    private void releaseObject(GameObject insertObject)
    {
        if (grabbedScript != null)
        {
            objectRenderScript.enabled = true;
            grabState = "none";
            objectGrabbed = null;
            grabbedScript = null;
        }
        else
        {
            print("ERROR- object being ungrabbed by " + gameObject.name + " does not have require -grabbableObject- script");
        }
    }
    //public functions
    public void attemptGrab()
    {
        bool attemptSuccesful = false;
        for (int i = 0; i < objectsInRange.Count; i++)
        {
            if (objectsInRange[i].GetComponent<grabbableObject>() != null)
            {
                attemptSuccesful = true;
                grabObject(objectsInRange[i]);
                break;
            }
        }
        if (!attemptSuccesful)
        {
            grabState = "grabbing";
        }
    }
    public void stopGrabAttempt()
    {
        if(grabState == "grabbing"){
            grabState = "none";
        }
    }
    public void attemptSlashGrab()
    {

    }
    public void stopAttemptSlash()
    {

    }
    public void attemptThrow(float strength, float angle)
    {
        if (grabbedScript != null)
        {
            objectRenderScript.enabled = true;
            grabState = "none";
            grabbedScript.throwEffect(strength, angle);
            objectGrabbed = null;
            grabbedScript = null;
        }
        else
        {
            print("ERROR- object being ungrabbed by " + gameObject.name + " does not have require -grabbableObject- script");
        }
    }
    public string getGrabState()
    {
        return grabState;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        objectsInRange.Add(collision.gameObject);
        if(grabState == "grabbing")
        {
            if (collision.gameObject.GetComponent<grabbableObject>())
            {
                grabObject(collision.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        objectsInRange.Remove(collision.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if(objectPlayerScript.getAngleFace() >= 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder -  15;
        }
        if (objectPlayerScript.getAngleFace() < 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder + 15;
        }
        //uses polar equation for ellispes in order to create effect
        float radius = (xRange*yRange)/Mathf.Sqrt(Mathf.Pow(yRange * Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2) + Mathf.Pow(xRange * Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2));
        gameObject.transform.position = new Vector3(Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.x , Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.y + yAddition);
    }
}
