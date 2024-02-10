using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleInitialVelocity : MonoBehaviour
{
    [SerializeField]
    Vector2 initialVelocity;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }
}
