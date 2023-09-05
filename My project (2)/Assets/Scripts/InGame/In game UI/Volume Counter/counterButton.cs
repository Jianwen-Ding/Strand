using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterButton : MonoBehaviour
{
    int index;
    baseCounter cacheCounter;
    //public get/set variables
    public void setIndex(int setIndex)
    {
        index = setIndex;
    }
    public void setCacheCounter(baseCounter setCounter)
    {
        transform.SetParent(setCounter.transform.parent);
        cacheCounter = setCounter;
    }
    //button triggerc
    public void activateButton()
    {
        if (cacheCounter.getCurrentCount() == 1 && index == 1)
        {
            cacheCounter.updateCounter(0);
        }
        else {
            cacheCounter.updateCounter(index);
        }
    }
}
