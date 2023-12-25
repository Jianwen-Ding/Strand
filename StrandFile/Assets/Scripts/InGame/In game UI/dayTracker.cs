using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class dayTracker : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textCache;
    // Start is called before the first frame update
    void Start()
    {
        textCache = gameObject.GetComponent<TextMeshProUGUI>();
        textCache.text = PlayerPrefs.GetInt("daysSpent", 0) + "";
    }
}
