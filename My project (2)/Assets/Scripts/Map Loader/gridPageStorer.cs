using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridPageStorer : MonoBehaviour
{
    //---STRING THAT STORES ALL PAGE CLASSES, WILL BE CONVERTED IN START FUNCTION--
    /*-FORMAT-
    
    *ALL CAPTALIZED BEGGINING DESCRIPTOR
    -ALL CAPTALIZED NAME OF PAGE-
    upOpen: {boolean}
    downOpen: {boolean}
    leftOpen: {boolean}
    rightOpen: {boolean}
    pageTheme: {string}
    pageSpecialUse: {string}
    pageMap:
    [2D ARRAY]

    */
    // "-" marks the start and name of a page
    // "{" marks the start and value of a parameter
    // "[" and "," and "]" are used to signify the 2D array
    //DO NOT USE THOSE SYMBOLS OUTSIDE OF THOSE USES OR RISK ERROR
    [SerializeField]
    private static string gridInfo = "GRID PAGES.TXT HAS NOT BEEN ACCSESSED YET";
    //File Location of TXT file
    [SerializeField]
    private const string AccessGridInfoPath = @"Assets\information storage\grid_pages.txt";
    //Stores all grids here !! USE PAGE CLASS !!
    [SerializeField]
    private static ArrayList gridsAvailable = new ArrayList();
    //Object place in index is key for placement in integer maps, this variable will be plugged into the actual static variable
    [SerializeField]
    private GameObject[] gridKeyListSerialized;
    //Static key to be sent out to others
    private static GameObject[] gridKeyListStatic;
    //Storage Class
    public class page
    {
        private string pageName;
        private bool upOpen;
        private bool downOpen;
        private bool leftOpen;
        private bool rightOpen;
        //Theme marks the general region that a page is located in
        //"all" - can be loaded regardless of theme
        private string pageTheme;
        //Use specifies a specific special use for this type of region
        //"default" - has no special properties
        private string pageSpecialUse;
        //index of numbers, each number repersenting difffrent things
        //-0- empty
        //-1- ground tile
        //-2- wall tile
        //-3- enemy potential spawn point
        //-4- reward potential spawn point
        private int[][] pageMap;
        //Constructor for page
        public page(string setPageName, bool setUpOpen, bool setDownOpen, bool setLeftOpen, bool setRightOpen, string setPageTheme, string setPageSpecialUse, int[][] setPageMap)
        {
            pageName = setPageName;
            upOpen = setUpOpen;
            downOpen = setDownOpen;
            leftOpen = setLeftOpen;
            rightOpen = setRightOpen;
            pageTheme = setPageTheme;
            pageSpecialUse = setPageSpecialUse;
            pageMap = setPageMap;
        }
        //Checks if page falls in line with given parameters and is a viable canidate to be placed in
        public bool pageQualifies(bool checkUpOpen, bool checkDownOpen, bool checkLeftOpen, bool checkRightOpen, string checkPageTheme, string checkPageSpecialUse)
        {
            return ((checkUpOpen == upOpen || upOpen) && (checkDownOpen == downOpen || downOpen) && (checkRightOpen == rightOpen || rightOpen) && (checkLeftOpen == leftOpen || leftOpen) && (checkPageTheme.Equals(pageTheme) || pageTheme.Equals("all")) && checkPageSpecialUse.Equals(pageSpecialUse));
        }
        //Gives Out Entire Value of Map
        public override string ToString()
        {
            string initialString ="Name of the page: "  + pageName + "\n upOpen: " + upOpen + "\n downOpen: " + downOpen + "\n leftOpen: " + leftOpen + "\n rightOpen: " + rightOpen + "\n pageTheme: " + pageTheme + "\n pageSpecialUse: " + pageSpecialUse + " \n pageMap: \n";
            string gridPrint = "[";
            for(int y = 0; y < pageMap.Length; y++)
            {
                if (y != 0)
                {
                    gridPrint = gridPrint + ", ";
                }
                gridPrint = gridPrint + "[";
                for (int x = 0; x < pageMap[y].Length; x++)
                {
                    if (x != 0)
                    {
                        gridPrint = gridPrint + ", ";
                    }
                    gridPrint = gridPrint + pageMap[y][x];
                }
                gridPrint = gridPrint + "]\n";
            }
            gridPrint = gridPrint + "]";
            return initialString + gridPrint;
        }
        //Get set functions
        public int[][] getMap()
        {
            return pageMap;
        }
        public string getName()
        {
            return pageName;
        }
    }
    //Finds random suitable Page
    public static page findRandomSuitablePage(bool checkUpOpen, bool checkDownOpen, bool checkLeftOpen, bool checkRightOpen, string checkPageTheme, string checkPageSpecialUse)
    {
        ArrayList suitablePages = new ArrayList();
        for (int i = 0; i < gridsAvailable.Count; i++)
        {
            page currentPage = (page)gridsAvailable[i];
            if (currentPage.pageQualifies(checkUpOpen, checkDownOpen, checkLeftOpen, checkRightOpen, checkPageTheme, checkPageSpecialUse))
            {
                suitablePages.Add(currentPage);
            }
        }
        if (suitablePages.Count != 0)
        {
            return (page)suitablePages[Random.Range(0, suitablePages.Count)];
        }
        else
        {
            print("ERROR- Could not find any suitable pages ");
            print("checkUpOpen: "+ checkUpOpen);
            print("checkDownOpen: " + checkDownOpen);
            print("checkRightOpen: " + checkRightOpen);
            print("checkLeftOpen: " + checkLeftOpen);
            return null;
        }
    }
    //Finds Page with a specific name
    public static page findPageWithName(string name)
    {
        for (int i = 0; i < gridsAvailable.Count; i++)
        {
            page currentPage = (page)gridsAvailable[i];
            if (currentPage.getName() == name)
            {
                return currentPage;
            }
        }
        return null;
    }
    //Get/Set Functions
    public static GameObject[] getGridKey()
    {
        return gridKeyListStatic;
    }
    void Awake()
    {
        gridKeyListStatic = gridKeyListSerialized;
        //Loads in grid pages.txt into gridInfo
        //Skips first 55 lines
        string[] segmentedString = System.IO.File.ReadAllLines(AccessGridInfoPath);
        gridInfo = "";
        for(int i = 55; i < segmentedString.Length; i++)
        {
            gridInfo = gridInfo + segmentedString[i];
        }
        // Loads Text Into page classes
        /*-FORMAT-
    
        *ALL CAPTALIZED BEGGINING DESCRIPTOR
        -ALL CAPTALIZED NAME OF PAGE-
        upOpen: {boolean}
        downOpen: {boolean}
        leftOpen: {boolean}
        rightOpen: {boolean}
        pageTheme: {string}
        pageSpecialUse: {string}
        pageMap:
        [2D ARRAY]

        */
        string textNew = gridInfo;
        //finds the endpoint of a page
        while (textNew.IndexOf("-") != -1)
        {
            string newPageName;
            bool newUpOpen;
            bool newDownOpen;
            bool newLeftOpen;
            bool newRightOpen;
            string newPageTheme;
            string newPageSpecialUse;
            int[][] newPageMap;
            //Finds non grid parameters
            //Finds Name
            textNew = textNew.Substring(textNew.IndexOf("-") + 1);
            newPageName = textNew.Substring(0, textNew.IndexOf("-"));
            textNew = textNew.Substring(textNew.IndexOf("-") + 1);
            //Finds Up Open
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newUpOpen = bool.Parse(textNew.Substring(0, textNew.IndexOf("}")));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Down Open
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newDownOpen = bool.Parse(textNew.Substring(0, textNew.IndexOf("}")));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Left Open
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newLeftOpen = bool.Parse(textNew.Substring(0, textNew.IndexOf("}")));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Right Open
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newRightOpen = bool.Parse(textNew.Substring(0, textNew.IndexOf("}")));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Page Theme
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newPageTheme = textNew.Substring(0, textNew.IndexOf("}"));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Page Special Use
            textNew = textNew.Substring(textNew.IndexOf("{") + 1);
            newPageSpecialUse = textNew.Substring(0, textNew.IndexOf("}"));
            textNew = textNew.Substring(textNew.IndexOf("}") + 1);
            //Finds Grid
            textNew = textNew.Substring(textNew.IndexOf("[") + 1);
            string yArrayProcessor = textNew;
            int amountOfYArrays = 0;
            //Counts amount of Y Arrays There Are
            while (yArrayProcessor.IndexOf("[") != -1 && (yArrayProcessor.IndexOf("[") < yArrayProcessor.IndexOf("-") || yArrayProcessor.IndexOf("-") == -1))
            {
                yArrayProcessor = yArrayProcessor.Substring(yArrayProcessor.IndexOf("[") + 1);
                amountOfYArrays += 1;
            }
            newPageMap = new int[amountOfYArrays][];
            int yAxisIndex = 0;
            while (textNew.IndexOf("[") != -1 && (textNew.IndexOf("[") < textNew.IndexOf("-") || textNew.IndexOf("-") == -1))
            {
                textNew = textNew.Substring(textNew.IndexOf("[") + 1);
                ArrayList componentsOfX = new ArrayList();
                while (textNew.IndexOf(",") != -1 && textNew.IndexOf(",") < textNew.IndexOf("]"))
                {
                    componentsOfX.Add(int.Parse(textNew.Substring(0, textNew.IndexOf(","))));
                    textNew = textNew.Substring(textNew.IndexOf(",") + 1);
                    
                }
                componentsOfX.Add(int.Parse(textNew.Substring(0, textNew.IndexOf("]"))));
                newPageMap[yAxisIndex] = new int[componentsOfX.Count];
                for (int i = 0; i < componentsOfX.Count; i++)
                {
                    newPageMap[yAxisIndex][i] = (int)componentsOfX[i];
                }
                yAxisIndex++;
            }
            //Combines all information into new page
            gridsAvailable.Add(new page(newPageName, newUpOpen, newDownOpen, newLeftOpen, newRightOpen, newPageTheme, newPageSpecialUse, newPageMap));
        }
        /*
        for(int i = 0; i < gridsAvailable.Count; i++)
        {
            print(gridsAvailable[i]);
        }
        */
    }
}
