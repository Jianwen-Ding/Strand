using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameSave : MonoBehaviour
{
    [SerializeField]
    private static string saveInformation = "null";
    //File Location of TXT file
    [SerializeField]
    private const string AccessSaveInfoPath = "grid_pages";
    [SerializeField]
    private const string AccessSaveInfoPathFull = "/grid_pages.txt";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
