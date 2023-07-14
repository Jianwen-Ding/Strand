using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudio : MonoBehaviour
{
    //Sound effects and volumes, Volume is float between 0 and 1
    [SerializeField]
    AudioClip lowHealthEffect;
    [SerializeField]
    float lowHealthVolume;
    [SerializeField]
    AudioClip walkEffect;
    [SerializeField]
    float walkVolume;
    [SerializeField]
    AudioClip deathEffect;
    [SerializeField]
    float deathVolume;
    [SerializeField]
    AudioClip throwEffect;
    [SerializeField]
    float throwVolume;
    [SerializeField]
    AudioClip standingGrabEffect;
    [SerializeField]
    float standingGrabVolume;
    [SerializeField]
    AudioClip lungeGrabEffect;
    [SerializeField]
    float lungeGrabVolume;
    [SerializeField]
    AudioClip hurtEffect;
    [SerializeField]
    float hurtVolume;
    [SerializeField]
    AudioClip pickUpEffect;
    [SerializeField]
    float pickUpVolume;
    //changing vars
    [SerializeField]
    float walkPausedLeft;
    [SerializeField]
    bool lowHealth;
    [SerializeField]
    bool isWalking;
    //Setup vars
    [SerializeField]
    AudioSource audioActiveSound;
    //Handles walking sound underlaying everything
    [SerializeField]
    AudioSource audioUnderlayWalk;
    //Handles low health sound effect underlaying everything
    [SerializeField]
    AudioSource audioUnderlayLowHealthAlert;
    // Start is called before the first frame update
    void Start()
    {
        walkPausedLeft = 0;
        AudioSource[] gottenClips = gameObject.GetComponents<AudioSource>();
        if (gottenClips.Length >= 3)
        {
            audioActiveSound = gottenClips[0];
            audioUnderlayWalk = gottenClips[1];
            audioUnderlayLowHealthAlert = gottenClips[2];
            audioUnderlayWalk.clip = walkEffect;
            audioUnderlayWalk.volume = walkVolume;
            audioUnderlayLowHealthAlert.clip = lowHealthEffect;
            audioUnderlayLowHealthAlert.volume = lowHealthVolume;
        }
        else
        {
            print("ERROR- not enough audioclips registered for -PLAYERAUDIO- script");
        }
    }
    public void setWalkingAudioState(bool setState)
    {
        if(setState != isWalking && walkPausedLeft <= 0)
        {
            if (setState)
            {
                audioUnderlayWalk.time = 0;
                audioUnderlayWalk.UnPause();
                audioUnderlayWalk.Play();
            }
            else
            {
                audioUnderlayWalk.Pause();
            }
            isWalking = setState;
        }
    }
    public void pauseWalking(float time)
    {
        setWalkingAudioState(false);
        walkPausedLeft = time;
    }
    public void setHealthAudioState(bool setState)
    {
        if (setState != lowHealth)
        {
            if (setState)
            {
                audioUnderlayLowHealthAlert.time = 0;
                audioUnderlayLowHealthAlert.UnPause();
                audioUnderlayLowHealthAlert.Play();
            }
            else
            {
                audioUnderlayLowHealthAlert.Pause();
            }
            lowHealth = setState;
        }
    }
    //States include "death", "hurt", "throw", "standGrab", "lungeGrab"
    public void triggerActiveAudioState(string state)
    {
        AudioClip usedClip = null;
        float usedVolume = 0;
        switch (state)
        {
            case "death":
                usedClip = deathEffect;
                usedVolume = deathVolume;
                break;
            case "hurt":
                usedClip = hurtEffect;
                usedVolume = hurtVolume;
                break;
            case "throw":
                usedClip = throwEffect;
                usedVolume = throwVolume;
                break;
            case "pickUp":
                usedClip = standingGrabEffect;
                usedVolume = standingGrabVolume;
                break;
            case "standGrab":
                usedClip = standingGrabEffect;
                usedVolume = standingGrabVolume;
                break;
            case "lungeGrab":
                usedClip = lungeGrabEffect;
                usedVolume = lungeGrabVolume;
                break;
            default:
                print("ERROR- Audio state requested not found");
                break;
        }
        audioActiveSound.PlayOneShot(usedClip);
        audioActiveSound.volume = usedVolume;
    }
    // Update is called once per frame
    void Update()
    {
        if(walkPausedLeft > 0)
        {
            walkPausedLeft -= Time.deltaTime;
        }
    }
}
