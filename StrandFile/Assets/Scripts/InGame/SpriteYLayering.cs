using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteYLayering : MonoBehaviour
{
    //This script makes it so that there are not any layering/perspective errors
    [SerializeField]
    private bool unmoving;
    [SerializeField]
    private float yAdjust;
    private SpriteRenderer objectSpriteRender;
    public float getAdjust()
    {
        return yAdjust;
    }
    public void setAdjust(float adjust)
    {
        yAdjust = adjust;
    }
    // Start is called before the first frame update
    void Start()
    {
        objectSpriteRender = gameObject.GetComponent<SpriteRenderer>();
        if (unmoving)
        {
            objectSpriteRender.sortingOrder = -(int)(50 * (gameObject.transform.position.y + yAdjust));
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        objectSpriteRender.sortingOrder = -(int)(50 * (gameObject.transform.position.y + yAdjust));
    }
}
