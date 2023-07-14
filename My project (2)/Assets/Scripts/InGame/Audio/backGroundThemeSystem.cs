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
    AudioClip transitionBell;
    [SerializeField]
    bool isMuffled;
    //state starts from 0 and goes up as each song ends
    //it goes from
    //0 - dawn
    //1 - midday
    //2 - dusk
    //3 - night
    int dayState;
    //Cache variable
    AudioSource soundSource;
    // Start is called before the first frame update
    void Start()
    {
        dayState = 0;
        isMuffled = false;
        soundSource = gameObject.GetComponent<AudioSource>();
    }
    public AudioClip findTheme(int indexSet, bool muffleSet)
    {
        AudioClip returnTheme;
        if (muffleSet)
        {
            switch (indexSet)
            {
                case 0:
                    returnTheme = dawnThemeMuffled;
                    break;
                case 1:
                    returnTheme = middayThemeMuffled;
                    break;
                case 2:
                    returnTheme = duskThemeMuffled;
                    break;
                case 3:
                    returnTheme = nightThemeMuffled;
                    break;
                default:
                    returnTheme = null;
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
                    break;
                case 1:
                    returnTheme = middayTheme;
                    break;
                case 2:
                    returnTheme = duskTheme;
                    break;
                case 3:
                    returnTheme = nightTheme;
                    break;
                default:
                    returnTheme = null;
                    print("ERROR- index given goes past parameters");
                    break;
            }
        }
        return returnTheme;
    }
    //Toggles wether to switch to unmuffled or muffled 
    public void muffelToggle(bool toggleSet)
    {
        if(toggleSet != isMuffled)
        {
            float currentPlayBackTime = soundSource.time;
            soundSource.PlayOneShot(findTheme(dayState, isMuffled));
            soundSource.time = currentPlayBackTime;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //if(soundSource.)
    }
}
