using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class talkieAddScript : MonoBehaviour
{
    [SerializeField]
    string addText;

    [SerializeField]
    float timeUntilRepeat;
    float timeLeft = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && timeLeft <= 0)
        {
            GameObject.FindAnyObjectByType<talkieBacklog>().addToTalk(addText);
            timeLeft = timeUntilRepeat;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && timeLeft <= 0)
        {
            GameObject.FindAnyObjectByType<talkieBacklog>().addToTalk(addText);
            timeLeft = timeUntilRepeat;
        }
    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
    }
}
