using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inGameMenuState : MonoBehaviour
{
    //Do not use prefabs for these, to be dragged in
    [SerializeField]
    private GameObject mapDarkenScreen;
    [SerializeField]
    private GameObject miniMap;
    [SerializeField]
    private miniMapGenerator miniMapScript;
    [SerializeField]
    private GameObject pauseScreen;
    [SerializeField]
    private backGroundThemeSystem musicSource;
    [SerializeField]
    private bool pressedPause = false;
    //"paused" - stops time and pause state
    //"map" - darkens screen
    //"default"
    [SerializeField]
    private string currentState;
    // Start is called before the first frame update
    void Start()
    {
        musicSource = FindObjectOfType<backGroundThemeSystem>();
        miniMapScript = miniMap.GetComponent<miniMapGenerator>();
    }
    public void changeState(string newState)
    {
        if(newState != currentState)
        {
            if(currentState == "map")
            {
                mapDarkenScreen.SetActive(false);
                miniMapScript.deactivateOnMainDisplay();
            }
            if(currentState == "paused")
            {
                musicSource.pauseToggle(false);
                pauseScreen.SetActive(false);
                mapDarkenScreen.SetActive(false);
                Time.timeScale = 1;
            }
            if(newState == "paused")
            {
                musicSource.pauseToggle(true);
                pauseScreen.SetActive(true);
                mapDarkenScreen.SetActive(true);
                Time.timeScale = 0;
            }
            if(newState == "map")
            {
                musicSource.muffelToggle(true);
                mapDarkenScreen.SetActive(true);
                miniMapScript.activateOnMainDisplay();
            }
            if(newState == "default")
            {
                musicSource.muffelToggle(false);
            }
            currentState = newState;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(currentState == "default")
        {
            if(Input.GetAxis("mapButton") != 0)
            {
                changeState("map");
            }
        }
        if(currentState == "map")
        {
            if (Input.GetAxis("mapButton") == 0)
            {
                changeState("default");
            }
        }
        if (Input.GetKeyDown("escape"))
        {
            if(!pressedPause)
            {
                pressedPause = true;
                if (currentState == "paused")
                {
                    changeState("default");
                }
                else if(currentState == "default")
                {
                    changeState("paused");
                }
            }
        }
        else
        {
            pressedPause = false;
        }
    }
}
