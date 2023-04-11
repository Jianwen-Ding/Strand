using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    #region variables
    //Position Variables
    //float cameraMovePositionSpeed;
    //int xPosition;
    ////int yPosition;
    //bool initialized = false;
    [SerializeField]
    GameObject playerCharacter;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(playerCharacter.transform.position.x, playerCharacter.transform.position.y, -10);
        
    }
}
