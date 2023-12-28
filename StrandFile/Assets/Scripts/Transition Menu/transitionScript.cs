using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class transitionScript : MonoBehaviour
{
    TextMeshProUGUI objectText;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        objectText = gameObject.GetComponent<TextMeshProUGUI>();
        objectText.text = "Total\nScrap\nCollected\n" + scrapStorer.getScrap() + "\n\nTotal\nEscapes\nRecorded\n" + scrapStorer.getEscapes();
    }
}
