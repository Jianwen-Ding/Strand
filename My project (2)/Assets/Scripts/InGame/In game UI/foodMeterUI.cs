using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodMeterUI : MonoBehaviour
{
    [SerializeField]
    float leftX;
    [SerializeField]
    float maxRightX;
    resourceSystem getResource;
    RectTransform getRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        getResource = Camera.main.gameObject.GetComponent<resourceSystem>();
        getRectTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float actualRightX = leftX + (maxRightX - leftX)* (getResource.getHungerMeter() / getResource.getMaxHungerMeter());
        transform.localPosition = new Vector3((leftX + actualRightX) / 2, transform.localPosition.y, transform.localPosition.z);
        getRectTransform.sizeDelta = new Vector3((-leftX + actualRightX), getRectTransform.sizeDelta.y);
    }
}
