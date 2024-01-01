using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script scales the audio of the attached audio source around master volume, sfx volume, and music volume depending on certain variables
public class AudioScaler : MonoBehaviour
{
    //Either SFX or Music, determines if scaled with master and music or master and sfx volume settings. True is Music. False is SFX.
    [SerializeField]
   bool isNotSFXandIsMusic;
    //Volume when all settings are turned to the max
    [SerializeField]
    float baseVolume;
    //The object's audio source
    [SerializeField]
    AudioSource cacheAudio;
    // Start is called before the first frame update
    void Start()
    {
        //caches component
        cacheAudio = gameObject.GetComponent<AudioSource>();
    }

    public void setVolume(float set)
    {
        baseVolume = set;
    }
    // Update is called once per frame
    void Update()
    {
        if (isNotSFXandIsMusic)
        {
            cacheAudio.volume = baseVolume * ((float)AudioMixer.getMasterVolume() / (float)10) * ((float)AudioMixer.getMusicVolume() / (float)10);
        }
        else
        {
            cacheAudio.volume = baseVolume * ((float)AudioMixer.getMasterVolume() / (float)10) * ((float)AudioMixer.getSFXVolume() / (float)10);
        }
    }
}
