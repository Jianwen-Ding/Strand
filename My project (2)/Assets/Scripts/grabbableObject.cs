using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbableObject : MonoBehaviour
{
    [SerializeField]
    private bool angleChange;
    [SerializeField]
    private float grabShrink;
    [SerializeField]
    private float throwStrength;
    [SerializeField]
    private bool hasBeenGrabbed;
    public bool getHasBeenGrabbed()
    {
        return hasBeenGrabbed;
    }
    public void setHasBeenGrabbed(bool set)
    {
        hasBeenGrabbed = set;
    }
    public virtual void grabbed()
    {
        
    }
    public virtual void whileGrabbed()
    {

    }
    public virtual void released()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
