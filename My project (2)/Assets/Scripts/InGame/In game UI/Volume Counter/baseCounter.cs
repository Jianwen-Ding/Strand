using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//To be used a parent class for counters
public class baseCounter : MonoBehaviour
{
    // max amount counted, handles amount of icons loaded
    [SerializeField]
    int maxCount;
    [SerializeField]
    private int currentCount;
    //where the count icon for count 1 starts
    [SerializeField]
    Vector2 startingPosition;
    //change in position for a additional counter icon
    [SerializeField]
    Vector2 incrementalChange;
    //color of icon when icon index below or at the current count
    [SerializeField]
    Color incrementedToColor;
    //color of icon when icon index greater current count
    [SerializeField]
    Color incrementedBehindColor;
    [SerializeField]
    GameObject iconPrefab;
    //list of generated icons
    [SerializeField]
    GameObject[] cacheIconList;
    //list of the button scripts of the icons
    [SerializeField]
    counterButton[] cacheScriptList;
    //list of the button images of the icons
    [SerializeField]
    Image[] cacheSpriteList;
    //checks if the icons have buttons
    [SerializeField]
    bool iconHaveButtonScript;
    //For child classes to use
    public void setCurrentCount(int setCount)
    {
        currentCount = setCount;
    }
    public int getCurrentCount()
    {
        return currentCount;
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        iconHaveButtonScript = (iconPrefab.GetComponent<counterButton>() != null);
        //establishes arrays
        cacheScriptList = new counterButton[maxCount];
        cacheIconList = new GameObject[maxCount];
        cacheSpriteList = new Image[maxCount];
        for (int i = 0; i < maxCount; i++)
        {
            //creates icons
            GameObject createdIcon = Instantiate(iconPrefab, startingPosition + incrementalChange * i, Quaternion.identity.normalized);
            //fills arrays
            cacheIconList[i] = createdIcon;
            cacheSpriteList[i] = createdIcon.GetComponent<Image>();
            if (iconHaveButtonScript)
            {
                cacheScriptList[i] = createdIcon.GetComponent<counterButton>();
                cacheScriptList[i].setIndex(i + 1);
                cacheScriptList[i].setCacheCounter(this);
            }
        }
    }
    public virtual void updateCounter(int setNewCounter)
    {
        currentCount = setNewCounter;
        //adjsuts color based on current count
        for(int i = 0; i < maxCount; i++)
        {
            if(i + 1 <= currentCount)
            {
                cacheSpriteList[i].color = incrementedToColor;
            }
            else
            {
                cacheSpriteList[i].color = incrementedBehindColor;
            }
        }
    }
}
