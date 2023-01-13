using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteYLayering : MonoBehaviour
{
    private SpriteRenderer objectSpriteRender;
    // Start is called before the first frame update
    void Start()
    {
        objectSpriteRender = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        objectSpriteRender.sortingOrder = -(int)gameObject.transform.position.y;
    }
}
