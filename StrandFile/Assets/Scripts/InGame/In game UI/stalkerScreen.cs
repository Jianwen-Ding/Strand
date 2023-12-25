using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class stalkerScreen : MonoBehaviour
{
    resourceSystem overallResources;
    [SerializeField]
    float opacityConvertSpeed;
    [SerializeField]
    Image cacheTopImage;
    [SerializeField]
    Image cacheBottomImage;
    [SerializeField]
    float baseOpacity;
    float currentOpacity = 0;
    [SerializeField]
    AudioSource cacheAudioSource;
    // Start is called before the first frame update
    void Start()
    {
        overallResources = Camera.main.gameObject.GetComponent<resourceSystem>();
        if (overallResources.getStalkHunger())
        {
            cacheAudioSource.Play();
        }
        else
        {
            cacheAudioSource.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (overallResources.getStalkHunger())
        {
            if (currentOpacity < baseOpacity)
            {
                currentOpacity += Time.deltaTime * opacityConvertSpeed;
                if (currentOpacity >= baseOpacity)
                {
                    cacheAudioSource.Play();
                    currentOpacity = baseOpacity;
                }
            }
        }
        else {
            if (currentOpacity > 0)
            {
                currentOpacity -= Time.deltaTime * opacityConvertSpeed;
                if (currentOpacity <= 0)
                {
                    cacheAudioSource.Pause();
                    currentOpacity = 0;
                }
            }
        }
        cacheTopImage.color = new Color(cacheTopImage.color.r, cacheTopImage.color.g, cacheTopImage.color.b, currentOpacity);
        cacheBottomImage.color = new Color(cacheBottomImage.color.r, cacheBottomImage.color.g, cacheBottomImage.color.b, currentOpacity);
    }
}
