using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class singleTaskScript : MonoBehaviour
{
    string givenTask;
    enum taskState
    {
        Uncompleted,
        Completed,
        Fading
    }
    [SerializeField]
    taskState currentState;
    [SerializeField]
    float timeUntilFade;
    [SerializeField]
    float fadeSpeed;
    [SerializeField]
    float opacity = 1;
    taskListOverall taskControllerCache;
    Animator animationCache;
    Image spriteRenderCache;
    TMPro.TextMeshProUGUI textCache;
    bool isFading;
    
    public void setUp(string givenString, taskListOverall overallController)
    {
        currentState = taskState.Uncompleted;
        taskControllerCache = overallController;
        textCache = gameObject.GetComponent<TextMeshProUGUI>();
        spriteRenderCache = transform.GetChild(0).GetComponent<Image>();
        animationCache = transform.GetChild(0).GetComponent<Animator>();
        givenTask = givenString;
        textCache.text = "- " + givenString;
    }

    public void completeTask()
    {
        animationCache.SetBool("hasCompleted", true);
        if(currentState != taskState.Completed && currentState != taskState.Fading)
        {
            currentState = taskState.Completed; 
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(currentState == taskState.Completed)
        {
            timeUntilFade -= Time.deltaTime;
            if(timeUntilFade < 0)
            {
                currentState = taskState.Fading;
            }
        }
        else if (currentState == taskState.Fading)
        {
            Color currentSpriteColor = spriteRenderCache.color;
            Color currentTextColor = textCache.color;
            opacity -= Time.deltaTime * fadeSpeed;
            if(opacity <= 0)
            {
                spriteRenderCache.color = new Color(currentSpriteColor.r, currentSpriteColor.g, currentSpriteColor.b, 0);
                textCache.color = new Color(currentTextColor.r, currentTextColor.g, currentTextColor.b, 0);
                taskControllerCache.takeOffTask(this.gameObject);
            }
            else
            {
                spriteRenderCache.color = new Color(currentSpriteColor.r, currentSpriteColor.g, currentSpriteColor.b, opacity);
                textCache.color = new Color(currentTextColor.r, currentTextColor.g, currentTextColor.b, opacity);
            }
        }
    }
}
