using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class debugText : MonoBehaviour
{
    //Setup
    TextMeshProUGUI displayText;
    //Displayed Variables
    string applicationPath;
    // Start is called before the first frame update
    void Start()
    {
        displayText = gameObject.GetComponent<TextMeshProUGUI>();
        applicationPath = Application.dataPath;
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = "DEBUG MODE TEXT \napplication path: " + applicationPath;
    }
}
