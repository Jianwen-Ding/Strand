using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGroundThemeSystem : MonoBehaviour
{
    //audio clip presets
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
    int dayState;
    //manages changes into next state
    float timeLeftUntilNextState;
    //Cache variable
    AudioSource soundSource;
    // Start is called before the first frame update
    void Start()
    {
        timeLeftUntilNextState = dawnTheme.length;
        dayState = 0;
        isMuffled = false;
        soundSource = gameObject.GetComponent<AudioSource>();
    }
    public void playThemeFromIndex(int indexSet, bool muffleSet)
    {
        AudioClip returnTheme;
        float returnVolume;
        if (muffleSet)
        {
            switch (indexSet)
            {
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
        soundSource.volume = returnVolume;
        soundSource.PlayOneShot(returnTheme);
        
    }
    //Toggles wether to switch to unmuffled or muffled 
    public void muffelToggle(bool toggleSet)
    {
        if(toggleSet != isMuffled)
        {
            float currentPlayBackTime = soundSource.time;
            playThemeFromIndex(dayState, toggleSet);
            soundSource.time = currentPlayBackTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
