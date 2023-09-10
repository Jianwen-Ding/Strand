using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    //out of 10 automatically at 10
    //expressed volume = (volume for specific effect) * masterVolume * (sfxVolume or musicVolume)
    [SerializeField]
    static int masterVolume = -1;
    [SerializeField]
    static int musicVolume;
    [SerializeField]
    static int sfxVolume;
    [SerializeField]
    private int presentableMasterVol;
    [SerializeField]
    private int presentableMuiscVol;
    [SerializeField]
    private int presentableSFXVol;
    // Start is called before the first frame update
    void Start()
    {
        //Based on player prefs "masterVolume", "musicVolume", and "sfxVolume"
        if (masterVolume == -1)
        {
            masterVolume = 5;
            musicVolume = 5;
            sfxVolume = 5;
        }
    }
    //Get/set public 
    public static int getMasterVolume()
    {
        return masterVolume;
    }
    public static int getMusicVolume()
    {
        return musicVolume;
    }
    public static int getSFXVolume()
    {
        return sfxVolume;
    }
    public static void setMasterVolume(int setVol)
    {
        masterVolume = setVol;
    }
    public static void setMusicVolume(int setVol)
    {
        musicVolume = setVol;
    }
    public static void setSFXVolume(int setVol)
    {
        sfxVolume = setVol;
    }
    // Update is called once per frame
    void Update()
    {
        presentableMasterVol = masterVolume;
        presentableMuiscVol = musicVolume;
        presentableSFXVol = sfxVolume;
    }
}
