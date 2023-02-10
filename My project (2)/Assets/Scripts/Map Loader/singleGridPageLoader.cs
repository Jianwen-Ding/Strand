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
    //-- parameters to load and unload with based on distance--
    ArrayList loadedObject = new ArrayList();
    // Start is called before the first frame update
    void Start()
    {
        //gets needed variables
        pageMap = gridPageStorer.findRandomSuitablePage(upOpen, downOpen, leftOpen, rightOpen, pageTheme, pageSpecialUse).getMap();
        pageKey = gridPageStorer.getGridKey();
        //loads page
        for(int y = 0; y < pageMap.Length; y++)
        {
            for(int x = 0; x < pageMap[y].Length; x++)
            {
                GameObject newObject = Instantiate(pageKey[pageMap[y][x]], new Vector3(initialDisplacement.x + gameObject.transform.position.x + gridXDistance * x, initialDisplacement.y + gameObject.transform.position.y + gridYDistance * y), Quaternion.identity.normalized);
                loadedObject.Add(newObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
