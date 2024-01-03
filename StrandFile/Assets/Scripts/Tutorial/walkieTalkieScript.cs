using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class walkieTalkieScript : MonoBehaviour
{
    // Cache Vars
    Image objectRenderer;
    Animator objectAnimator;
    Image talkieBubble;
    talkieBacklog talkieTextBackLog;
    TextMeshProUGUI talkieText;
    // Walkie States
    // 0- walkie idle
    // 1- walkie rise
    // 2- walkie call
    // 3- walkie drop
    [SerializeField]
    int walkieState = 0;
    [SerializeField]
    bool walkieInRange = false;
    [SerializeField]
    float timeRise;
    [SerializeField]
    float timeDrop;
    [SerializeField]
    float timeUntilIdleCut;
    float timeStateLeft;

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        objectRenderer = gameObject.GetComponent<Image>();
        objectAnimator = gameObject.GetComponent<Animator>();
        talkieBubble = transform.GetChild(0).gameObject.GetComponent<Image>();
        talkieTextBackLog = transform.GetChild(0).GetChild(0).gameObject.GetComponent<talkieBacklog>();
        talkieText = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }


    // Update is called once per frame
    void Update()
    {
        timeStateLeft -= Time.deltaTime;
        switch (walkieState)
        {
            case 0:
                if(timeStateLeft <= 0)
                {
                    objectRenderer.enabled = false;
                    objectAnimator.enabled = false;
                }
                if (!talkieTextBackLog.getDeactivated())
                {
                    objectAnimator.SetInteger("talkieState",1);
                    timeStateLeft = timeRise;
                    objectRenderer.enabled = true;
                    objectAnimator.enabled = true;
                    walkieState = 1;
                }
                break;
            case 1:
                if (timeStateLeft <= 0)
                {
                    objectAnimator.SetInteger("talkieState", 2);
                    walkieState = 2;
                    talkieBubble.enabled = true;
                    talkieText.enabled = true;
                }
                break;
            case 2:
                if (talkieTextBackLog.getDeactivated())
                {
                    walkieState = 3;
                    timeStateLeft = timeDrop;
                    objectAnimator.SetInteger("talkieState", 3);
                    talkieBubble.enabled = false;
                    talkieText.enabled = false;
                }
                break;
            case 3:
                if (timeStateLeft <= 0)
                {
                    objectAnimator.SetInteger("talkieState", 0);
                    walkieState = 0;
                    timeStateLeft = timeUntilIdleCut;
                }
                break;
        }
    }
}
