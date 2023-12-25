using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpriteLoader : MonoBehaviour
{
    [SerializeField]
    private Sprite[] possibleSpirtes;
    [SerializeField]
    private int randomSet;
    private SpriteRenderer renderSet;

    // Start is called before the first frame update
    void Start()
    {
        renderSet = gameObject.GetComponent<SpriteRenderer>();
        randomSet = Random.Range(0, possibleSpirtes.Length);
        renderSet.sprite = possibleSpirtes[Random.Range(0, possibleSpirtes.Length - 1)];
        this.enabled = false;
    }
}
