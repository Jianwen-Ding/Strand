using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class taskCreatorScrapCounter : taskCreator
{
    [SerializeField]
    int scrapThreshold;
    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("Scrap",0) >= scrapThreshold)
        {
            attemptSignalTaskComplete();
        }
    }
}
