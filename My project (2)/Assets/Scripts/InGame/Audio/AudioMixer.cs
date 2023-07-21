using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour
{
    //out of 10 automatically at 10
    //expressed volume = (volume for specific effect) * masterVolume * (sfxVolume or musicVolume)
    static int masterVolume;
    static int musicVolume;
    static int sfxVolume;
    // Start is called before the first frame update
    void Start()
    {
        //Based on player prefs "masterVolume", "musicVolume", and "sfxVolume"
        if (-1 == PlayerPrefs.GetInt("musicVolume", -1))
        {
            masterVolume = 10;
            musicVolume = 10;
            sfxVolume = 10;
            PlayerPrefs.SetInt("musicVolume", 10);
            PlayerPrefs.SetInt("masterVolume", 10);
            PlayerPrefs.SetInt("sfxVolume", 10);
        }
        else
        {
            masterVolume = PlayerPrefs.GetInt("masterVolume", 10);
            musicVolume = PlayerPrefs.GetInt("musicVolume", 10);
            sfxVolume = PlayerPrefs.GetInt("sfxVolume", 10);
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
        PlayerPrefs.SetInt("masterVolume", setVol);
    }
    public static void setMusicVolume(int setVol)
    {
        musicVolume = setVol;
        PlayerPrefs.SetInt("musicVolume", setVol);
    }
    public static void setSFXVolume(int setVol)
    {
        sfxVolume = setVol;
        PlayerPrefs.SetInt("sfxVolume", setVol);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
