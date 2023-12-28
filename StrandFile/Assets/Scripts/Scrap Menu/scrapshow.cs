using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class scrapshow : MonoBehaviour
{
    TextMeshProUGUI objectText;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        objectText = gameObject.GetComponent<TextMeshProUGUI>();
        objectText.text = "Scrap\nStored:\n" + scrapStorer.getScrap();
    }
    private void Update()
    {
        objectText.text = "Scrap\nStored:\n" + scrapStorer.getScrap();
    }
}
