using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterSprite : MonoBehaviour
{
    [SerializeField]
    Sprite[] progressSprites;
    [SerializeField]
    int[] progressThreshold;
    [SerializeField]
    SpriteRenderer objectRenderer;
    [SerializeField]
    resourceSystem resource;
    // Start is called before the first frame update
    void Start()
    {
        resource = FindObjectOfType<resourceSystem>();
        objectRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Sprite givenSprite = progressSprites[0];
        for(int i = 0; i < progressThreshold.Length; i++)
        {
            if (progressThreshold[i] <= resource.getScrapCollected())
            {
                givenSprite = progressSprites[i];
            }
        }
        objectRenderer.sprite = givenSprite;
    }
}
