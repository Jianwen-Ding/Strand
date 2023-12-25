using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class foodMeterUI : MonoBehaviour
{
    [SerializeField]
    float leftX;
    [SerializeField]
    float maxRightX;
    [SerializeField]
    Color normalMode;
    [SerializeField]
    Color stalkerMode;
    resourceSystem getResource;
    Image getImage;
    RectTransform getRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        getResource = Camera.main.gameObject.GetComponent<resourceSystem>();
        getRectTransform = gameObject.GetComponent<RectTransform>();
        getImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        float actualRightX = leftX + (maxRightX - leftX)* (getResource.getHungerMeter() / getResource.getMaxHungerMeter());
        transform.localPosition = new Vector3((leftX + actualRightX) / 2, transform.localPosition.y, transform.localPosition.z);
        getRectTransform.sizeDelta = new Vector3((-leftX + actualRightX), getRectTransform.sizeDelta.y);
        if (getResource.getStalkHunger())
        {
            getImage.color = stalkerMode;
        }
        else
        {
            getImage.color = normalMode;
        }
    }
}
