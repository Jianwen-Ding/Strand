using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sceneEnterFadeIn : MonoBehaviour
{
    [SerializeField]
    float transitionSpeed;
    Image objectImage;
    // Start is called before the first frame update
    void Awake()
    {
        objectImage = gameObject.GetComponent<Image>();
        objectImage.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(objectImage.color.a - Time.unscaledDeltaTime * transitionSpeed <= 0)
        {
            objectImage.color = new Color(objectImage.color.r, objectImage.color.g, objectImage.color.b, objectImage.color.a - 0);
            Destroy(gameObject);
        }
        else
        {
            objectImage.color = new Color(objectImage.color.r, objectImage.color.g, objectImage.color.b, objectImage.color.a - Time.unscaledDeltaTime * transitionSpeed);
        }
    }
}
