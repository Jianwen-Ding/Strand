using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class scrapStorer : MonoBehaviour
{

    [SerializeField]
    private TextAsset baseScrapFile;
    private const string pathAddOn = "/StreamingAssets";
    private static string _currentFilePath = "";
    private const string beforeScrapCollectedIndicator = "TOTAL SCRAP COLLECTED";
    private const string beforeEscapesIndicator = "TIMES ESCAPED";
    private const string beforeGoldenIndicator = "<GOLDEN>";
    private const string afterGoldenIndicator = "<GOLDEN-END>";
    private const string beforeNormalIndicator = "<NORMAL>";
    private const string afterNormalIndicator = "<NORMAL-END>";
    [SerializeField]
    GameObject[] normalScrapPrefabs;
    private static GameObject[] normalPrefabsStatic;
    [SerializeField]
    GameObject[] goldenScrapPrefabs;
    private static GameObject[] goldenPrefabsStatic;
    // Stores the scrap states
    private class scrapState
    {
        bool isEnabled;
        string scrapPrefab;

        // Get/Set Functions
        public bool getIsEnabled()
        {
            return isEnabled;
        }

        public void setIsEnabled(bool setBool)
        {
            isEnabled = setBool;
        }

        public string getScrapPrefab()
        {
            return scrapPrefab;
        }

        public void setScrapPrefab(string setPrefab)
        {
            scrapPrefab = setPrefab;
        }

        // Constructor
        public scrapState(bool setEnabled, string setPrefab)
        {
            isEnabled = setEnabled;
            scrapPrefab = setPrefab;
        }
    }
    // Gets the current file path
    private static string currentFilePath()
    {
        if (_currentFilePath == "")
        {
            _currentFilePath = Application.persistentDataPath + pathAddOn;
        }
        return _currentFilePath;
    }

    // Sets the current save file to scrap
    public void revertCurrentScrap()
    {
        File.WriteAllText(currentFilePath(), baseScrapFile.text);
    }

    // Deletes the current file
    private static void deleteFile()
    {
        File.Delete(currentFilePath());
    }

    // Gets the text from the current file
    private static string getText()
    {
        string totalText;
        if (!File.Exists(currentFilePath()))
        {
            totalText = "";
        } else {
            using (StreamReader outputFile = new StreamReader(currentFilePath()))
            {
                totalText = outputFile.ReadToEnd();
            }
        }
    
        return totalText;
    }

    public static bool checkInitialized()
    {
        return File.Exists(currentFilePath());
    }
    // Skips the formatting and extracts the relevant text in the data
    private static string getFileRelevantText()
    {
        string totalText = getText();
        return totalText.Substring(totalText.IndexOf("===========DATA==========="));
    }

    // Skips the formatting and adjust the relevant text in the data
    private static void setFileRelevantText(string relevantText)
    {
        string totalText = getText();
        File.WriteAllText(currentFilePath(), totalText.Substring(0, totalText.IndexOf("===========DATA===========")) + relevantText);
    }

    // Gets the total scrap collected
    public static int getScrap()
    {
        string beforeScrap = getFileRelevantText().Substring(getFileRelevantText().IndexOf(beforeScrapCollectedIndicator));
        beforeScrap = beforeScrap.Substring(beforeScrap.IndexOf("|") + 1);
        return int.Parse(beforeScrap.Substring(0, beforeScrap.IndexOf("|")));
    }

    // Sets the total scrap collected
    public static void setScrap(int scrap)
    {
        int beforeScrapIndex = getFileRelevantText().IndexOf(beforeScrapCollectedIndicator);
        string beforeScrapIndicator = getFileRelevantText().Substring(beforeScrapIndex);
        int beforeScrapDataIndex = beforeScrapIndicator.IndexOf("|") + 1;
        string inScrap = beforeScrapIndicator.Substring(beforeScrapDataIndex);
        string afterScrap = inScrap.Substring(inScrap.IndexOf("|"));
        string beforeScrap = getFileRelevantText().Substring(0, beforeScrapIndex + beforeScrapDataIndex);
        setFileRelevantText(beforeScrap + scrap + afterScrap);
    }

    // Gets the times escaped
    public static int getEscapes()
    {
        string beforeEscape = getFileRelevantText().Substring(getFileRelevantText().IndexOf(beforeEscapesIndicator));
        beforeEscape = beforeEscape.Substring(beforeEscape.IndexOf("|") + 1);
        return int.Parse(beforeEscape.Substring(0, beforeEscape.IndexOf("|")));
    }

    // Sets the times escaped
    public static void setEscapes(int scrap)
    {
        int beforeEscapeIndex = getFileRelevantText().IndexOf(beforeEscapesIndicator);
        string beforeEscapeIndicator = getFileRelevantText().Substring(beforeEscapeIndex);
        int beforeEscapeDataIndex = beforeEscapeIndicator.IndexOf("|") + 1;
        string inEscape = beforeEscapeIndicator.Substring(beforeEscapeDataIndex);
        string afterEscape = inEscape.Substring(inEscape.IndexOf("|"));
        string beforeEscape = getFileRelevantText().Substring(0, beforeEscapeIndex + beforeEscapeDataIndex);
        setFileRelevantText(beforeEscape + scrap + afterEscape);
    }

    // Gets the normal scrap states
    private static scrapState[] getNormalScrapStates()
    {
        List<scrapState> returnStates = new List<scrapState>();
        int atStart = getFileRelevantText().IndexOf(beforeNormalIndicator);
        int atEnd = getFileRelevantText().IndexOf(afterNormalIndicator);
        string beforeStates = getFileRelevantText().Substring(atStart, atEnd - atStart);
        string inStates = beforeStates;
        while (inStates.Contains('['))
        {
            string prefabName;
            bool prefabEnabled;
            inStates = inStates.Substring(inStates.IndexOf("[") + 1);
            prefabName = inStates.Substring(0, inStates.IndexOf(","));
            inStates = inStates.Substring(inStates.IndexOf(",") + 1);
            string prefabCheck = inStates.Substring(0, inStates.IndexOf("]"));
            prefabEnabled = prefabCheck == "T";
            inStates = inStates.Substring(inStates.IndexOf("]") + 1);
            returnStates.Add(new scrapState(prefabEnabled, prefabName));
        }
        return returnStates.ToArray();
    }

    // Gets the golden scrap states
    private static scrapState[] getGoldenScrapStates()
    {
        List<scrapState> returnStates = new List<scrapState>();
        int atStart = getFileRelevantText().IndexOf(beforeGoldenIndicator);
        int atEnd = getFileRelevantText().IndexOf(afterGoldenIndicator);
        string beforeStates = getFileRelevantText().Substring(atStart, atEnd - atStart);
        string inStates = beforeStates;
        while (inStates.Contains('['))
        {
            string prefabName;
            bool prefabEnabled;
            inStates = inStates.Substring(inStates.IndexOf("[") + 1);
            prefabName = inStates.Substring(0, inStates.IndexOf(","));
            inStates = inStates.Substring(inStates.IndexOf(",") + 1);
            string prefabCheck = inStates.Substring(0, inStates.IndexOf("]"));
            prefabEnabled = prefabCheck == "T";
            inStates = inStates.Substring(inStates.IndexOf("]") + 1);
            returnStates.Add(new scrapState(prefabEnabled, prefabName));
        }
        return returnStates.ToArray();
    }

    // Gets list of enabled normal prefabs
    public static GameObject[] getNormalPrefabs()
    {
        scrapState[] states = getNormalScrapStates();
        List<GameObject> returnPrefabs = new List<GameObject>();
        for (int i = 0; i < normalPrefabsStatic.Length; i++)
        {
            bool hasFoundEnabled = false;
            for(int z = 0; z < states.Length; z++)
            {
                if (states[z].getScrapPrefab() == normalPrefabsStatic[i].name && states[z].getIsEnabled())
                {
                    hasFoundEnabled = true;
                }
            }
            if (hasFoundEnabled)
            {
                returnPrefabs.Add(normalPrefabsStatic[i]);
            }
        }
        return returnPrefabs.ToArray();
    }

    // Gets list of enabled golden prefabs
    public static GameObject[] getGoldenPrefabs()
    {
        scrapState[] states = getGoldenScrapStates();
        List<GameObject> returnPrefabs = new List<GameObject>();
        for (int i = 0; i < goldenPrefabsStatic.Length; i++)
        {
            bool hasFoundEnabled = false;
            for (int z = 0; z < states.Length; z++)
            {
                if (states[z].getScrapPrefab() == goldenPrefabsStatic[i].name && states[z].getIsEnabled())
                {
                    hasFoundEnabled = true;
                }
            }
            if (hasFoundEnabled)
            {
                returnPrefabs.Add(goldenPrefabsStatic[i]);
            }
        }
        return returnPrefabs.ToArray();
    }

    // Get normal scrap active state
    public static bool getNormalScrapEnabled(string scrapName)
    {
        scrapState[] states = getNormalScrapStates();
        for(int i = 0; i < states.Length; i++)
        {
            if (states[i].getScrapPrefab() == scrapName && states[i].getIsEnabled())
            {
                return true;
            }
        }
        return false;
    }

    // Set normal scrap active state
    public static void setNormalScrapEnabled(string scrapName, bool setEnable)
    {
        int atStart = getFileRelevantText().IndexOf(beforeNormalIndicator);
        int atEnd = getFileRelevantText().IndexOf(afterNormalIndicator);
        string beforeStates = getFileRelevantText().Substring(atStart, atEnd - atStart);
        string inStates = beforeStates;
        int totalAdvance = atStart;
        while (inStates.Contains('['))
        {
            string prefabName;
            int advanceToName = inStates.IndexOf("[") + 1;
            totalAdvance += advanceToName;
            inStates = inStates.Substring(advanceToName);
            prefabName = inStates.Substring(0, inStates.IndexOf(","));
            if(prefabName == scrapName)
            {
                totalAdvance += inStates.IndexOf(",") + 1;
                string insert;
                if (setEnable)
                {
                    insert = "T";
                }
                else {
                    insert = "F";
                }
                string afterInsert = getFileRelevantText().Substring(totalAdvance);
                setFileRelevantText(getFileRelevantText().Substring(0,totalAdvance) + insert + afterInsert.Substring(afterInsert.IndexOf("]")));
                break;
            }
        }
    }

    // Get golden scrap active state
    public static bool getGoldenScrapEnabled(string scrapName)
    {
        scrapState[] states = getGoldenScrapStates();
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].getScrapPrefab() == scrapName && states[i].getIsEnabled())
            {
                print(scrapName + " is enabled");
                return true;
            }
        }
        print(scrapName + " is disabled");
        return false;
    }

    // Set golden scrap active state
    public static void setGoldenScrapEnabled(string scrapName, bool setEnable)
    {
        int atStart = getFileRelevantText().IndexOf(beforeGoldenIndicator);
        int atEnd = getFileRelevantText().IndexOf(afterGoldenIndicator);
        string beforeStates = getFileRelevantText().Substring(atStart, atEnd - atStart);
        string inStates = beforeStates;
        int totalAdvance = atStart;
        while (inStates.Contains('['))
        {
            string prefabName;
            int advanceToName = inStates.IndexOf("[") + 1;
            totalAdvance += advanceToName;
            inStates = inStates.Substring(advanceToName);
            prefabName = inStates.Substring(0, inStates.IndexOf(","));
            if (prefabName == scrapName)
            {
                totalAdvance += inStates.IndexOf(",") + 1;
                string insert;
                if (setEnable)
                {
                    insert = "T";
                }
                else
                {
                    insert = "F";
                }
                string afterInsert = getFileRelevantText().Substring(totalAdvance);
                setFileRelevantText(getFileRelevantText().Substring(0, totalAdvance) + insert + afterInsert.Substring(afterInsert.IndexOf("]")));
                break;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        normalPrefabsStatic = normalScrapPrefabs;
        goldenPrefabsStatic = goldenScrapPrefabs;
        if(getText() == "")
        {
            revertCurrentScrap();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
