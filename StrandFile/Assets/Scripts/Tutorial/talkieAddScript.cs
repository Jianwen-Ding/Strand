using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkieAddScript : MonoBehaviour
{
    [SerializeField]
    string addText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject.FindAnyObjectByType<talkieBacklog>().addToTalk(addText);
        }
    }
}
