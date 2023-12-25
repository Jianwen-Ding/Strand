using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class scrapStorer : MonoBehaviour
{
    [SerializeField]
    private TextAsset baseScrapFile;
    private string pathAddOn = "currentSave.txt";
    private string currentFilePath;
    private const string beforeScrapCollectedIndicator = "TOTAL SCRAP COLLECTED";
    private const string beforeEscapesIndicator = "TIMES ESCAPED";
    private const string beforeGoldenIndicator = "<GOLDEN>";
    private const string beforeNormalIndicator = "<NORMAL>";
    [SerializeField]
    GameObject[] normalScrapPrefabs;
    [SerializeField]
    GameObject[] goldenScrapPrefabs;

    public void revertCurrentScrap()
    {
        File.WriteAllText(currentFilePath, baseScrapFile.text);
    }

    private void deleteFile()
    {
        File.Delete(currentFilePath);
    }
    
    private string getText()
    {
        string totalText;
        using (StreamReader outputFile = new StreamReader(currentFilePath))
        {
            totalText = outputFile.ReadToEnd();
        }
        return totalText;
    }
    private string getFileRelevantText()
    {
        string totalText = getText();
        return totalText.Substring(totalText.IndexOf("===========DATA==========="));
    }

    private void setFileRelevantText(string relevantText)
    {
        string totalText = getText();
        using (StreamWriter outputFile = new StreamWriter(currentFilePath))
        {
            outputFile.WriteLine(totalText.Substring(0,totalText.IndexOf("===========DATA===========")) + relevantText);
        }
    }
    public int getScrap()
    {
        string beforeScrap = getFileRelevantText().Substring(getFileRelevantText().IndexOf(beforeScrapCollectedIndicator));
        beforeScrap = beforeScrap.Substring(beforeScrap.IndexOf("|") + 1);
        return int.Parse(beforeScrap.Substring(0, beforeScrap.IndexOf("|")));
    }
    // Start is called before the first frame update
    void Start()
    {
        currentFilePath = Path.Combine(Application.streamingAssetsPath, pathAddOn);
        if (getText() == "")
        {
            using (StreamWriter outputFile = new StreamWriter(currentFilePath))
            {
                outputFile.Write(baseScrapFile.text);
            }
        }
        print(getText());
        if(getScrap() == 0)
        {
            Time.timeScale = 5;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
