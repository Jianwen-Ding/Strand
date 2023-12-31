using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogCollider : MonoBehaviour
{
    [SerializeField]
    bool leftFace;
    dogEnemy dogScript;
    // Start is called before the first frame update
    void Start()
    {
        dogScript = transform.parent.gameObject.GetComponent<dogEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != transform.parent.gameObject)
        {
            dogScript.facingLeftSet(leftFace);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject != transform.parent.gameObject)
        {
            dogScript.facingLeftSet(leftFace);
        }
    }
}
