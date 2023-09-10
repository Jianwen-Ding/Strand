using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGroundThemeSystem : MonoBehaviour
{
    //audio clip presets
    [SerializeField]
    AudioClip pauseTheme;
    [SerializeField]
    float pauseVolume;
    [SerializeField]
    AudioClip dawnTheme;
    [SerializeField]
    float dawnVolume;
    [SerializeField]
    AudioClip dawnThemeMuffled;
    [SerializeField]
    float dawnMuffledVolume;
    [SerializeField]
    AudioClip middayTheme;
    [SerializeField]
    float middayVolume;
    [SerializeField]
    AudioClip middayThemeMuffled;
    [SerializeField]
    float middayMuffledVolume;
    [SerializeField]
    AudioClip duskTheme;
    [SerializeField]
    float duskVolume;
    [SerializeField]
    AudioClip duskThemeMuffled;
    [SerializeField]
    float duskMuffledVolume;
    [SerializeField]
    AudioClip nightTheme;
    [SerializeField]
    float nightVolume;
    [SerializeField]
    AudioClip nightThemeMuffled;
    [SerializeField]
    float nightmMuffledVolume;
    [SerializeField]
    AudioClip transitionBell;
    [SerializeField]
    float bellVolume;
    [SerializeField]
    float bellMuffledVolume;
    //variables
    //to swap between muffled audio clips and non muffled ones
    [SerializeField]
    bool isMuffled;
    //state starts from 0 and goes up as each song ends
    //it goes from
    //0 - dawn
    //1 - midday
    //2 - dusk
    //3 - night
    //-1 (only for playThemeFromIndex function) - bell
    [SerializeField]
    int dayState;
    //for checks if bell is ringing
    [SerializeField]
    bool isBellRing;
    //manages changes into next state
    [SerializeField]
    float timeLeftUntilNextState;
    //the index of the final daystate
    [SerializeField]
    const int finalState = 3;
    //time of state when paused
    [SerializeField]
    float pausedTime;
    //checks if game has been paused
    [SerializeField]
    bool hasBeenPaused;
    //Cache variable
    [SerializeField]
    AudioSource soundSource;
    //Volume for each sound effect before editing by volume controls
    [SerializeField]
    float baseVolume;
    //Volume after considering volume controls
    [SerializeField]
    float expressedVolume;
    // Start is called before the first frame update
    void Awake()
    {
        nightSystem.setTimeUntilNight(getTotalLength());
        soundSource = gameObject.GetComponent<AudioSource>();
        timeLeftUntilNextState = dawnTheme.length;
        playThemeFromIndex(0, false);
        dayState = 0;
        isMuffled = false;
        isBellRing = false;
    } 
    //get length of songs until night state
    public float getTotalLength()
    {
        return dawnTheme.length + middayTheme.length + duskTheme.length + transitionBell.length * 3;
    }
    //plays indicated audio clip with indicated volume while returning indicated audio clip
    public AudioClip playThemeFromIndex(int indexSet, bool muffleSet)
    {
        AudioClip returnTheme;
        float returnVolume;
        if (muffleSet)
        {
            switch (indexSet)
            {
                case -1:
                    returnTheme = transitionBell;
                    returnVolume = bellMuffledVolume;
                    break;
                case 0:
                    returnTheme = dawnThemeMuffled;
                    returnVolume = dawnMuffledVolume;
                    break;
                case 1:
                    returnTheme = middayThemeMuffled;
                    returnVolume = middayMuffledVolume;
                    break;
                case 2:
                    returnTheme = duskThemeMuffled;
                    returnVolume = duskMuffledVolume;
                    break;
                case 3:
                    returnTheme = nightThemeMuffled;
                    returnVolume = nightmMuffledVolume;
                    break;
                default:
                    returnTheme = null;
                    returnVolume = 0;
                    print("ERROR- index given goes past parameters");
                    break;
            }
        }
        else
        {
            switch (indexSet)
            {
                case -1:
                    returnTheme = transitionBell;
                    returnVolume = bellVolume;
                    break;
                case 0:
                    returnTheme = dawnTheme;
                    returnVolume = dawnVolume;
                    break;
                case 1:
                    returnTheme = middayTheme;
                    returnVolume = middayVolume;
                    break;
                case 2:
                    returnTheme = duskTheme;
                    returnVolume = duskVolume;
                    break;
                case 3:
                    returnTheme = nightTheme;
                    returnVolume = nightVolume;
                    break;
                default:
                    returnTheme = null;
                    returnVolume = 0;
                    print("ERROR- index given goes past parameters");
                    break;
            }
        }
        soundSource.Stop();
        soundSource.time = 0;
        //volume adjusted by overall mixer ratios out of ten
        baseVolume = returnVolume;
        soundSource.clip = returnTheme;
        soundSource.Play();
        return returnTheme;
    }
    //Toggles wether to switch to unmuffled or muffled 
    public void muffelToggle(bool toggleSet)
    {
        if(toggleSet != isMuffled)
        {
            float currentPlayBackTime = soundSource.time;
            if (isBellRing)
            {
                playThemeFromIndex(-1, toggleSet);
            }
            else
            {
                playThemeFromIndex(dayState, toggleSet);
            }
            soundSource.time = currentPlayBackTime;
            isMuffled = toggleSet;
        }
    }
    public void pauseToggle(bool toggleSet)
    {
        if (hasBeenPaused != toggleSet)
        {
            //is being paused
            if (toggleSet)
            {
                pausedTime = soundSource.time;
                soundSource.Stop();
                soundSource.time = 0;
                baseVolume = pauseVolume;
                soundSource.clip = pauseTheme;
                soundSource.Play();
                soundSource.loop = true;
            }
            //is being unpaused
            else
            {
                soundSource.loop = false;
                if (isBellRing)
                {
                    playThemeFromIndex(-1, isMuffled);
                }
                else
                {
                    playThemeFromIndex(dayState, isMuffled);
                }
                soundSource.time = pausedTime;
            }
            hasBeenPaused = toggleSet;
        }
    }
    // Update is called once per frame
    void Update()
    {
        expressedVolume = baseVolume * ((float)AudioMixer.getMasterVolume() / (float)10) * ((float)AudioMixer.getMusicVolume() / (float)10);
        soundSource.volume = expressedVolume;
        if (timeLeftUntilNextState <= 0)
        {
            if (isBellRing || dayState >= finalState)
            {
                if (dayState < finalState)
                {
                    isBellRing = false;
                    dayState++;
                }
                timeLeftUntilNextState = playThemeFromIndex(dayState, isMuffled).length;

            }
            else
            {
                isBellRing = true;
                timeLeftUntilNextState = playThemeFromIndex(-1, isMuffled).length;
            }
        }
        else
        {
            timeLeftUntilNextState -= Time.deltaTime;
        }
    }
}
