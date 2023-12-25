using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAudio : MonoBehaviour
{
    //Audio clips will follow volume of same index
    //Index follow following pattern
    //Index:
    //0- Hurt
    //1- Stun
    //2- Death
    //3 and onward- non standard class
    [SerializeField]
    AudioClip[] audioStates;
    [SerializeField]
    float[] volumes;
    //Cache
    [SerializeField]
    int amountOfSources;
    [SerializeField]
    AudioSource[] source;
    // Start is called before the first frame update
    void Start()
    {
        //Create amount of audio sources
        source = new AudioSource[amountOfSources];
        for(int i = 0; i < amountOfSources; i++)
        {
            source[i] = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        }
    }

    // For other functions to use to play audio clips
    public void playSound(int clipIndex, int sourceIndex)
    {
        AudioClip usedClip = null;
        float usedVolume = 0;
        if(clipIndex < audioStates.Length && sourceIndex < audioStates.Length)
        {
            usedClip = audioStates[clipIndex];
            usedVolume = volumes[clipIndex];
            source[sourceIndex].PlayOneShot(usedClip);
            source[sourceIndex].volume = usedVolume * ((float)AudioMixer.getMasterVolume() / (float)10) * ((float)AudioMixer.getSFXVolume() / (float)10);
        }
        else
        {
            if(clipIndex >= audioStates.Length)
            {
                print("ERROR- Enemy Audio was given too high of an clip index to play: " + clipIndex);
            }
            if(sourceIndex >= audioStates.Length)
            {
                print("ERROR- Enemy Audio was given too high of an source index to play: " + sourceIndex);
            }
        }
    }
    // To stop a sound at a specific index temporarily
    public void muteSound(int sourceIndex)
    {
        if (sourceIndex < audioStates.Length)
        {
            source[sourceIndex].volume = 0;
        }
        else
        {
            print("ERROR- Enemy Audio was given too high of an index to stop: " + sourceIndex);
        }
    }
}
