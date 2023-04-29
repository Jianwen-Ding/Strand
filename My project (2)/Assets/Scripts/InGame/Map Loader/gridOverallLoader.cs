using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridOverallLoader : MonoBehaviour
{
    //Setup variables
    //This needs to be attached through serializeField
    [SerializeField]
    private miniMapGenerator miniMap;
    //Player Locations 
    [SerializeField]
    private int playerLocationX;
    [SerializeField]
    private int playerLocationY;
    //Grid Set
    private singleGridPageLoader[][] pageGridMap;
    //Grid Set Toggles
    private singleGridLoadToggle[][] pageGridMapToggle;
    //--Information for initial generation--
    [SerializeField]
    private GameObject pagePrefab;
    //Steps for generation
    //1- Loads Grid into postion
    //2- Randomly arrange the openings of the pages (each opening calculated using percentageChanceForOpen)
    //3- Resolves one sided connections and patches outstanding opennings
    //4- Grids next to center must be connected to it
    //5- Finds Center and uses openings to find all grids that can be travelled to in current configuration
    //6- Finds Grid connected to center that is next to unconnected to grid and connects the two
    //6- Repeat from step 4-5 until all are connected
    //7- Adds a number of resource grids per distance from center randomly
    //8- Generates pages
    [SerializeField]
    string theme;
    [SerializeField]
    private Vector2 initialVector;
    private Vector2 diffrenceBetweenPages = new Vector2((float)-17.7777782, 10);
    //USE UNEVEN NUMBER FOR GRID LENGTH IN ORDER TO MAKE A SINGLE CENTER GRID
    [SerializeField]
    private int xGridLength;
    [SerializeField]
    private int yGridLength;
    //Chance for open side on page in percentage out of 100
    [SerializeField]
    private float percentageChanceForOpen;
    private int centerX;
    private int centerY;
    //Amount of Food Surplus pages
    [SerializeField]
    private int foodSurplusPages;
    //Amount of Water Surplus pages
    [SerializeField]
    private int waterSurplusPages;
    //Amount of Scrap Surplus pages
    [SerializeField]
    private int scrapSurplusPages;
    // Start is called before the first frame update
    void Start()
    {
        //--Generates grid--
        //Used in step 4
        int[][] generationLoadMap;
        //1: Loading Grids into position
        pageGridMap = new singleGridPageLoader[yGridLength][];
        pageGridMapToggle = new singleGridLoadToggle[yGridLength][];
        generationLoadMap = new int[yGridLength][];
        for (int y = 0; y < yGridLength; y++)
        {
            pageGridMap[y] = new singleGridPageLoader[xGridLength];
            pageGridMapToggle[y] = new singleGridLoadToggle[xGridLength];
            generationLoadMap[y] = new int[xGridLength];
            for (int x = 0; x < xGridLength; x++)
            {
                generationLoadMap[y][x] = 0;
                GameObject loadedObject = Instantiate(pagePrefab, new Vector3(initialVector.x + (-diffrenceBetweenPages.x * xGridLength) / 2 + diffrenceBetweenPages.x * x, initialVector.y + (-diffrenceBetweenPages.y * xGridLength) / 2 + diffrenceBetweenPages.y * y), Quaternion.identity.normalized);
                //Setting up load toggle
                singleGridLoadToggle currentLoadedScripToggle = loadedObject.GetComponent<singleGridLoadToggle>();
                currentLoadedScripToggle.setGridPositionX(x);
                currentLoadedScripToggle.setGridPositionY(y);
                pageGridMapToggle[y][x] = currentLoadedScripToggle;
                singleGridPageLoader currentLoadedScript = loadedObject.GetComponent<singleGridPageLoader>();
                pageGridMap[y][x] = currentLoadedScript;
                //2: Randomly arrange the openings of the pages
                currentLoadedScript.setUpOpen(Random.Range(0, 100) < percentageChanceForOpen);
                currentLoadedScript.setDownOpen(Random.Range(0, 100) < percentageChanceForOpen);
                currentLoadedScript.setRightOpen(Random.Range(0, 100) < percentageChanceForOpen);
                currentLoadedScript.setLeftOpen(Random.Range(0, 100) < percentageChanceForOpen);
                currentLoadedScript.setPageTheme(theme);
                currentLoadedScript.setPageSpecialUse("default");
                currentLoadedScript.setLoadedIn(false);
            }
        }
        //2:
        for (int y = 0; y < pageGridMap.Length; y++)
        {
            for (int x = 0; x < pageGridMap[y].Length; x++)
            {
                //Checks Page Above
                if (y + 1 < generationLoadMap.Length)
                {
                    if ((pageGridMap[y + 1][x].getUpOpen() || pageGridMap[y][x].getDownOpen()) && (!pageGridMap[y + 1][x].getUpOpen() || !pageGridMap[y][x].getDownOpen()))
                    {
                        pageGridMap[y + 1][x].setUpOpen(true);
                        pageGridMap[y][x].setDownOpen(true);
                    }
                }
                else
                {
                    if (pageGridMap[y][x].getDownOpen())
                    {
                        pageGridMap[y][x].setDownOpen(false);
                    }
                }
                //Checks Page Below
                if (y - 1 >= 0)
                {
                    if ((pageGridMap[y - 1][x].getDownOpen() || pageGridMap[y][x].getUpOpen()) && (!pageGridMap[y - 1][x].getDownOpen() || !pageGridMap[y][x].getUpOpen()))
                    {
                        pageGridMap[y - 1][x].setDownOpen(true);
                        pageGridMap[y][x].setUpOpen(true);
                    }
                }
                else
                {
                    if (pageGridMap[y][x].getUpOpen())
                    {
                        pageGridMap[y][x].setUpOpen(false);
                    }
                }
                //Checks Page Right
                if (x + 1 < generationLoadMap[y].Length)
                {
                    if ((pageGridMap[y][x + 1].getRightOpen() || pageGridMap[y][x].getLeftOpen()) && (!pageGridMap[y][x + 1].getRightOpen() || !pageGridMap[y][x].getLeftOpen()))
                    {
                        pageGridMap[y][x + 1].setRightOpen(true);
                        pageGridMap[y][x].setLeftOpen(true);
                    }
                }
                else
                {
                    if (pageGridMap[y][x].getLeftOpen())
                    {
                        pageGridMap[y][x].setLeftOpen(false);
                    }
                }
                //Checks Page Left
                if (x - 1 >= 0)
                {
                    if ((pageGridMap[y][x - 1].getLeftOpen() || pageGridMap[y][x].getRightOpen()) && (!pageGridMap[y][x - 1].getLeftOpen() || !pageGridMap[y][x].getRightOpen()))
                    {
                        pageGridMap[y][x - 1].setLeftOpen(true);
                        pageGridMap[y][x].setRightOpen(true);
                    }
                }
                else
                {
                    if (pageGridMap[y][x].getRightOpen())
                    {
                        pageGridMap[y][x].setRightOpen(false);
                    }
                }
            }
        }
        //3: connects center to everything next to it
        centerX = (xGridLength / 2);
        centerY = (yGridLength / 2);
        pageGridMap[centerY][centerX].setUpOpen(true);
        pageGridMap[centerY][centerX].setDownOpen(true);
        pageGridMap[centerY][centerX].setRightOpen(true);
        pageGridMap[centerY][centerX].setLeftOpen(true);
        pageGridMap[centerY][centerX].setPageTheme(theme);
        pageGridMap[centerY][centerX].setPageSpecialUse("base");
        pageGridMap[centerY][centerX].setLoadedIn(true);
        pageGridMap[centerY + 1][centerX].setUpOpen(true);
        pageGridMap[centerY + 1][centerX].setLoadedIn(true);
        pageGridMap[centerY - 1][centerX].setDownOpen(true);
        pageGridMap[centerY - 1][centerX].setLoadedIn(true);
        pageGridMap[centerY][centerX + 1].setRightOpen(true);
        pageGridMap[centerY][centerX + 1].setUpOpen(true);
        pageGridMap[centerY][centerX - 1].setLeftOpen(true);
        pageGridMap[centerY][centerX - 1].setUpOpen(true);
        bool hasFilledIn = false;
        int currentDistance;
        //Loops until the entire grid is accessible
        while (!hasFilledIn)
        {
            //A 2D arrayList, contains locations of gridspaces specific distances
            ArrayList accessibleLocations = new ArrayList();
            accessibleLocations.Add(new ArrayList());
            accessibleLocations.Add(new ArrayList());
            ((ArrayList)accessibleLocations[1]).Add(new Vector2(centerX, centerY));
            bool hasReachedAllPossible = false;
            //Clears integer map
            generationLoadMap = new int[yGridLength][];
            for (int y = 0; y < yGridLength; y++)
            {
                generationLoadMap[y] = new int[xGridLength];
                for (int x = 0; x < xGridLength; x++)
                {
                    generationLoadMap[y][x] = 0;
                }
            }
            generationLoadMap[centerX][centerY] = 1;
            //4: Finds places center connects to
            //Loops until algorithm finds all that is accessible
            currentDistance = 1;
            while (!hasReachedAllPossible)
            {
                accessibleLocations.Add(new ArrayList());
                for (int i = 0; i < ((ArrayList)accessibleLocations[currentDistance]).Count; i++)
                {
                    int x = (int)((Vector2)((ArrayList)accessibleLocations[currentDistance])[i]).x;
                    int y = (int)((Vector2)((ArrayList)accessibleLocations[currentDistance])[i]).y;
                    if (generationLoadMap[y][x] == currentDistance)
                    {
                        //Checks Page Above
                        if (y + 1 < generationLoadMap.Length && generationLoadMap[y + 1][x] == 0 && pageGridMap[y + 1][x].getUpOpen() && pageGridMap[y][x].getDownOpen())
                        {
                            generationLoadMap[y + 1][x] = currentDistance + 1;
                            ((ArrayList)accessibleLocations[currentDistance + 1]).Add(new Vector2(x, y + 1));
                        }
                        //Checks Page Below
                        if (y - 1 >= 0 && generationLoadMap[y - 1][x] == 0 && pageGridMap[y - 1][x].getDownOpen() && pageGridMap[y][x].getUpOpen())
                        {
                            generationLoadMap[y - 1][x] = currentDistance + 1;
                            ((ArrayList)accessibleLocations[currentDistance + 1]).Add(new Vector2(x, y - 1));
                        }
                        //Checks Page Right
                        if (x + 1 < generationLoadMap[y].Length && generationLoadMap[y][x + 1] == 0 && pageGridMap[y][x + 1].getRightOpen() && pageGridMap[y][x].getLeftOpen())
                        {
                            generationLoadMap[y][x + 1] = currentDistance + 1;
                            ((ArrayList)accessibleLocations[currentDistance + 1]).Add(new Vector2(x + 1, y));
                        }
                        //Checks Page Left
                        if (x - 1 >= 0 && generationLoadMap[y][x - 1] == 0 && pageGridMap[y][x - 1].getLeftOpen() && pageGridMap[y][x].getRightOpen())
                        {
                            generationLoadMap[y][x - 1] = currentDistance + 1;
                            ((ArrayList)accessibleLocations[currentDistance + 1]).Add(new Vector2(x - 1, y));
                        }
                    }
                }
                if (((ArrayList)accessibleLocations[currentDistance + 1]).Count <= 0)
                {
                    hasReachedAllPossible = true;
                }
                currentDistance++;
            }
            //5: Finds grid page next to existing one
            bool foundInaccessibleArea = false;
            for (int dist = 1; dist < accessibleLocations.Count && !foundInaccessibleArea; dist++)
            {
                for (int i = 0; i < ((ArrayList)accessibleLocations[dist]).Count && !foundInaccessibleArea; i++)
                {
                    int x = (int)((Vector2)((ArrayList)accessibleLocations[dist])[i]).x;
                    int y = (int)((Vector2)((ArrayList)accessibleLocations[dist])[i]).y;
                    //6: Finds closest page that is next to a non accessible page
                    bool upPossible = false;
                    bool downPossible = false;
                    bool rightPossible = false;
                    bool leftPossible = false;
                    int possible = 0;
                    //Checks Page Above
                    if (y + 1 < generationLoadMap.Length && generationLoadMap[y + 1][x] == 0)
                    {
                        possible++;
                        upPossible = true;
                    }
                    //Checks Page Below
                    else if (y - 1 >= 0 && generationLoadMap[y - 1][x] == 0)
                    {
                        possible++;
                        downPossible = true;
                    }
                    //Checks Page Right
                    else if (x + 1 < generationLoadMap[y].Length && generationLoadMap[y][x + 1] == 0)
                    {
                        possible++;
                        rightPossible = true;
                    }
                    //Checks Page Left
                    else if (x - 1 >= 0 && generationLoadMap[y][x - 1] == 0)
                    {
                        possible++;
                        leftPossible = true;
                    }
                    //Randomly Picks a possible move
                    if (leftPossible || upPossible || downPossible || rightPossible)
                    {
                        if (leftPossible && ((!upPossible && !downPossible && !rightPossible) || Random.Range(0, possible + 2) == 0))
                        {
                            pageGridMap[y][x - 1].setLeftOpen(true);
                            pageGridMap[y][x].setRightOpen(true);
                        }
                        if (rightPossible && ((!upPossible && !downPossible) || Random.Range(0, possible + 2) == 0))
                        {
                            pageGridMap[y][x + 1].setRightOpen(true);
                            pageGridMap[y][x].setLeftOpen(true);
                        }
                        if (upPossible && ((!downPossible) || Random.Range(0, possible + 2) == 0))
                        {
                            pageGridMap[y + 1][x].setUpOpen(true);
                            pageGridMap[y][x].setDownOpen(true);
                        }
                        if (downPossible)
                        {
                            pageGridMap[y - 1][x].setDownOpen(true);
                            pageGridMap[y][x].setUpOpen(true);
                        }
                        foundInaccessibleArea = true;
                    }
                }
            }
            if (!foundInaccessibleArea)
            {
                hasFilledIn = true;
            }

        }
        //7: Loads in special use pages
        for (int i = 0; i < foodSurplusPages; i++)
        {
            int y = Random.Range(0, pageGridMap.Length);
            int x = Random.Range(0, pageGridMap[y].Length);
            while (pageGridMap[y][x].getPageSpecialUse() != "default")
            {
                y = Random.Range(0, pageGridMap.Length);
                x = Random.Range(0, pageGridMap[y].Length);
            }
            pageGridMap[y][x].setPageSpecialUse("food");
        }
        for (int i = 0; i < waterSurplusPages; i++)
        {
            int y = Random.Range(0, pageGridMap.Length);
            int x = Random.Range(0, pageGridMap[y].Length);
            while (pageGridMap[y][x].getPageSpecialUse() != "default")
            {
                y = Random.Range(0, pageGridMap.Length);
                x = Random.Range(0, pageGridMap[y].Length);
            }
            pageGridMap[y][x].setPageSpecialUse("water");
        }
        for (int i = 0; i < scrapSurplusPages; i++)
        {
            int y = Random.Range(0, pageGridMap.Length);
            int x = Random.Range(0, pageGridMap[y].Length);
            while (pageGridMap[y][x].getPageSpecialUse() != "default")
            {
                y = Random.Range(0, pageGridMap.Length);
                x = Random.Range(0, pageGridMap[y].Length);
            }
            pageGridMap[y][x].setPageSpecialUse("scrap");
        }
        //Loads in Water Grid Surplus Pages
        //8: Loads in grid pages
        for (int y = 0; y < pageGridMap.Length; y++)
        {
            for (int x = 0; x < pageGridMap[y].Length; x++)
            {
                ((singleGridPageLoader)pageGridMap[y][x]).generateGrid();
            }
        }
        //begins mini map generation
        miniMap.loadMiniMap();
        //deactivates distants grids and places player
        updatePlayerPosition(centerX, centerY);
    }
    //Public functions
    public singleGridPageLoader[][] getPageGridMap()
    {
        return pageGridMap;
    }
    public void updatePlayerPosition(int positionX, int positionY)
    {
        //Updates loading status of surrounding grids
        playerLocationX = positionX;
        playerLocationY = positionY;
        for(int y = 0; y < pageGridMap.Length; y++)
        {
            for (int x = 0; x < pageGridMap[y].Length; x++)
            {
                if(Mathf.Abs(playerLocationX - x) <= 1 && Mathf.Abs(playerLocationY - y) <= 1)
                {
                    pageGridMapToggle[y][x].activateGrid();
                }
                else
                {
                    pageGridMapToggle[y][x].deactivateGrid();
                }
            }
        }
        //Updates loading status of minimap
        miniMap.recenterMiniMap(playerLocationX, playerLocationY);
        miniMap.getMiniMapGridSymbols()[positionY][positionX].SetActive(true);
    }
    public int getPlayerPositionX()
    {
        return playerLocationX;
    }
    public int getPlayerPositionY()
    {
        return playerLocationY;
    }
    public int getXGridLength()
    {
        return xGridLength;
    }
    public int getYGridLength()
    {
        return yGridLength;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
