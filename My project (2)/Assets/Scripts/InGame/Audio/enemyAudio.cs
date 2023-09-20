using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAudio : MonoBehaviour
{
    //Audio clips will follow volume of same index
    //Index follow following pattern
    //Index:
    //0- Hurt
    //1- Damage
    //2- Stun
    //3- Grabbed
    //4- Failed Grab
    //5 and onward- non standard class
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
            print("ERROR- Enemy Audio was given too high of an index");
        }
    }
}
