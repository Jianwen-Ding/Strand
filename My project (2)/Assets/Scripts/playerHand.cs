using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHand : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject;
    [SerializeField]
    private PlayerMainScript objectPlayerScript;
    [SerializeField]
    private SpriteRenderer objectPlayerRenderScript;
    [SerializeField]
    private SpriteRenderer objectRenderScript;
    [SerializeField]
    private float yAddition;
    [SerializeField]
    private float yRange;
    [SerializeField]
    private float xRange;
    // Start is called before the first frame update
    void Start()
    {
        playerObject = gameObject.transform.parent.gameObject;
        objectPlayerScript = playerObject.GetComponent<PlayerMainScript>();
        objectPlayerRenderScript = playerObject.GetComponent<SpriteRenderer>();
        objectRenderScript = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(objectPlayerScript.getAngleFace() >= 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder -  1;
        }
        if (objectPlayerScript.getAngleFace() < 0)
        {
            objectRenderScript.sortingOrder = objectPlayerRenderScript.sortingOrder + 1;
        }
        //uses polar equation for ellispes in order to create effect
        float radius = (xRange*yRange)/Mathf.Sqrt(Mathf.Pow(yRange * Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2) + Mathf.Pow(xRange * Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad), 2));
        gameObject.transform.position = new Vector3(Mathf.Cos(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.x , Mathf.Sin(objectPlayerScript.getAngleFace() * Mathf.Deg2Rad) * radius + playerObject.transform.position.y + yAddition);
    }
}
