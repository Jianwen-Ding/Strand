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
    [SerializeField]
    private GameObject[] pageKey;
    //displacement that lines up with each part of the key
    [SerializeField]
    private Vector2[] displacementKey;
    //Determines if grid has been generated yet
    [SerializeField]
    private bool generatedYet = false;
    //Determines if grid has been loaded in
    [SerializeField]
    private bool loadedIn = false;
    //-- parameters to load and unload with based on distance--
    [SerializeField]
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
    public string getPageTheme()
    {
        return pageTheme;
    }
    public void setPageTheme(string set)
    {
        pageTheme = set;
    }
    public string getPageSpecialUse()
    {
        return pageSpecialUse;
    }
    public void setPageSpecialUse(string set)
    {
        pageSpecialUse = set;
    }
    public bool getLoadedIn()
    {
        return loadedIn;
    }
    public void setLoadedIn(bool set)
    {
        loadedIn = set;
    }
    public void generateGrid()
    {
        if (!generatedYet)
        {
            generatedYet = true;
            //gets needed variables
            gridPageStorer.page page = gridPageStorer.findRandomSuitablePage(upOpen, downOpen, leftOpen, rightOpen, pageTheme, pageSpecialUse);
            if (page != null)
            {
                pageMap = page.getMap();
            }
            else
            {
                print("ERROR- MAP NOT FOUND");
            }
            pageKey = gridPageStorer.getGridKey();
            //loads page
            for (int y = 0; y < pageMap.Length; y++)
            {
                for (int x = 0; x < pageMap[y].Length; x++)
                {
                    GameObject newObject;
                    switch (pageMap[y][x]) 
                    { 
                        default:
                            newObject = Instantiate(pageKey[pageMap[y][x]], new Vector3(displacementKey[pageMap[y][x]].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[pageMap[y][x]].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            break;

                        case 2:
                            if (upOpen)
                            {
                                newObject = Instantiate(pageKey[0], new Vector3(displacementKey[0].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[0].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            else
                            {
                                newObject = Instantiate(pageKey[1], new Vector3(displacementKey[1].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[1].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            break;

                        case 3:
                            if (rightOpen)
                            {
                                newObject = Instantiate(pageKey[0], new Vector3(displacementKey[0].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[0].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            else
                            {
                                newObject = Instantiate(pageKey[1], new Vector3(displacementKey[1].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[1].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            break;

                        case 4:
                            if (downOpen)
                            {
                                newObject = Instantiate(pageKey[0], new Vector3(displacementKey[0].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[0].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            else
                            {
                                newObject = Instantiate(pageKey[1], new Vector3(displacementKey[1].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[1].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            break;

                        case 5:
                            if (leftOpen)
                            {
                                newObject = Instantiate(pageKey[0], new Vector3(displacementKey[0].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[0].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            else
                            {
                                newObject = Instantiate(pageKey[1], new Vector3(displacementKey[1].x + initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, displacementKey[1].y + initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                            }
                            break;

                    }
                    loadedObject.Add(newObject);
                    newObject.transform.parent = gameObject.transform;
                }
            }
        }
        else
        {
            print("ERROR- THIS GRID SEEMS TO HAVE ALREADY BEEN GENERATED BUT FUNCTION -generateGrid- HAS BEEN CALLED AGAIN");
        }
        
    }
}
