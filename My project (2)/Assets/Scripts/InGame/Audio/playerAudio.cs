using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAudio : MonoBehaviour
{
    //Sound effects and volumes, Volume is float between 0 and 1
    [SerializeField]
    AudioClip dashEffect;
    [SerializeField]
    float dashVolume;
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
    //Setup vars
    [SerializeField]
    AudioClip audioClipActiveSound;
    //Handles walking sound underlaying everything
    [SerializeField]
    AudioClip audioClipUnderlay;
    // Start is called before the first frame update
    void Start()
    {
        AudioClip[] gottenClips = gameObject.GetComponents<AudioClip>();
        if (gottenClips.Length >= 2)
        {
            audioClipActiveSound = gottenClips[0];
            audioClipUnderlay = gottenClips[1];
        }
        else
        {
            print("ERROR- not enough audioclips registered for -PLAYERAUDIO- script");
        }
    }
    public void setWalkingAudioState(bool setState)
    {

    }
    public void triggerActiveAudioState(string state)
    {
        switch (state)
        {
            default:
                print("ERROR- Audio state requested not found");
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
