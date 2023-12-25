using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debugScript : MonoBehaviour
{
    [SerializeField]
    bool isSpedUp;
    [SerializeField]
    bool invincibleChar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpedUp)
        {
            Time.timeScale = 20;
        }
        else
        {
            Time.timeScale = 1;
        }
        if (invincibleChar)
        {
            FindObjectOfType<PlayerMainScript>().healPlayer(10);
        }
    }
}
