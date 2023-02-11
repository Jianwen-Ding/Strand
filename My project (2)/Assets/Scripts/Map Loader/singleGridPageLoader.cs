using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class singleGridPageLoader : MonoBehaviour
{
    //Extract grid templates from gridPageStorer
    //loads a grid of objects
    //-- parameters that will most likely stay the same--
    [SerializeField]
    private Vector2 initialDisplacement;
    [SerializeField]
    private float gridXDistance;
    [SerializeField]
    private float gridYDistance;
    //-- adjustable parameters to load grid with--
    [SerializeField]
    private bool upOpen;
    [SerializeField]
    private bool downOpen;
    [SerializeField]
    private bool rightOpen;
    [SerializeField]
    private bool leftOpen;
    //The same variables load diffrent themes
    [SerializeField]
    private string pageTheme;
    //A specific use for 
    [SerializeField]
    private string pageSpecialUse;
    //integer map of layout to be found later
    private int[][] pageMap;
    //key for integer map
    private GameObject[] pageKey;
    //Determines if grid has been generated yet
    [SerializeField]
    private bool generatedYet = false;
    //Determines if grid has been loaded in
    [SerializeField]
    private bool loadedIn = false;
    //-- parameters to load and unload with based on distance--
    ArrayList loadedObject = new ArrayList();
    //Get/Set Variable
    public bool getUpOpen()
    {
        return upOpen;
    }
    public void setUpOpen(bool set)
    {
        upOpen = set;
    }
    public bool getDownOpen()
    {
        return downOpen;
    }
    public void setDownOpen(bool set)
    {
        downOpen = set;
    }
    public bool getRightOpen()
    {
        return rightOpen;
    }
    public void setRightOpen(bool set)
    {
        rightOpen = set;
    }
    public bool getLeftOpen()
    {
        return leftOpen;
    }
    public void setLeftOpen(bool set)
    {
        leftOpen = set;
    }
    public void generateGrid()
    {
        if (!generatedYet)
        {
            generatedYet = true;
            //gets needed variables
            pageMap = gridPageStorer.findRandomSuitablePage(upOpen, downOpen, leftOpen, rightOpen, pageTheme, pageSpecialUse).getMap();
            pageKey = gridPageStorer.getGridKey();
            //loads page
            for (int y = 0; y < pageMap.Length; y++)
            {
                for (int x = 0; x < pageMap[y].Length; x++)
                {
                    GameObject newObject = Instantiate(pageKey[pageMap[y][x]], new Vector3(initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                    loadedObject.Add(newObject);
                }
            }
        }
        else
        {
            print("ERROR- THIS GRID SEEMS TO HAVE ALREADY BEEN GENERATED BUT FUNCTION -generateGrid- HAS BEEN CALLED AGAIN");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
   
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
