using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singleGridPageLoader : MonoBehaviour
{
    //Extract grid templates from gridPageStorer
    //loads a grid of objects
    //-- parameters that will most likely stay the same with 
    private float totalXLength;
    private float totalYLength;
    private float gridXDistance;
    private float gridYDistance;
    //-- adjustable parameters to load grid with--
    private bool upOpen;
    private bool downOpen;
    private bool rightOpen;
    private bool leftOpen;
    //The same variables load diffrent themes
    private string pageTheme;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
