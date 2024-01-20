using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Animator>().keepAnimatorStateOnDisable = true;
    }
}
